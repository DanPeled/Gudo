using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SmartConsole.Components
{
    public class LogMessageSelection : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image m_BackgroundImage;
        [SerializeField] private Color m_BackgroundColorSelected;
        
        #region Events
        
        // Select event
        public static event Action<int> OnSelectLogMessage;
        
        #endregion
        
        private Color m_BackgroundColor;
        private int m_Hash;
        
        private void Start()
        {
            m_BackgroundColor = m_BackgroundImage.color;
            m_Hash = gameObject.GetHashCode();
        }
        
        private void OnEnable() => OnSelectLogMessage += OnLogMessageSelected;

        private void OnDisable() => OnSelectLogMessage -= OnLogMessageSelected;
        
        public void OnPointerClick(PointerEventData eventData) => OnSelectLogMessage?.Invoke(m_Hash);

        private void OnLogMessageSelected(int hash)
        {
            m_BackgroundImage.color = m_Hash == hash ? m_BackgroundColorSelected : m_BackgroundColor;
        }
    }
}
