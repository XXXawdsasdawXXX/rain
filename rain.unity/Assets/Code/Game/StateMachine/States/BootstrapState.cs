using System;
using System.Threading;
using Code.Game.Save;
using Core.Scenes;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Code.Game.StateMachine.States
{
    public sealed class BootstrapState : AState
    {
        private readonly ISceneCurtain _sceneCurtain;
        private readonly SettingsSaveRepository _settingsSave;
        private readonly GameSaveRepository _gameSave;

        private CancellationTokenSource _cts;
        
        public BootstrapState(Action<Type> requestTransition,
            ISceneCurtain sceneCurtain,
            SettingsSaveRepository settingsSave, GameSaveRepository gameSave) : base(requestTransition)
        {
            _sceneCurtain = sceneCurtain;
            _settingsSave = settingsSave;
            _gameSave = gameSave;
        }

        public override async UniTask Enter()
        {
            await SceneManager.LoadSceneAsync(EScene.Bootstrap.ToString());
            
            _cts = new CancellationTokenSource();
            
            _sceneCurtain.ShowWithOutAnimation();

            await _settingsSave.LoadOrDefaultAsync(_cts.Token);

            await _gameSave.LoadOrDefaultAsync(_cts.Token);
            
            _cts?.Cancel();
            
            requestTransition(typeof(MenuState));
        }

        public override  UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}