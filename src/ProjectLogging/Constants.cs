
namespace ProjectLogging;



public static class Constants
{
    public static class Resources
    {
        public const string Pdf = "pdf";
        public const string Html = "html";
        public const string Css = "css";
        public const string Js = "js";



        public static string Directory(string resourceType) => resourceType switch 
            {
                Pdf     => ".",
                Html    => ".",
                Css     => "styles",
                Js      => "scripts",
                _       => "resources",
            };
    }
}
