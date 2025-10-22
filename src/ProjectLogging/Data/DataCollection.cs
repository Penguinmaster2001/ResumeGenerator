
using System.Text;



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
            // DESIGN ISSUE: Throwing generic Exception instead of a more specific exception type.
            // Consider creating a custom exception (e.g., DataNotFoundException) or using built-in
            // exceptions like KeyNotFoundException or InvalidOperationException for better error
            // handling and clearer intent. Generic exceptions make it harder for calling code to
            // handle specific error scenarios appropriately.
            throw new Exception($"Data of type {typeof(T)} and label {label} not found in collection.");
        }

        return (T)data;
    }




    public override string ToString()
    {
        var sb = new StringBuilder();

        foreach (var key in _data.Keys)
        {
            sb.Append($"{key.label}:");
            sb.Append(_data[key]);
        }

        return sb.ToString();
    }
}
