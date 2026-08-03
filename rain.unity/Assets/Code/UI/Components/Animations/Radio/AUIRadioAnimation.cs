using System;

namespace Code.UI.Components.Animations
{
    [Serializable]
    public abstract class AUIRadioAnimation
    {
        public abstract void InvokeCheckAnimation();
        
        public abstract void InvokeUnCheckAnimation();
    }
}