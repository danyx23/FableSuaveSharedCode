# Suave / Fable shared code example

This is the backend of the example. It is a simple Suave webserver app that only replies at the /test endpoint to a POST request. If you give it a Cart instance (serialized as json), it will caluclate the price and return that as a double value. CORS headers are added so that the request will work from a browser/electron.

This backend of the example builds heavily on the example/configuration from [Thomas Petricek's Social Drawing example](https://github.com/tpetricek/SocialDrawing).

### Building/Running

On Windows, make sure the .NET Framework and F# compiler are installed (e.g. by installing Visual Studio 2015), on Linux/OSX you need Mono installed (see http://fsharp.org/ for details).

Then run `build.cmd` on windows, `./build.sh` on linux/osx to install dependencies and build the project. Finally, start the server with `.\build\SuaveFableTest.exe` or `mono ./build/SuaveFableTest.exe`