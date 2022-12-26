namespace SmartConsole
{
    /// <summary>
    ///   <para>The type of the console log message in Smart_Console</para>
    /// </summary>
    public enum LogMessageTypes
    {
        /// <summary>
        ///   <para>LogMessageType used for Errors.</para>
        /// </summary>
        Error,
        /// <summary>
        ///   <para>LogMessageType used for Warnings.</para>
        /// </summary>
        Warning,
        /// <summary>
        ///   <para>LogMessageType used for regular log messages.</para>
        /// </summary>
        Log,
        /// <summary>
        ///   <para>LogMessageType used for Commands.</para>
        /// </summary>
        Command,
        /// <summary>
        ///   <para>LogMessageType used to Autocomplete commands.</para>
        /// </summary>
        Autocomplete
    }
}
