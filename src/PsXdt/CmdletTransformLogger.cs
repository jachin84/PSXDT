using System;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using Microsoft.Web.XmlTransform;

namespace PsXdt
{
    internal class CmdletTransformLogger : IXmlTransformationLogger
    {
        private readonly PSCmdlet _cmdlet;
        private int _indentLevel;
        private const string IndentStringPiece = "  ";

        public CmdletTransformLogger(PSCmdlet cmdlet)
        {
            _cmdlet = cmdlet ?? throw new ArgumentNullException(nameof(cmdlet));
        }

        public string IndentString =>
            _indentLevel == 0 ? string.Empty : string.Concat(Enumerable.Repeat(IndentStringPiece, _indentLevel));


        #region Implementation of IXmlTransformationLogger

        public void LogMessage(string message, params object[] messageArgs)
        {
            LogMessage(MessageType.Normal, message, messageArgs);
        }

        public void LogMessage(MessageType type, string message, params object[] messageArgs)
        {
            message = string.Concat(IndentString, message);
            _cmdlet.WriteVerbose(string.Format(message, messageArgs));
        }

        public void LogWarning(string message, params object[] messageArgs)
        {
            _cmdlet.WriteWarning(string.Format(message, messageArgs));
        }

        public void LogWarning(string file, string message, params object[] messageArgs)
        {
            message = $"{string.Format(message, messageArgs)}. File: {file}.";
            _cmdlet.WriteWarning(message);
        }

        public void LogWarning(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
        {
            message = $"{string.Format(message, messageArgs)}. File: {file}. Line Number: {lineNumber}. Line Position: {linePosition}";
            _cmdlet.WriteWarning(message);
        }

        public void LogError(string message, params object[] messageArgs)
        {
            var exeception = new InvalidOperationException(string.Format(message, messageArgs));
            _cmdlet.WriteError(
                new ErrorRecord(exeception, null,ErrorCategory.InvalidOperation, null));
        }

        public void LogError(string file, string message, params object[] messageArgs)
        {
            message = $"{string.Format(message, messageArgs)}. File: {file}.";
            var exception = new InvalidOperationException(string.Format(message, messageArgs));
            _cmdlet.WriteError(
                new ErrorRecord(exception, null, ErrorCategory.InvalidOperation, null));
        }

        public void LogError(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
        {
            message = $"{string.Format(message, messageArgs)}. File: {file}. Line Number: {lineNumber}. Line Position: {linePosition}";
            var exception = new InvalidOperationException(string.Format(message, messageArgs));
            _cmdlet.WriteError(
                new ErrorRecord(exception, null, ErrorCategory.InvalidOperation, null));
        }

        public void LogErrorFromException(Exception ex)
        {
            _cmdlet.WriteError(new ErrorRecord(ex, null, ErrorCategory.InvalidOperation, null));
        }

        public void LogErrorFromException(Exception ex, string file)
        {
            ex.Data.Add("File", file);
            _cmdlet.WriteError(new ErrorRecord(ex, null, ErrorCategory.InvalidOperation, null));
        }

        public void LogErrorFromException(Exception ex, string file, int lineNumber, int linePosition)
        {
            ex.Data.Add("File", file);
            ex.Data.Add("LineNumber", lineNumber);
            ex.Data.Add("LinePosition",linePosition);
            _cmdlet.WriteError(new ErrorRecord(ex, null, ErrorCategory.InvalidOperation, null));
        }

        public void StartSection(string message, params object[] messageArgs)
        {
            throw new NotImplementedException();
        }

        public void StartSection(MessageType type, string message, params object[] messageArgs)
        {
            LogMessage(type, message, messageArgs);
            _indentLevel++;
        }

        public void EndSection(string message, params object[] messageArgs)
        {
            EndSection(MessageType.Normal, message, messageArgs);
        }

        public void EndSection(MessageType type, string message, params object[] messageArgs)
        {
            Debug.Assert(_indentLevel > 0, "There must be at least one section started");
            if (_indentLevel > 0)
            {
                _indentLevel--;
            }
            LogMessage(type, message, messageArgs);
        }

        #endregion
    }
}
