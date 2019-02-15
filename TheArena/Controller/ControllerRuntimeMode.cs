using System;
using System.Threading.Tasks;
using ArenaV2.Api;
using ArenaV2.Api.Logging;
using NDesk.Options;

namespace ArenaV2.Controller {
    public class ControllerRuntimeMode : IRuntimeMode {
        private readonly ILogger _logger;
        private ushort port = 12276;

        public string Description { get; }
        public OptionSet Options { get; }

        public ControllerRuntimeMode(ILogger logger) {
            this._logger = logger;

            this.Description = "The arena controller, which manages every arena runner.";

            this.Options = new OptionSet();
            this.Options.Add<ushort>("p|port=", $"The incoming port, which the controller will listen for connections to. Default port is {this.port}.", v => this.port = v);
        }

        public Task Execute(string[] remainingArgs) {
            throw new NotImplementedException();
        }
    }
}
