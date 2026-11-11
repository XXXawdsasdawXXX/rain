using UnityEngine;

namespace Core.Input
{
    public class CameraView : MonoBehaviour
    {
        [field: SerializeField] public Camera Camera { get; private set; }
    }
}