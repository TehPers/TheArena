using ArenaV2.Api.Bindings;
using ArenaV2.Api.Extensions;

namespace ArenaV2.Controller.Bindings {
    public class ControllerModule : ConfiguredModule {
        protected override void LoadShared() {
            this.BindRuntimeMode<ControllerRuntimeMode>("controller");
        }

        protected override void LoadDebug() {
            
        }

        protected override void LoadRelease() {
            
        }
    }
}