# project-beats-framework
Reusable, core framework behind Project: Beats
  
## This project is a huge WIP.
Currently, I'm restructuring my original source to make things more organized and scalable.  
Slowly making progress 🧩

## Dependencies
- Newtonsoft.Json (Tested with net45 version)
- ICSharpCode.SharpZipLib

## Development progress
[https://trello.com/b/5gpuJrRa/project-beats-renewed]

## Versions
### 1.0.4 (WIP)
#### New features
- Added the ability to directly interact with the inner value of a bindable and automatically call Trigger.

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