using UnityEngine;

namespace Core.Input
{
    public class CameraView : Essential.Mono
    {
        [field: SerializeField] public Camera Camera { get; private set; }
    }
}