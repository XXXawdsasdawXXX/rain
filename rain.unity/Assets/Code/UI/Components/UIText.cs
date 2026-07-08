using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Code.UI.Components
{
    public class UIText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private LocalizedString _localizedString = new();
        
        private object[] _args;
        
        
        private void OnEnable()
        {
            if (_isValidLocalization())
            {
                LocalizationSettings.SelectedLocaleChanged += _onSelectedLocalChanged;
                _updateLocalizationText();
            }
        }

        private void OnDisable()
        {
            if (_isValidLocalization())
            {
                LocalizationSettings.SelectedLocaleChanged += _onSelectedLocalChanged;
            }
        }

        public void SetLocalizationArguments(params object[] args)
        {
            _args = args;
        }

        private void _onSelectedLocalChanged(Locale _)
        {
            _updateLocalizationText();
        }

        private void _updateLocalizationText()
        {
            _text.SetText(_args == null
                ? _localizedString.GetLocalizedString()
                : _localizedString.GetLocalizedString(_args));
        }

        private bool _isValidLocalization()
        {
            return _localizedString != null &&
                   _localizedString.TableReference.ReferenceType != TableReference.Type.Empty &&
                   _localizedString.TableEntryReference.ReferenceType != TableEntryReference.Type.Empty;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_text == null)
            {
                gameObject.TryGetComponent(out _text);
            }
        }
#endif
    }
}
