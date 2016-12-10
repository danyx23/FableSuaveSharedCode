# Suave / Fable shared code example

This is the frontend part of the example. It is adapted from the official Fable-Electron Hello-World example and demonstrates an electron app that shares some code with the backend and sends the quantity of items to the server to perform the actual process of "buying". 

To demonstrate the nice styling possibilities with Electron I grabbed a style from [html5up](http://html5up.net) and dropped it into the app. The app also uses the Font Awesome icon font as a proof of concept for the buy button.

## To Use

To clone and run this repository you'll need [Git](https://git-scm.com) and [Node.js](https://nodejs.org/en/download/) (which comes with [npm](http://npmjs.com)) installed on your computer. From your command line:

```bash
# Install fable-compiler (tested with version 0.7.20)
npm install -g fable-compiler

# compile with fable (this will install npm dependencies on first run and might take a while)
fable

# Then start the electron app with (start the backend server first if you want "buying" to work)
npm start
```
