using System.Threading.Tasks;

namespace ArenaV2.Api {
    public interface IRuntimeMode : IVerb {
        /// <summary>Executes this runtime mode.</summary>
        /// <param name="remainingArgs">The remaining runtime arguments.</param>
        Task Execute(string[] remainingArgs);
    }
}
