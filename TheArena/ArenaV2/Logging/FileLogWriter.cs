using System.IO;
using System.Threading.Tasks;
using ArenaV2.Api.Logging;
using Ninject;

namespace ArenaV2.Logging {
    internal class FileLogWriter : AsyncLogWriter {
        private readonly LogLevel _minSeverity;
        private readonly StreamWriter _outputStream;

        public FileLogWriter(LogLevel minSeverity, [Named("Output")] string output) {
            this._minSeverity = minSeverity;
            this._outputStream = new StreamWriter(File.OpenWrite(output));
        }

        protected override bool ShouldLog(LogMessage message) {
            return message.Severity >= this._minSeverity;
        }

        protected override Task WriteAsync(LogMessage message) {
            return this._outputStream.WriteAsync(message.ToString());
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                this._outputStream.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}