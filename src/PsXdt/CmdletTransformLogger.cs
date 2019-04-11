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

        public string IndentString => string.Concat(Enumerable.Repeat(IndentStringPiece, _indentLevel));


        #region Implementation of IXmlTransformationLogger

        public void LogMessage(string message, params object[] messageArgs)
        {
            LogMessage(MessageType.Normal, message, messageArgs);
        }

        public void LogMessage(MessageType type, string message, params object[] messageArgs)
        {
            bool writeVerbose;
            switch (type)
            {
                case MessageType.Normal:
                    importance = MessageImportance.Normal;
                    break;
                case MessageType.Verbose:
                    importance = MessageImportance.Low;
                    break;
                default:
                    Debug.Fail("Unknown MessageType");
                    importance = MessageImportance.Normal;
                    break;
            }

            if (_useSections)
            {
                message = string.Concat(IndentString, message);
            }
            _cmdlet.WriteVerbose(message);
            _loggingHelper.LogMessage(importance, message, messageArgs);
        }

        public void LogWarning(string message, params object[] messageArgs)
        {
            throw new NotImplementedException();
        }

        public void LogWarning(string file, string message, params object[] messageArgs)
        {
            throw new NotImplementedException();
        }

        public void LogWarning(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
        {
            throw new NotImplementedException();
        }

        public void LogError(string message, params object[] messageArgs)
        {
            throw new NotImplementedException();
        }

        public void LogError(string file, string message, params object[] messageArgs)
        {
            throw new NotImplementedException();
        }

        public void LogError(string file, int lineNumber, int linePosition, string message, params object[] messageArgs)
        {
            throw new NotImplementedException();
        }

        public void LogErrorFromException(Exception ex)
        {
            throw new NotImplementedException();
        }

        public void LogErrorFromException(Exception ex, string file)
        {
            throw new NotImplementedException();
        }

        public void LogErrorFromException(Exception ex, string file, int lineNumber, int linePosition)
        {
            throw new NotImplementedException();
        }

        public void StartSection(string message, params object[] messageArgs)
        {
            throw new NotImplementedException();
        }

        public void StartSection(MessageType type, string message, params object[] messageArgs)
        {
            throw new NotImplementedException();
        }

        public void EndSection(string message, params object[] messageArgs)
        {
            throw new NotImplementedException();
        }

        public void EndSection(MessageType type, string message, params object[] messageArgs)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
