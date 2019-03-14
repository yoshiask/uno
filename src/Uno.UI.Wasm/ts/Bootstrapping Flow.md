# Uno Bootstrapping Flow

1. Html, script & runtime initialization
   1. `require.js`: loading: it's the first thing to load
   2. `mono-config.js`: configuration emitted by Uno & mono tools during compilation
      - Contains runtime settings (debug, list of assemblies...)
   3. `uno-config.js`: configuration emitted by `Uno.Bootstrapper` during compilation
      - Contains emscriptem settings
      - Contains settings for Uno bootstrapper
   4. `uno-bootstrap.js`: first piece of code to run
      - Emscriptem overrides for wasm loading (mono runtime)
   5. `mono.js`: mono runtime initialization.
      - Launch the `Program.Main()` in the assembly
2. `Program.Main()`
   1. This code:
      ```csharp
      Windows.UI.Xaml.Application.Start(_ => _app = new App());
      ```
      (calling `Application.Start()`)
   2. `Application.Start()` will call the javascript (typescript) `windowManager.init()`
      1. Initialization of the _CoreDispatcher_ (`Windows.UI.Core.CodeDispatcher.init()`)
      2. Initialization of the virtual _files storage_ (IndexDB)
      3. Create the `WindowManager` instance:
         - create root `div` element
         - register to the browser `resize` handler
      4. Set the browser title (if defined in manifest)
   3. `Application.Start()` will set the TPL's SynchronizationContext

