using System;

namespace Code.UI.Components
{
    [Serializable]
    public abstract class AUIHoverAnimation 
    {
        public abstract void InvokeHoverAnimation();
        public abstract void InvokeUnHoverAnimation();
    }
}