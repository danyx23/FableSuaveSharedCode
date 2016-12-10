namespace DiscountSample

module Shared =

  let ItemPrice = 20.0

  type Cart =
    { Quantity: int
    }

  type Discount =
    { Name: string
      Applies: (Cart -> bool)
      ApplyDiscount: (double -> double)
    }

  let doesTwelveItemsDiscountApply cart =
    match cart.Quantity with
    | 12 -> true
    | _ -> false


  let twelveItemsDiscount =
    { Name = "12 Items - 90% off!"
      Applies = doesTwelveItemsDiscountApply
      ApplyDiscount = (fun price -> price * 0.1)
    }

  let calculatePrice (discounts : Discount list) (cart : Cart) : double =
    let discountsThatApply = List.filter (fun discount -> discount.Applies cart) discounts
    let cartCalculationFunction =
      match discountsThatApply with
      | [] -> id
      | x :: xs -> x.ApplyDiscount
    let undiscountedPrice = (double cart.Quantity) * ItemPrice
    cartCalculationFunction undiscountedPrice
