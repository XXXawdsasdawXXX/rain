using System;
using DG.Tweening;
using UnityEngine;

namespace Plugins.Demigiant.DOTween.Modules
{
    [Serializable]
    public struct ColorTweenData
    {
        public Color Color;
        public float Duration;
        public Ease Ease;
    }
}