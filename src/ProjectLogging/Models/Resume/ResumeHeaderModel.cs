
using ProjectLogging.Data;



namespace ProjectLogging.Models.Resume;



public class ResumeHeaderModel
{
    public string NameText;
    public string PhoneNumberText;
    public string EmailText;
    public string LocationText;
    public List<string> URLs;



    public ResumeHeaderModel(string name, string phoneNumber, string email, string location, List<string> urls)
    {
        NameText = name;
        PhoneNumberText = phoneNumber;
        EmailText = email;
        LocationText = location;
        URLs = urls;
    }



    public ResumeHeaderModel(PersonalInfo personalInfo)
    {
        NameText = personalInfo.Name;

        PhoneNumberText = personalInfo.PhoneNumber;
        EmailText = personalInfo.Email;
        LocationText = personalInfo.Location;

        URLs = personalInfo.URLs;
    }
}
