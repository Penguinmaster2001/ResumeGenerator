
using System.Text.Json.Serialization;



namespace ProjectLogging.Records;



[JsonSerializable(typeof(PersonalInfo))]
public record PersonalInfo(string Name, string PhoneNumber, string Email, string Location, List<string> URLs);
