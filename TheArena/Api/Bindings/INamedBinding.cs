namespace ArenaV2.Api.Bindings {
    public interface INamedBinding<out T> {
        string Name { get; }
        T Value { get; }
    }
}