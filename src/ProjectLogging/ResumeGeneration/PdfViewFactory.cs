
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration;



public class PdfViewFactory() : IPdfViewFactory
{
    private readonly Dictionary<Type, IPdfViewStrategy> _strategies = [];



    public void AddStrategy<T>(PdfViewStrategy<T> strategy)
    {
        _strategies.Add(strategy.ModelType, strategy);
    }



    public Action<IContainer> BuildView<T>(T model)
    {
        ArgumentNullException.ThrowIfNull(model, nameof(model));

        if (!_strategies.TryGetValue(typeof(T), out var strategy) || strategy is not PdfViewStrategy<T> typedStrategy)
        {
            throw new ArgumentException($"Strategy for model of type {model.GetType()} not added.", nameof(model));
        }

        return typedStrategy.BuildView(model, this);
    }
}
