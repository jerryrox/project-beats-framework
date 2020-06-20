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
### 1.0.2 (WIP)
#### New features
- Support setting bindable value without triggering.
- Support manually triggering bindable with a custom previous value.
- Support listening to bindable state without receing previous state.
- Added Contains<T>() for IDependencyContainer.
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