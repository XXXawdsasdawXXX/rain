using System;
using System.Collections.Generic;
using System.Linq;
using Essential;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace Code.UI.Components
{
    public class UIRadioGroup : Essential.Mono
    {
        public event Action<int> Checked;

        [SerializeField] private MonoPool<UIRadioButton> _pool;
        [SerializeField, Min(0)] private int _maxSelectedCount = 1;
      
        [SerializeField] private bool _checkedOnAwake;
        [SerializeField, Min(0), ShowIf(nameof(_checkedOnAwake))]
        private int _defaultSelectedIndex;
       
        private readonly Queue<UIRadioButton> _checked = new();

        private void Awake()
        {
            _pool.DisableAll();
        }

        public void Start()
        {
            if (_checkedOnAwake)
            {
                SetCheckedWithoutNotify(_defaultSelectedIndex);
            }
        }

        private void OnEnable()
        {
            _subscribe();
        }

        private void OnDisable()
        {
            _unsubscribe();
        }

        public T AddRadioButton<T>() where T : Essential.Mono
        {
            UIRadioButton button = _pool.GetNext();
            
            int index = _pool.GetAllEnabled().Count() - 1;
         
            button.Clicked += () => 
            {
                _setChecked(index);
            };

            return button as T;
        }


        public bool SetCheckedWithoutNotify(int index)
        {
            if (index >= _pool.GetAllEnabled().Count())
            {
                return false;
            }

            if (index < 0)
            {
                foreach (UIRadioButton uiRadioButton in _checked)
                {
                    uiRadioButton.SetValueWithoutNotify(false);
                }

                return false;
            }
            
            if (_checked.Count >= _maxSelectedCount && _checked.Count > 0)
            {
                UIRadioButton button = _checked.Dequeue();
                button.UnCheck();
            }

            UIRadioButton selectButton = _pool.GetAllEnabled()[index];
            selectButton.Check();
            _checked.Enqueue(selectButton);

            return true;
        }

        private void _setChecked(int index)
        {
            if (index < 0)
            {
                foreach (UIRadioButton uiRadioButton in _checked)
                {
                    uiRadioButton.SetValueWithoutNotify(false);
                }

                return;
            }
            
            if (SetCheckedWithoutNotify(index))
            {
                Checked?.Invoke(index);
            }
        }
        
        private void _subscribe()
        {
            for (int i = 0; i < _pool.GetAllEnabled().Count; i++)
            {
                UIRadioButton button = _pool.GetAllEnabled()[i];
                
                button.SetValueWithoutNotify(false);
                int index = i;
              
                button.Clicked += () => 
                {
                    if (!button.IsChecked.Value)
                    {
                        _setChecked(index);
                    }
                };
            }
        }

        private void _unsubscribe()
        {
            foreach (UIRadioButton button in _pool.GetAllEnabled())
            {
                button.ClearSubscriptions();
            }
        }

        #region Editor

#if UNITY_EDITOR
        

        [Button]
        private void _updateIndex()
        {
            List<UIRadioButton> buttons = _pool.GetAllEnabled();
            
            if (buttons == null || buttons.Count == 0)
            {
                return;
            }

            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i] == null)
                {
                    break;
                }

                buttons[i].SetIndex(i);
                EditorUtility.SetDirty(buttons[i]);
            }

            EditorUtility.SetDirty(this);
        }

        private void OnValidate()
        {
            _updateIndex();
        }

#endif
        #endregion

    }
}