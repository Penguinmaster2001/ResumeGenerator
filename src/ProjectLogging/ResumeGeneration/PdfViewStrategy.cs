
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration;



public abstract class PdfViewStrategy<T> : IPdfViewStrategy
{
    public Type ModelType => typeof(T);



    public abstract Action<IContainer> BuildView(T model, IPdfViewFactory factory);
}
