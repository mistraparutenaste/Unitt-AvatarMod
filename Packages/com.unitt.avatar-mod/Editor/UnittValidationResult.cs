namespace Unitt.AvatarMod.Editor
{
    public enum UnittValidationSeverity
    {
        Info,
        Warning,
        Error
    }

    public sealed class UnittValidationResult
    {
        private UnittValidationResult(UnittValidationSeverity severity, string message)
        {
            Severity = severity;
            Message = message;
        }

        public UnittValidationSeverity Severity { get; }
        public string Message { get; }

        public static UnittValidationResult Info(string message)
        {
            return new UnittValidationResult(UnittValidationSeverity.Info, message);
        }

        public static UnittValidationResult Warning(string message)
        {
            return new UnittValidationResult(UnittValidationSeverity.Warning, message);
        }

        public static UnittValidationResult Error(string message)
        {
            return new UnittValidationResult(UnittValidationSeverity.Error, message);
        }
    }
}
