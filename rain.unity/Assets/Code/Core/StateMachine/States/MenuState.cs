using System;
using Core.Scenes;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.StateMachine.States
{
    public class MenuState : State
    {
        private readonly ISceneCurtain _sceneCurtain;

        public MenuState(Action<Type> requestTransition, ISceneCurtain sceneCurtain) : base(requestTransition)
        {
            _sceneCurtain = sceneCurtain;
        }

        public override async UniTask Enter()
        {
            await _sceneCurtain.Show();
            await SceneManager.LoadSceneAsync(EScene.Menu.ToString());
            await _sceneCurtain.Hide();
        }

        public override UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}