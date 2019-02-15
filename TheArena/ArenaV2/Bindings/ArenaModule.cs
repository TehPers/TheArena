using ArenaV2.Api.Bindings;
using ArenaV2.Api.Extensions;
using ArenaV2.Controller.Bindings;
using Ninject.Modules;
using Runner.Bindings;

namespace ArenaV2.Bindings {
    internal class ArenaModule : ConfiguredModule {
        protected override void LoadShared() {
            this.Kernel?.Load(new INinjectModule[] {
                new LoggingModule(),
                new ControllerModule(),
                new RunnerModule()
            });

            this.BindRuntimeMode<HelpRuntimeMode>("help");
        }

        protected override void LoadDebug() {

        }

        protected override void LoadRelease() {

        }
    }
}
