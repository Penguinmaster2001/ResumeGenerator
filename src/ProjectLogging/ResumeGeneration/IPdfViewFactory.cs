
using QuestPDF.Infrastructure;



namespace ProjectLogging.ResumeGeneration;



public interface IPdfViewFactory
{
    void AddStrategy<T>(PdfViewStrategy<T> strategy);



    Action<IContainer> BuildView<T>(T model);
}
