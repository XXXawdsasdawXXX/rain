using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Essential.Data.RangeFloat;
using Essential.Extensions;
using TriInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Code.Game.World
{
    [Serializable]
    public class ObstacleRender
    {
        [Serializable]
        public class Model
        {
            [field: SerializeField, MinMaxRangeFloat(0, 3)]
            public RangedFloat FrameDuration { get; private set; }

            [field: SerializeField] public bool IsLoop { get; private set; }
            [field: SerializeField] public bool RandomizedFrame { get; private set; }
            [field: SerializeField] public Sprite[] Sprites { get; private set; }
        }

        [SerializeField] protected Transform root;
        [SerializeField] protected MatrixTriggerZone matrixTrigger;
        [SerializeField] protected List<SpriteRenderer> renderers;
        [SerializeField] protected Model idleModel;

        [SerializeField] protected bool isHasReactions;

        [ShowInInspector, ShowIf(nameof(isHasReactions))]
        protected Model[] reactionModels;

        [SerializeField] protected bool isCanBeDestroyed;

        [ShowInInspector, ShowIf(nameof(isCanBeDestroyed))]
        protected Model destroyeModel;

        [Title("runtime")] [ShowInInspector, ReadOnly]
        protected EObstacleState state;

        protected CancellationTokenSource _cts;
        private int _spriteIndex;


        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        public virtual void PlayIdle()
        {
            playSpriteSequence(idleModel, EObstacleState.Idle);
        }

        public virtual void PlayRandomReaction()
        {
            if (isHasReactions && reactionModels.Length > 0)
            {
                int modelIndex = Random.Range(0, reactionModels.Length);
                playSpriteSequence(reactionModels[modelIndex], EObstacleState.Reaction);
            }
        }

        public virtual void PlayReaction(int reactionIndex)
        {
            if (isHasReactions && reactionModels.Length > reactionIndex)
            {
                playSpriteSequence(reactionModels[reactionIndex], EObstacleState.Reaction);
            }
        }

        public virtual void PlayDestroy()
        {
            if (isCanBeDestroyed)
            {
                playSpriteSequence(destroyeModel, EObstacleState.Destroyed);
            }
        }

        protected virtual async void playSpriteSequence(Model model, EObstacleState sequenceState)
        {
            Dispose();

            state = sequenceState;
            _cts = new CancellationTokenSource();
            _spriteIndex = 0;

            do
            {
                foreach (SpriteRenderer spriteRenderer in renderers)
                {
                    spriteRenderer.sprite = model.Sprites[
                        model.RandomizedFrame
                            ? Random.Range(0, renderers.Count)
                            : _spriteIndex];

                    TimeSpan randomizeDelay = TimeSpan.FromSeconds(Random.Range(0, 0.3f));

                    await UniTask.Delay(
                            randomizeDelay,
                            cancellationToken: _cts.Token)
                        .SuppressCancellationThrow();

                    if (_cts.IsCancellationRequested)
                    {
                        break;
                    }
                }

                _spriteIndex++;

                if (_spriteIndex >= model.Sprites.Length)
                {
                    _spriteIndex = 0;

                    if (!model.IsLoop)
                    {
                        Dispose();
                        break;
                    }
                }

                TimeSpan duration = TimeSpan.FromSeconds(model.FrameDuration.GetRandomValue());

                await UniTask.Delay(
                        duration,
                        cancellationToken: _cts.Token)
                    .SuppressCancellationThrow();

                if (_cts.IsCancellationRequested)
                {
                    break;
                }
            } while (state == sequenceState && !_cts.IsCancellationRequested);
        }


#if UNITY_EDITOR


        [Flags]
        private enum ERenderType
        {
            Tile,
            RandomizeSprite
        }

        [Title("editor")] [SerializeField] private ERenderType _renderTypes;


        [Button]
        private void _updateObstaclePosition(Tilemap tilemap)
        {
            Vector3Int cellPosition = tilemap.WorldToCell(root.position);
            Vector3 worldPosition = tilemap.GetCellCenterWorld(cellPosition);

            root.transform.position = worldPosition;

            EditorUtility.SetDirty(root);
        }

        [Button]
        private void _updateView()
        {
            _clearRenders();

            List<Vector2Int> enabledCells = new();
            matrixTrigger.Cells.GetEnabledCells(enabledCells);

            for (int i = 0; i < enabledCells.Count; i++)
            {
                Vector2Int cell = enabledCells[i];
                SpriteRenderer renderer = i == 0 ? renderers[0] : Object.Instantiate(renderers[0], root);
                Vector3 position = matrixTrigger.GetCellCenterWorld(cell.x, cell.y).AsVector3();
                renderer.transform.position = position;

                if (renderers.Contains(renderer))
                {
                    continue;
                }

                renderers.Add(renderer);
            }

            EditorUtility.SetDirty(root);
        }

        [Button]
        private void _clearRenders()
        {
            _findSpriteRenderersInChildren();

            for (int i = renderers.Count - 1; i >= 1; i--)
            {
                SpriteRenderer renderer = renderers[i];
                renderers.RemoveAt(i);
                Object.DestroyImmediate(renderer.gameObject);
            }

            EditorUtility.SetDirty(root);
        }

        [Button]
        private void _findSpriteRenderersInChildren()
        {
            renderers.Clear();
            renderers = root.GetComponentsInChildren<SpriteRenderer>(true).ToList();
            EditorUtility.SetDirty(root);
        }
#endif
    }
}