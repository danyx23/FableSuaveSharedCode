# F# Suave / Fable shared code example

This is a simple proof of concept of using [F#](http://fsharp.org/) on both the backend and the frontend and of sharing some code between them. Be sure to check out the [blogpost](http://danielbachler.de/2016/12/10/f-sharp-on-the-frontend-and-the-backend.html) that describes the motivation in more detail.

On the frontend side it demonstrates cross-compiling F# to Javascript using [Fable](http://fable.io) and using this Javascript in an electron app. The app is an extremely primitive shopping app that let's the user change the quantity of a single product. It updates the price on every button click and also checks if any of a list of discounts applies. The only discount that is actually implemented is a discount that lowers the price by 90% if the user wants to buy exactly 12 items. The app uses [Fable-Arch](https://github.com/fable-compiler/fable-arch) which reimplements the great [Elm Architecture](https://guide.elm-lang.org/architecture/) in F#. The app is styled with CSS from http://html5up.net by AJ. It uses the Font Awesome icon font to display a lighting bolt icon on the buy button.

The backend is a [Suave](http://suave.io) http server application that listens to POST requests on the /test endpoint. It tries to deserialize the body as JSON to a super primitive shopping Cart type (just a record with a Quantity field), calculates the price with any applicable discounts and the returns just the price as a double if everything worked.

The core "business logic", i.e. the data types for Cart, Discount etc and the logic to test if a discount applies are implemented in a separate file that is included by both the frontend and the backend. By sharing one source of truth between them instead of writing the same logic in two different languages for the two worlds, a big source of errors is prevented.

Check the [frontend](frontend/README.md) and [backend](backend/README.md) directory for more information how to run the two parts and what they were based on.

Many thanks to the many contributors/authors of all the pieces that made this possible and the examples on top of which this demonstration was built!
