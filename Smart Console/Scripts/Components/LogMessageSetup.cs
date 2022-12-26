using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SmartConsole.Components
{
    public class LogMessageSetup : MonoBehaviour
    {
        [SerializeField] private Image m_IconImage;
        [SerializeField] private TextMeshProUGUI m_Text;
        [SerializeField] private Image m_BackgroundImage;
        
        [SerializeField] private Sprite m_LogSprite;
        [SerializeField] private Sprite m_ErrorSprite;
        [SerializeField] private Sprite m_WarningSprite;
        [SerializeField] private Sprite m_CommandSprite;

        [SerializeField] private Color m_ParameterColor;
        [SerializeField] private Color m_BackgroundColor1;
        [SerializeField] private Color m_BackgroundColor2;
        [SerializeField] private Color m_AutocompleteBackgroundColor;
        
        private const string k_DateFormat = "hh:mm:ss";
        
        public void SetText(string message, bool addDate = true)
        {
            if (addDate)
            {
                string time = DateTime.Now.ToString(k_DateFormat);
                m_Text.text = "[" + time + "] ";
            }
            
            m_Text.text += message;
        }
        
        public void SetTextParameter(string[] parameters)
        {
            string colorHex = ColorUtility.ToHtmlStringRGBA(m_ParameterColor);

            for (int i = 0; i < parameters.Length; i++)
            {
                m_Text.text += $"<color=#{colorHex}> {parameters[i]}</color>";
            }
        }

        public void SetIcon(LogMessageTypes type)
        {
            m_IconImage.sprite = type switch
            {
                LogMessageTypes.Log => m_LogSprite,
                LogMessageTypes.Error => m_ErrorSprite,
                LogMessageTypes.Warning => m_WarningSprite,
                LogMessageTypes.Command => m_CommandSprite,
                LogMessageTypes.Autocomplete => null,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public void SetBackgroundColor(ref bool even)
        {
            Color color = even ? m_BackgroundColor2 : m_BackgroundColor1;
            m_BackgroundImage.color = color;
            even = !even;
        }
        
        public void SetAutocompleteBackgroundColor()
        {
            m_BackgroundImage.color = m_AutocompleteBackgroundColor;
        }
    }
}
