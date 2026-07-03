using Core.Save;
using Core.Save.Models;
using Cysharp.Threading.Tasks;

namespace Code.UI.Windows.Base
{
    public interface IWindowController
    {
        public bool IsDetached { get; }
        
        public UniTask InitializeWindow(UIWindowManager manager);
        
        public void LoadWindow(GameModel game);

        public void StartWindow();

        public void SubscribeToEvents(bool flag);
        
        public void Open();

        public void Close();
    }
}