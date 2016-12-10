open Suave                 // always open suave
open Suave.Web             // for config
open Suave.Writers
open Suave.Filters
open Suave.Successful
open Suave.Operators
open Newtonsoft.Json
open FSharp.Core
open DiscountSample.Shared


type Error =
  { Error : string
  }

let discounts = [ twelveItemsDiscount ]

let serializeJson obj =
  let converter = new Fable.JsonConverter()
  JsonConvert.SerializeObject(obj, converter)

let deserializeCart str =
  let converter = new Fable.JsonConverter()
  let deserialized = JsonConvert.DeserializeObject<Cart>(str, converter)
  let boxed = box deserialized
  match boxed with
    | null ->
      None
    | other ->
      Some deserialized


let deserializeCalculateSerialize json =
  let deserialized =
    json
    |> deserializeCart

  match deserialized with
    | Some cart ->
      cart
        |> calculatePrice discounts
        |> serializeJson

    | None ->
      { Error = "Could not deserialize" }
      |> serializeJson

let mapJson f =
  request(fun r ->
    System.Text.Encoding.UTF8.GetString(r.rawForm)
    |> f
    |> Successful.OK)

let setCORSHeaders =
    setHeader  "Access-Control-Allow-Origin" "*"
    >=> setHeader "Access-Control-Allow-Headers" "content-type"

let allow_cors : WebPart =
    choose [
        OPTIONS >=>
            fun context ->
                context |> (
                    setCORSHeaders
                    >=> OK "CORS approved" )
    ]

let app =
  choose [
    allow_cors
    POST >=> path "/test" >=> (mapJson deserializeCalculateSerialize) >=> Writers.setMimeType "application/json"
  ]

startWebServer defaultConfig app
