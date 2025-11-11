
using System.Text.Json.Serialization;



namespace ProjectLogging;



public class GenerationSettings
{
    public string BasePath { get; }
    public string PdfStylesPath { get; }
    public string PdfStyle { get; }
    public string AiConfigPath { get; }
    public string WebsiteOutputPath { get; }
    public string ResumeOutputPath { get; }
    public string ResumeName { get; }
    public string DataConfigPath { get; }



    [JsonConstructor]
    public GenerationSettings(
        string basePath,
        string pdfStylesPath,
        string pdfStyle,
        string aiConfigPath,
        string websiteOutputPath,
        string resumeOutputPath,
        string resumeName,
        string dataConfigPath)
    {
        BasePath = Path.GetFullPath(basePath);
        PdfStylesPath = GetFullPath(pdfStylesPath);
        PdfStyle = pdfStyle;
        AiConfigPath = GetFullPath(aiConfigPath);
        WebsiteOutputPath = GetFullPath(websiteOutputPath);
        ResumeOutputPath = GetFullPath(resumeOutputPath);
        ResumeName = resumeName;
        DataConfigPath = GetFullPath(dataConfigPath);
    }



    public string GetFullPath(string path)
    {
        return Path.GetFullPath(path, BasePath);
    }
}
