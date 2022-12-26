using System.Collections.Generic;
using UnityEngine;

namespace SmartConsole.Components
{
    [RequireComponent(typeof(ConsoleSystem))]
    public class ConsoleUIGenerator : MonoBehaviour
    {
        [SerializeField] private Transform m_GridContent;
        [SerializeField] private GameObject m_LogMessageTextPrefab;
        
        private ConsoleSystem m_ConsoleSystem;
        private List<GameObject> m_AutocompleteInstances = new List<GameObject>();
        private string m_AutocompleteCommandRef;
        private bool m_IsLastMessageEven;
        
        private void OnEnable()
        {
            m_ConsoleSystem.OnSubmitLogMessage += GenerateLog;
            m_ConsoleSystem.OnSubmitAutocompleteLogMessage += GenerateAutocompleteLog;
            m_ConsoleSystem.OnClearAutocomplete += ClearAutocompletes;
        }
        
        private void OnDisable()
        {
            m_ConsoleSystem.OnSubmitLogMessage -= GenerateLog;
            m_ConsoleSystem.OnSubmitAutocompleteLogMessage -= GenerateAutocompleteLog;
            m_ConsoleSystem.OnClearAutocomplete -= ClearAutocompletes;
        }

        private void Awake()
        {
            m_ConsoleSystem = gameObject.GetComponent<ConsoleSystem>();
        }

        private void GenerateLog(LogMessage logMessage)
        {
            GameObject logInstance = Instantiate(m_LogMessageTextPrefab, m_GridContent);
            
            if (logInstance.TryGetComponent(out LogMessageSetup logInstanceSetup))
            {
                logInstanceSetup.SetText(logMessage.Text);
                logInstanceSetup.SetIcon(logMessage.Type);
                logInstanceSetup.SetBackgroundColor(ref m_IsLastMessageEven);
            }
        }

        private void GenerateAutocompleteLog(LogMessage logMessage, string text)
        {
            GameObject logInstance = Instantiate(m_LogMessageTextPrefab, m_GridContent);

            if (string.IsNullOrEmpty(m_AutocompleteCommandRef))
            {
                m_AutocompleteCommandRef = text;
            }
            else if (m_AutocompleteCommandRef != text)
            {
                ClearAutocompletes();
                m_AutocompleteCommandRef = text;
            }
                
            m_AutocompleteInstances.Add(logInstance);
            m_ConsoleSystem.AutocompleteLogMessages.Add(logMessage);

            if (logInstance.TryGetComponent(out LogMessageSetup logInstanceSetup))
            {
                logInstanceSetup.SetText(logMessage.Text, false);
                logInstanceSetup.SetIcon(LogMessageTypes.Autocomplete);
                logInstanceSetup.SetAutocompleteBackgroundColor();
                
                if (logMessage.ParametersNames != null)
                {
                    logInstanceSetup.SetTextParameter(logMessage.ParametersNames);
                }
            }
        }

        private void ClearAutocompletes()
        {
            for (int i = 0; i < m_AutocompleteInstances.Count; i++)
            {
                Destroy(m_AutocompleteInstances[i]);
            }
            
            m_AutocompleteInstances.Clear();
            m_ConsoleSystem.AutocompleteLogMessages.Clear();
        }
        
        public void Clear()
        {
            foreach (Transform child in m_GridContent)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
