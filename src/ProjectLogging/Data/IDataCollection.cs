
namespace ProjectLogging.Data;



public interface IDataCollection
{
    IEnumerable<(string label, T data)> GetDataOfType<T>();



    void AddData<T>(string label, T data);



    T GetData<T>(string label);
}
