
namespace ProjectLogging.Views.ViewCreation;



public class ViewFactory<V> : IViewFactory<V>
{
    private readonly Dictionary<Type, IViewStrategy<V>> _strategies = [];
    private readonly Dictionary<Type, object> _helpers = [];
    private readonly List<Action<V, IViewFactory<V>>> _postActions = [];



    public void AddHelper<T, U>() where U : T, new()
    {
        _helpers[typeof(T)] = new U();
    }



    public void AddHelper<T, U>(U helper) where U : T
    {
        ArgumentNullException.ThrowIfNull(helper, nameof(helper));

        _helpers[typeof(T)] = helper;
    }



    public T GetHelper<T>()
    {
        if (!_helpers.TryGetValue(typeof(T), out var helper))
        {
            throw new ArgumentException($"No helper of type {typeof(T)} found");
        }

        return (T)helper;
    }



    public void AddPostAction(Action<V, IViewFactory<V>> action) => _postActions.Add(action);



    public void AddStrategy<T>(ViewStrategy<V, T> strategy)
    {
        _strategies.Add(strategy.ModelType, strategy);
    }



    public void AddStrategy<S>() where S : IViewStrategy<V>, new()
    {
        var strategy = new S();
        _strategies.Add(strategy.ModelType, strategy);
    }



    public V CreateView<T>(T model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        // DESIGN ISSUE: Unsafe type casting without proper validation. If the strategy exists but is
        // not of type ViewStrategy<V, T>, the cast to 'typedStrategy' will succeed (null check passes)
        // but the strategy won't actually be usable. The pattern match should be combined with a null
        // check, or use "is ViewStrategy<V, T> typedStrategy" in the condition for safer type checking.
        // Additionally, consider whether TryGetValue and pattern matching can be refactored into a
        // single, clearer operation to avoid this split validation logic.
        if (!_strategies.TryGetValue(model.GetType(), out var strategy)
            || strategy is not ViewStrategy<V, T> typedStrategy)
        {
            throw new ArgumentException($"Valid strategy for model of type {model.GetType()} not added.",
                nameof(model));
        }
        
        var view = typedStrategy.BuildView(model, this);

        foreach (var action in _postActions)
        {
            action.Invoke(view, this);
        }

        return view;
    }
}
