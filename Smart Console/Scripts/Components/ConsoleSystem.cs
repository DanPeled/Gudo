using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace SmartConsole.Components
{
    public class ConsoleSystem : MonoBehaviour
    {
        [SerializeField] private GameObject m_CanvasGameobject;
        [SerializeField] private TMP_InputField m_Field;
        [SerializeField] private TextMeshProUGUI m_ParamsText;
        
#if ENABLE_INPUT_SYSTEM
        [SerializeField] private InputAction m_OpenCloseAction;
        [SerializeField] private InputAction m_AutocompleteAction;
        [SerializeField] private InputAction m_CopyNextLogMessage;
        [SerializeField] private InputAction m_CopyPreviousLogMessage;
#elif ENABLE_LEGACY_INPUT_MANAGER
        [SerializeField] private KeyCode m_OpenCloseKeyCode;
        [SerializeField] private KeyCode m_AutocompleteKeyCode;
        [SerializeField] private KeyCode m_CopyNextLogMessageKeyCode;
        [SerializeField] private KeyCode m_CopyPreviousLogMessageKeyCode;
#endif
        
        [SerializeField] private bool m_ShowApplicationLogMessage;
        [SerializeField] private bool m_OpenAtStart;
        [SerializeField] private bool m_LockUnlockCursor;
        [SerializeField] private UnityEvent<bool> m_OnOpenCloseEvent;

        public List<LogMessage> AutocompleteLogMessages = new List<LogMessage>();
        
        #region Events
        
        // Submit events  
        public event Action<LogMessage> OnSubmitLogMessage;
        public event Action<LogMessage, string> OnSubmitAutocompleteLogMessage;
        
        // Clear event
        public event Action OnClearAutocomplete;
        
        #endregion
        
        private List<LogMessage> LogMessagesSent = new List<LogMessage>();
        private int m_CurrentAutocompleteIndex;
        private int m_CurrentLogMessageIndexCopied = -1;
        
        private void OnEnable()
        {
            if (m_ShowApplicationLogMessage)
            {
                Application.logMessageReceived += SubmitLog;
            }
            
        #if ENABLE_INPUT_SYSTEM
            m_OpenCloseAction.Enable();
            m_AutocompleteAction.Enable();
            m_CopyNextLogMessage.Enable();
            m_CopyPreviousLogMessage.Enable();
        #endif
            
            if (m_LockUnlockCursor)
            {
                m_OnOpenCloseEvent.AddListener(LockUnlockCursor);
            }
            
            m_Field.onValueChanged.AddListener(SubmitAutocompleteField);
            m_Field.onSubmit.AddListener(SubmitField);
        }
        
        private void OnDisable()
        {
            if (m_ShowApplicationLogMessage)
            {
                Application.logMessageReceived -= SubmitLog;
            }
            
        #if ENABLE_INPUT_SYSTEM
            m_OpenCloseAction.Disable();
            m_AutocompleteAction.Disable();
            m_CopyNextLogMessage.Disable();
            m_CopyPreviousLogMessage.Disable();
        #endif
            
            if (m_LockUnlockCursor)
            {
                m_OnOpenCloseEvent.RemoveListener(LockUnlockCursor);
            }
            
            m_Field.onValueChanged.RemoveListener(SubmitAutocompleteField);
            m_Field.onSubmit.RemoveListener(SubmitField);
        }
        
        private void Start()
        {
        #if ENABLE_INPUT_SYSTEM
            m_OpenCloseAction.performed += ctx => OpenCloseConsole();
            m_AutocompleteAction.performed += ctx => Autocomplete();
            m_CopyNextLogMessage.performed += ctx => CopyLogMessageIndex(-1);
            m_CopyPreviousLogMessage.performed += ctx => CopyLogMessageIndex(1);
        #endif
            
            if (m_OpenAtStart)
            {
                OpenCloseConsole();
            }
            
            Debug.Log("Smart Console has been setup successfully");
        }

#if ENABLE_LEGACY_INPUT_MANAGER && !ENABLE_INPUT_SYSTEM
        private void Update()
        {
            if (Input.GetKeyDown(m_OpenCloseKeyCode))
            {
                OpenCloseConsole();
            }
            
            if (Input.GetKeyDown(m_AutocompleteKeyCode))
            {
                Autocomplete();
            }
            
            if (Input.GetKeyDown(m_CopyNextLogMessageKeyCode))
            {
                CopyLogMessageIndex(-1);
            }
            
            if (Input.GetKeyDown(m_CopyPreviousLogMessageKeyCode))
            {
                CopyLogMessageIndex(1);
            }
        }
#endif
        
        /// <summary>
        /// Find command that fits a specific text
        /// </summary>
        /// <param name="text">specific text</param>
        private Command FindCommands(string text) => 
            Command.List.Find(command => string.Equals(command.MethodInfo.Name.ToLower(), text.ToLower()));
        
        /// <summary>
        /// Find command(s) that start with a specific text
        /// </summary>
        /// <param name="text">start with text</param>
        private List<Command> FindStartWithCommands(string text) => 
            Command.List.FindAll(command => command.MethodInfo.Name.ToLower().StartsWith(text.ToLower()));
        
        /// <summary>
        /// Submit the command(s) that fits a specific text as autocomplete log message
        /// </summary>
        /// <param name="text">start with text</param>
        private void SubmitAutocompleteField(string text)
        {
            m_ParamsText.text = "";
            m_CurrentAutocompleteIndex = 0;
            
            if (string.IsNullOrEmpty(text))
            {
                OnClearAutocomplete?.Invoke();
                return;
            }
            
            string[] textParts = text.Split(' ');
            var commands = FindStartWithCommands(textParts[0]);
            
            if (commands.Count == 0)
            {
                OnClearAutocomplete?.Invoke();
                return;
            }
            
            for (int i = 0; i < commands.Count; i++)
            {
                var parametersInfo = commands[i].MethodInfo.GetParameters();
                var parameters = new string[parametersInfo.Length];
                
                for (int j = 0; j < parametersInfo.Length; j++)
                {
                    parameters[j] = parametersInfo[j].Name;
                }
                
                LogMessage logMessage = new LogMessage(commands[i].MethodInfo.Name, LogMessageTypes.Autocomplete, parameters);
                OnSubmitAutocompleteLogMessage?.Invoke(logMessage, text);
            }
        }
        
        public void SubmitFieldWrapper() => SubmitField(m_Field.text);
        
        /// <summary>
        /// Submit the command that fits the text field
        /// </summary>
        /// <param name="text">text field</param>
        private void SubmitField(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            
            string[] fieldParts = text.Split(' ');
            Command command = FindCommands(fieldParts[0]);
            bool isCommand = command != null && command.MethodInfo != null;
            
            LogMessage logMessage = new LogMessage(text, isCommand ? LogMessageTypes.Command : LogMessageTypes.Error);
            OnSubmitLogMessage?.Invoke(logMessage);
            LogMessagesSent.Add(logMessage);
            
            if (isCommand)
            {
                if (command.MethodInfo.GetParameters().Length > 0)
                {
                    // use command with parameter(s)
                    string[] paramParts = new string[fieldParts.Length - 1];
                    int j = 0;
                
                    for (int i = 1; i < fieldParts.Length; i++)
                    {
                        paramParts[j] = fieldParts[i];
                        j++;
                    }
                    
                    command.Use(paramParts);
                }
                else
                {
                    command.Use();
                }
            }
            
            m_CurrentLogMessageIndexCopied = -1;
            m_ParamsText.text = "";
            m_Field.ActivateInputField();
            m_Field.SetTextWithoutNotify("");
            OnClearAutocomplete?.Invoke();
        }
        
        /// <summary>
        /// Submit the command as application log message
        /// </summary>
        private void SubmitLog(string logString, string stackTrace, LogType type)
        {
            LogMessageTypes messagetype = type switch
            {
                LogType.Log => LogMessageTypes.Log,
                LogType.Error => LogMessageTypes.Error,
                LogType.Assert => LogMessageTypes.Error,
                LogType.Warning => LogMessageTypes.Warning,
                LogType.Exception => LogMessageTypes.Error,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
            
            LogMessage logMessage = new LogMessage(logString, messagetype);
            OnSubmitLogMessage?.Invoke(logMessage);
        }
        
        /// <summary>
        /// Copy the autocomplete log message index text into the field text
        /// </summary>
        private void Autocomplete()
        {
            if (AutocompleteLogMessages.Count == 0)
            {
                return;
            }

            if (AutocompleteLogMessages.Count <= m_CurrentAutocompleteIndex)
            {
                m_CurrentAutocompleteIndex = 0;
            }
            
            m_Field.SetTextWithoutNotify(AutocompleteLogMessages[m_CurrentAutocompleteIndex].Text);
            StartCoroutine(MoveTextToEnd());
            
            AutocompleteParameters();
            m_CurrentAutocompleteIndex++;
        }

        /// <summary>
        /// Copy the autocomplete log message index parameters text into the field text
        /// </summary>
        private void AutocompleteParameters()
        {
            string[] parametersNames = AutocompleteLogMessages[m_CurrentAutocompleteIndex].ParametersNames;

            if (parametersNames.Length > 0)
            {
                string invisibleText = "<color=#FFFFFF00>" + m_Field.text + "</color>";
                string paramText = "";

                for (int i = 0; i < parametersNames.Length; i++)
                {
                    paramText += $" {parametersNames[i]}";
                }

                m_ParamsText.text = invisibleText + paramText;
            }
            else
            {
                m_ParamsText.text = "";
            }
        }

        private void CopyLogMessageIndex(int toAdd)
        {
            if (LogMessagesSent.Count == 0)
            {
                return;
            }
            
            if (LogMessagesSent.Count == 1)
            {
                m_CurrentLogMessageIndexCopied = 0;
            }
            else
            {
                m_CurrentLogMessageIndexCopied += toAdd;

                if (m_CurrentLogMessageIndexCopied < 0)
                {
                    m_CurrentLogMessageIndexCopied = LogMessagesSent.Count - 1;
                }
                else if (m_CurrentLogMessageIndexCopied > LogMessagesSent.Count - 1)
                {
                    m_CurrentLogMessageIndexCopied = 0;
                }
            }
            
            m_Field.SetTextWithoutNotify(LogMessagesSent[m_CurrentLogMessageIndexCopied].Text);
            StartCoroutine(MoveTextToEnd());
            OnClearAutocomplete?.Invoke();
            SubmitAutocompleteField(m_Field.text);
        }
        
        /// <summary>
        /// Move caret to end of text
        /// </summary>
        /// <returns></returns>
        private IEnumerator MoveTextToEnd()
        {
            yield return new WaitForEndOfFrame();
            m_Field.MoveTextEnd(false);
        }
        
        /// <summary>
        /// Open the console if disabled or close it if enabled
        /// </summary>
        public void OpenCloseConsole()
        {
            m_CanvasGameobject.SetActive(!m_CanvasGameobject.activeInHierarchy);
            m_OnOpenCloseEvent.Invoke(m_CanvasGameobject.activeInHierarchy);

            if (m_CanvasGameobject.activeInHierarchy)
            {
                m_Field.Select();
                m_Field.ActivateInputField();
                
                if (!string.IsNullOrEmpty(m_Field.text))
                {
                    m_Field.SetTextWithoutNotify(m_Field.text.Remove(m_Field.text.Length - 1));
                }
            }
        }
        
        /// <summary>
        /// Lock the cursor if unlocked or Unlock it if locked
        /// </summary>
        private void LockUnlockCursor(bool state)
        {
            if (state)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
