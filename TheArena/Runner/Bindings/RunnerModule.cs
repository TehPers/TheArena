using ArenaV2.Api.Bindings;
using ArenaV2.Api.Extensions;

namespace Runner.Bindings {
    public class RunnerModule : ConfiguredModule {
        protected override void LoadShared() {
            this.BindRuntimeMode<RunnerRuntimeMode>("runner");
        }

        protected override void LoadDebug() {

        }

        protected override void LoadRelease() {

        }
    }
}
