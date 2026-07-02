using Core.AssetManagement;
using Core.StateMachine;
using UnityEngine;

namespace Core.GameLoop
{
    internal sealed class Bootstrap : Essential.Mono
    {
        [SerializeField] private AssetManagement.Installers _installer;
        [SerializeField] private Configurations _configurations;
            
        public void Awake()
        {
            GameStateMachine gameStateMachine = new GameStateMachine(_installer, _configurations);
        }
    }
}