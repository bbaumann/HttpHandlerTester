using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace httpTester
{
    public class HandlerTestAsync : HttpAsyncHandler
    {
        public override bool IsReusable
        {
            get { return false; }
        }

        public override void ProcessRequestAsync(System.Web.HttpContext context)
        {
            RequestProcessorAsync zone = new RequestProcessorAsync(context);
            zone.Render();
        }
    }

    public class HandlerTest : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {

            RequestProcessor zone = new RequestProcessor(context);
            zone.Render();
        }
    }

    public class HandlerTestNotSoAsync : HttpAsyncHandler
    {
        public override bool IsReusable
        {
            get { return false; }
        }

        public override void ProcessRequestAsync(System.Web.HttpContext context)
        {
            RequestProcessor zone = new RequestProcessor(context);
            zone.Render();
        }
    }
}