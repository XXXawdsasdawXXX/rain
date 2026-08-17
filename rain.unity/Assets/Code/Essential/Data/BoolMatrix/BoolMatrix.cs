using System;
using System.Collections.Generic;
using UnityEngine;

namespace Essential.Data.BoolMatrix
{
    /// <summary>
    /// Editable W×H bool grid for the Inspector. Row-major: index = y * Width + x.
    /// (0,0) is bottom-left when used as a world mask.
    /// </summary>
    [Serializable]
    public class BoolMatrix
    {
        [SerializeField] private int _width = 4;
        [SerializeField] private int _height = 4;
        [SerializeField] private bool[] _values = new bool[16];

        public int Width => Mathf.Max(0, _width);
        public int Height => Mathf.Max(0, _height);
        public int Count => Width * Height;

        public bool this[int x, int y]
        {
            get => Get(x, y);
            set => Set(x, y, value);
        }

        public BoolMatrix()
        {
            Resize(4, 4);
        }

        public BoolMatrix(int width, int height)
        {
            Resize(width, height);
        }

        public bool Get(int x, int y)
        {
            if (!IsInBounds(x, y))
            {
                return false;
            }

            EnsureArray();
            return _values[ToIndex(x, y)];
        }

        public void Set(int x, int y, bool value)
        {
            if (!IsInBounds(x, y))
            {
                return;
            }

            EnsureArray();
            _values[ToIndex(x, y)] = value;
        }

        public bool IsInBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }

        public void Resize(int width, int height)
        {
            width = Mathf.Max(0, width);
            height = Mathf.Max(0, height);

            bool[] next = new bool[width * height];
            int copyW = Mathf.Min(Width, width);
            int copyH = Mathf.Min(Height, height);

            if (_values != null)
            {
                for (int y = 0; y < copyH; y++)
                {
                    for (int x = 0; x < copyW; x++)
                    {
                        int oldIndex = y * _width + x;
                        if (oldIndex < _values.Length)
                        {
                            next[y * width + x] = _values[oldIndex];
                        }
                    }
                }
            }

            _width = width;
            _height = height;
            _values = next;
        }

        public void Clear(bool value = false)
        {
            EnsureArray();
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = value;
            }
        }

        public void GetEnabledCells(List<Vector2Int> results)
        {
            results.Clear();
            EnsureArray();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (_values[ToIndex(x, y)])
                    {
                        results.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        public int ToIndex(int x, int y)
        {
            return y * Width + x;
        }

        private void EnsureArray()
        {
            int needed = Count;
            if (_values != null && _values.Length == needed)
            {
                return;
            }

            Resize(Width, Height);
        }
    }
}
