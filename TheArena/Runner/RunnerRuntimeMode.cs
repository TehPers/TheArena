using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ArenaV2.Api;
using ArenaV2.Api.Logging;
using NDesk.Options;

namespace Runner {
    public class RunnerRuntimeMode : IRuntimeMode {
        private readonly ILogger _logger;
        private ushort _port = 12276;
        private string _hostname;

        public string Description { get; }
        public OptionSet Options { get; }

        public RunnerRuntimeMode(ILogger logger) {
            this._logger = logger;

            this.Description = "The arena runner, which executes matches for the controller.";

            this.Options = new OptionSet();
            this.Options.Add<ushort>("p|port=", $"The port to interact with the controller on. Default port is {this._port}", v => this._port = v);
            this.Options.Add<string>("h|hostname=", $"The hostname of the controller.", v => this._hostname = v);
        }

        public Task Execute(string[] remainingArgs) {
            throw new NotImplementedException();
        }
    }
}
