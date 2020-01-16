using PBFramework.Dependencies;

namespace PBFramework.UI
{
    public class UguiProgressBar : UguiSlider {

        [InitWithDependency]
        private void Init()
        {
            thumb.Active = false;

            component.interactable = false;

            SetNoTransition();
        }
    }
}