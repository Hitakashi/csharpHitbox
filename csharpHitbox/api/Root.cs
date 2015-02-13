using System;
using System.Net.Mime;
using System.Text;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace csharpHitbox.api
{
    public class Root : NancyModule
    {
        public Root()
        {
            Get["/"] = _ => "This is the root page";
        }
    }

    // Also putting the Nancy Bootstraper here since this is the root api

    public class CustomBoot : DefaultNancyBootstrapper
    {
        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            pipelines.OnError.AddItemToEndOfPipeline((z, a) =>
            {
                var json = Encoding.UTF8.GetBytes("{\"status\":\"error\"}");
                return new Response
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ContentType = "application/json",
                    Contents = s => s.Write(json, 0, json.Length)
                };
            });
            base.RequestStartup(container, pipelines, context);
        }

    }


}