namespace SmartConsole
{
    public class LogMessage
    {
        public string Text;
        public LogMessageTypes Type;
        public string[] ParametersNames;
        
        public LogMessage(string text, LogMessageTypes type, string[] parametersNames = null)
        {
            Text = text;
            Type = type;
            ParametersNames = parametersNames;
        }
    }
}
