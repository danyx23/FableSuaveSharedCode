#r "../node_modules/fable-core/Fable.Core.dll"
#load "../node_modules/fable-arch/Fable.Arch.Html.fs"
#load "../node_modules/fable-arch/Fable.Arch.App.fs"
#load "../node_modules/fable-arch/Fable.Arch.Virtualdom.fs"
#load "../../backend/src/ClientServerShared.fs"
#r "../node_modules/fable-powerpack/Fable.PowerPack.dll"


open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Browser

open Fable.Arch
open Fable.Arch.App
open Fable.Arch.Html
open DiscountSample.Shared
open Fable.PowerPack

type Status =
  | Shopping
  | TransactionPending
  | BuyingFailed of string
  | BuyingSucceeded of double

type Model =
  { Cart: Cart
    Status: Status
  }

let initModel =
  { Cart = { Quantity = 0 }
    Status = Shopping
  }

type Actions =
  | IncreaseQuantity
  | DecreaseQuantity
  | Buy
  | ServerResponseOk of double
  | ServerResponseError of string
  | SetStatus of Status


// Update
let update model action =
  let model' =
    match action with
    // On input change with update the model value
    | IncreaseQuantity ->
      { model with Cart = { model.Cart with Quantity = model.Cart.Quantity + 1 } }

    | DecreaseQuantity ->
      { model with Cart = { model.Cart with Quantity = System.Math.Max(0, model.Cart.Quantity - 1) } }
    | SetStatus status ->
      { model with Status = status }
    | Buy -> model
    | ServerResponseOk price -> { model with Status = BuyingSucceeded price }
    | ServerResponseError error -> { model with Status = BuyingFailed error }

  let delayedCall h =
    match action with
    | Buy ->
        let promise = Fable.PowerPack.Fetch.postRecord "http://127.0.0.1:8083/test" model.Cart []
                      |> Fable.PowerPack.Promise.bind
                          (fun response ->
                            response.json<double>()
                          )
                      |> Fable.PowerPack.Promise.map
                          (fun price ->
                            h (ServerResponseOk price)
                          )
                      |> Fable.PowerPack.Promise.catch
                          (fun err ->
                            h (ServerResponseError err.Message)
                          )

        h (SetStatus TransactionPending)
     | _ -> ()

  // We return the model, and a list of Actions to execute
  model', delayedCall |> toActionList

let availableDiscounts =
  [ twelveItemsDiscount ]

let view model =
  let infoText =
    match model.Status with
    | Shopping -> "Please make your selection"
    | TransactionPending -> "Waiting for confirmation from server"
    | BuyingFailed msg -> "Sorry, your purchase failed! The reason was: " + msg
    | BuyingSucceeded price -> "The purchase worked, with a final price of " + (price.ToString())

  let counterView quantity =
    div []
      [ label [] [ text ("How many items do you want?") ]
        button [ onMouseClick (fun _ -> IncreaseQuantity) ] [ text "+" ]
        label [] [ text (quantity.ToString()) ]
        button [ onMouseClick (fun _ -> DecreaseQuantity) ] [ text "-" ]
      ]

  let counters =
    [ counterView (model.Cart.Quantity) ]

  let applicableDiscounts =
    List.filter (fun item -> item.Applies model.Cart ) availableDiscounts

  let discountView (discount : Discount) =
    li []
      [ label [] [ text discount.Name ] ]

  let discountsView (discounts : Discount list) =
    ul [ classy "discounts" ]
      (List.map discountView discounts)

  let totalPrice =
    div [] [ text <| "The total price is: " + ((calculatePrice availableDiscounts (model.Cart)).ToString()) ]

  div
    []
    [
      label
        []
        [text "This is your shopping cart: "]
      div
        []
        [ text infoText ]
      br []
      div [] counters
      discountsView applicableDiscounts
      totalPrice
      button
        [ onMouseClick (fun _ -> Buy)
          classy "buy"
        ]
        [ i [ classy "fa fa-bolt" ] []
          text " Buy!"
        ]
    ]

/// Create our application
createApp initModel view update Virtualdom.createRender
|> withStartNodeSelector "#echo"
|> start
