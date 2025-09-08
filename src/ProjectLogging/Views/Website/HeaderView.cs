
using ProjectLogging.Models.Website;



namespace ProjectLogging.Views.Website;


public class HeaderView
{
    public HeaderModel Model { get; set; }




    public HeaderView(HeaderModel model)
    {
        Model = model;
    }
}
