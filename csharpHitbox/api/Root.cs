using Nancy;

namespace csharpHitbox.api
{
    public class Root : NancyModule
    {
        public Root()
        {
            Get["/"] = _ => "This is the root page";
        }
    }
}