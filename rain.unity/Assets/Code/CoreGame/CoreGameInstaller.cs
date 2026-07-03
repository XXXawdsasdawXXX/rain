using System;
using Code.CoreGame.GameTime;
using Code.CoreGame.Grid;
using Code.CoreGame.Harvest;
using Code.CoreGame.PlayerCamera;
using Core.ServiceLocator;
using UnityEngine;

namespace Code.CoreGame
{
    [CreateAssetMenu(fileName = "Installer_CoreGame", menuName = "Game/Installers/CoreGame")]
    public class CoreGameInstaller : Installer
    {
        public override Type[] GetTypes()
        {
            return new[]
            {
                //world
                typeof(GameTime.GameTime),
                typeof(GridService),
                typeof(WorldMaterialController),
                typeof(CameraController),
                //storages
                typeof(ResourceStorage),
            };
        }
    }
}