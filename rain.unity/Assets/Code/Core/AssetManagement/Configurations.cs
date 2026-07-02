using UnityEngine;

namespace Core.AssetManagement
{
    [CreateAssetMenu(fileName = "Installer_Configuration", menuName = "Game/Storage/Config")]
    public class Configurations : ScriptableObject
    {
        [field: SerializeField] public ScriptableObject[] Configs { get; private set; }

        public T Get<T>() where T : ScriptableObject
        {
            foreach (ScriptableObject config in Configs)
            {
                if (config is T typedConfig)
                {
                    return typedConfig;
                }
            }

            return null;
        }
    }
}