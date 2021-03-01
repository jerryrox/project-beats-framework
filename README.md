# project-beats-framework
Reusable, core framework behind Project: Beats
  
## This project is a huge WIP.
Slowly making progress 🧩

## Dependencies
- Newtonsoft.Json (Tested with net45 version)
- ICSharpCode.SharpZipLib

## Development progress
[https://trello.com/b/5gpuJrRa/project-beats-renewed]

## Versions
### 1.3.1 (WIP)

### 1.3.0
#### New features
- Added "verbose" and "info" log types in replacement of the old "normal" type logs.
#### Improvements
- Exposed a function in `InputManager` to dynamically change base screen resolution.
- Changed database data formatting option to non-pretty printing.
- Added an `UnbindAll` method for Bindables to remove all events attached to them.
- Added `MaxCursorCount` property for InputManager.

### 1.2.1
#### New features
- Added a new ITask implementation for UnityEngine.AsyncOperation.
#### Fixes
- Fixed cursor inputs' processed position not being accurate when screen size is changed during run-time.

### 1.2.0
#### Improvements
- Added `Bind` and `Unbind` methods to Bindable for semantic assistance.
- Added ability to retrieve Color from ColorPalette using a custom alpha.
#### Changes
- Changed the way manual tests are triggered in test environment.
- Added UNITY_EDITOR preprocessor to Testing namespace.

### 1.1.0
#### New features
- Added `TaskListener` and `ITask`.
- Added `SychronizedBool` class.
- Added `TestExtensions`.
#### Changes
- Refactored Cacher.
- Removed `Promise` and `Progress`.
- Increased the base timeout time for AssetRequest.

### 1.0.4
#### New features
- Added the ability to directly interact with the inner value of a bindable and automatically call Trigger.
- Added ability for a bindable to bind to another bindable.
#### Change
- Made IRaycastable part of higher level UI interfaces, not implementation objects.

### 1.0.3.1
#### Fixes
- Fixed platform host not being created for iOS.

### 1.0.3
#### New features
- Added DeepLinker module.
- Added a method to cache a dependency into a dependency container and inject afterwards.
#### Improvements
- Moved all URL manipulation / parsing logic to WebLink class.
#### Changes
- Make IGraphicObject.CreateChild automatically increment depth based on the number of children if depth is not specified.
- Reverted AudioClock time source value back to realtimeSinceStartup.
- Moved Services.UnityThreadService to Threading.UnityThread.
- Moved Services.AnimeService to Animations.AnimeService.
- Removed type parameter on dependency container's Inject method. This should've fixed the bug where injecting on an object referred with an interface fails.
#### Fixes
- Fixed warning which kept appearing when cloning dependency containers.

### 1.0.2
#### New features
- Support setting bindable value without triggering.
- Support manually triggering bindable with a custom previous value.
- Support listening to bindable state without receing previous state.
- Added Contains<T>() for IDependencyContainer.
- Added a Testing namespace for better testing with the UI objects.
#### Changes
- Replacement of ScreenNavigator.OnScreenChange with bindable CurrentScreen value.
- Moved FontManager from game to framework.
- Changed the reference for audio time tracking.
#### Fixes
- Fixed scrollview continuing to move even after a drag has occurred while playing scroll animation via ScrollTo call.
#### Others

### 1.0.1
#### Changes
- Make UnityAudio effect/music controllers instantiated on a single object instead of multiple.
#### Others
- Cleaned up README.