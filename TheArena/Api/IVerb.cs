using NDesk.Options;

namespace ArenaV2.Api {
    public interface IVerb {
        /// <summary>The description of this verb.</summary>
        string Description { get; }

        /// <summary>The options that should be accepted.</summary>
        OptionSet Options { get; }
    }
}