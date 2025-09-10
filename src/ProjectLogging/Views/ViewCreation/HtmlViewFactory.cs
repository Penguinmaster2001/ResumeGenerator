
namespace ProjectLogging.Views.ViewCreation;



public class ViewFactory<V> : IViewFactory<V>
{
    private readonly Dictionary<Type, IViewStrategy<V>> _strategies = [];



    public void AddStrategy<T>(ViewStrategy<V, T> strategy)
    {
        _strategies.Add(strategy.ModelType, strategy);
    }



    public V BuildView<T>(T model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        if (!_strategies.TryGetValue(typeof(T), out var strategy) || strategy is not ViewStrategy<V, T> typedStrategy)
        {
            throw new ArgumentException($"Strategy for model of type {model.GetType()} not added.", nameof(model));
        }

        return typedStrategy.BuildView(model, this);
    }
}
