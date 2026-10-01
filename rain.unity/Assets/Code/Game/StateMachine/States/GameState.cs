using System;
using Core.Scenes;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Code.Game.StateMachine.States
{
    public sealed class GameState : AState
    {
        private readonly ISceneCurtain _sceneCurtain;

        public GameState(Action<Type> requestTransition, ISceneCurtain sceneCurtain) : base(requestTransition)
        {
            _sceneCurtain = sceneCurtain;
        }

        public override async UniTask Enter()
        {
            await SceneManager.LoadSceneAsync(EScene.Game.ToString()).ToUniTask();
            await _sceneCurtain.Hide();
        }

        public override UniTask Exit()
        {
            return UniTask.CompletedTask;
        }
    }
}