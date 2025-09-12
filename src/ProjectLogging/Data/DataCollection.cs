

namespace ProjectLogging.Data;



public class DataCollection : IDataCollection
{
    private readonly Dictionary<(string label, Type type), object> _data = [];



    public IEnumerable<(string label, T data)> GetDataOfType<T>()
        => _data.Where(d => d.Key.type.IsAssignableTo(typeof(T))).Select(d => (d.Key.label, (T)d.Value));



    public void AddData<T>(string label, T data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));

        _data[(label, typeof(T))] = data;
    }



    public T GetData<T>(string label)
    {
        if (!_data.TryGetValue((label, typeof(T)), out var data))
        {
            throw new Exception($"Data of type {typeof(T)} and label {label} not found in collection.");
        }

        return (T)data;
    }
}
