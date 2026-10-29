using System;
using System.Threading;
using Code.Game.Save;
using Core.Scenes;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Code.Game.StateMachine.States
{
    public class MenuState : AState
    {
        private readonly ISceneCurtain _sceneCurtain;
        private readonly GameSaveRepository _gameSave;

        private CancellationTokenSource _cts;
        
        
        public MenuState(
            Action<Type> requestTransition, 
            ISceneCurtain sceneCurtain, 
            GameSaveRepository gameSave) : base(requestTransition)
        {
            _sceneCurtain = sceneCurtain;
            _gameSave = gameSave;
        }

        public override async UniTask Enter()
        {
            await SceneManager.LoadSceneAsync(EScene.Menu.ToString()).ToUniTask();
            
            await _sceneCurtain.Hide();
        }

        public override async UniTask Exit()
        {
            if (_gameSave.WasChanged)
            {
                _cts = new CancellationTokenSource();

                await _gameSave.SaveAsync(_cts.Token);
                
                _cts.Dispose();
            }
            
            await _sceneCurtain.Show();
        }
    }
}