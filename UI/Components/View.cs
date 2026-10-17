using MonoCoC.UI.Framework;

namespace MonoCoC.UI.Components
{
    public class View : Component
    {
        private int _buttonCount = 0;

        public virtual Button NextButton()
        {
            return new Button($"button{_buttonCount++ % 10}");
        }
    }
}