using Code.UI.Components.Selected;
using Core.GameLoop;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.UI.Components.Buttons
{
    public class UIScrollButtons<T> : Essential.Mono, IInitializeListener where T : UISelectable
    {
        public bool IsInitialized { get; set; }
        [field: SerializeField] public UIElementPool<T> Pool { get; private set; }

        
        public UniTask Initialize()
        {
            Pool.Initialize();
            
            return UniTask.CompletedTask;
        }
    }
}