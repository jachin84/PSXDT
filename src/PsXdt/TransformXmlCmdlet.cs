using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Net;
using System.Reflection;
using System.Text;

namespace PsXdt
{
    [Cmdlet("Transform", "XmlConfig")]
    public class TransformXmlCmdlet : PSCmdlet
    {
        private XmlTransformer _transformer;

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [Alias("PSPath")]
        [ValidateNotNullOrEmpty]
        public string ConfigFile { get; set; }

        [Parameter(Position = 2, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string TransformFile { get; set; }

        [Parameter(Position = 3, Mandatory = true)]
        public string OutputPath { get; set; }

        private void ValidateCmdlet()
        {
            if (!File.Exists(ConfigFile))
                throw new FileNotFoundException(ConfigFile);

            if (!File.Exists(TransformFile))
                throw new FileNotFoundException(ConfigFile);
        }

        private void InitializeTransformer()
        {
            try
            {
                _transformer = new XmlTransformer(new CmdletTransformLogger(this));
            }
            catch (Exception exception)
            {
                WriteExceptionError(exception);
            }
        }

        private void PerformTransformation()
        {
            WriteVerbose($"Attempting to transform {ConfigFile} with {TransformFile}.");

            var success = _transformer.Transform(ConfigFile, TransformFile, OutputPath);

            if (!success)
                throw new InvalidOperationException("Transformation failed!");

            WriteVerbose("Transform complete.");
        }

        private void WriteExceptionError(Exception ex) => 
            WriteError(new ErrorRecord(ex, string.Empty, ErrorCategory.CloseError, null));

        #region Overrides of Cmdlet

        protected override void BeginProcessing()
        {
            WriteDebug($"{DateTime.Now:T} - {this.GetType().Name} begin processing.");
            ValidateCmdlet();
            InitializeTransformer();
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            WriteDebug($"{DateTime.Now:T} - {this.GetType().Name} begin processing record.");
            try
            {
                base.ProcessRecord();
                PerformTransformation();
                WriteInformation("Transformation complete.", null);
            }
            catch (Exception ex)
            {
                WriteExceptionError(ex);
            }
        }

        protected override void StopProcessing()
        {
            WriteDebug($"{DateTime.Now:T} - {this.GetType().Name} stop processing.");
            base.StopProcessing();
        }

        protected override void EndProcessing()
        {
            WriteDebug($"{DateTime.Now:T} - {this.GetType().Name} end processing.");
            base.EndProcessing();
        }

        #endregion

    }
}
