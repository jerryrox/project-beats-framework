using PBFramework.Dependencies;

namespace PBFramework.UI
{
    public class UguiProgressBar : UguiSlider, IProgressBar {

        [InitWithDependency]
        private void Init()
        {
            thumb.Active = false;

            component.interactable = false;

            SetNoTransition();
        }
    }
}