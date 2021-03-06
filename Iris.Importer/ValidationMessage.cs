﻿namespace Iris.Importer
{
    public class ValidationMessage
    {
        public ValidationType Type { get; set; }
        public string Message { get; set; }

        public ValidationMessage()
        {
            Type = ValidationType.Success;
            Message = string.Empty;
        }
        public ValidationMessage(ValidationType type, string message)
        {
            Type = type;
            Message = message;
        }
    }

    public enum ValidationType
    {
        Success,
        Warning,
        Error
    }
}
