
namespace ProjectLogging.Data;



public interface IDataCollection
{
    DataConfig DataConfig { get; }



    IEnumerable<(string label, T data)> GetDataOfType<T>();



    void AddData<T>(string label, T data);



    T GetData<T>(string label);
}
