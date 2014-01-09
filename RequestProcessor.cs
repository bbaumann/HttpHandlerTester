using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace httpTester
{
    public abstract class BaseRequestProcessor
    {
        public int SleepTime
        {
            get;
            set;
        }

        protected HttpContext _context;

        public BaseRequestProcessor(HttpContext context)
        {
            _context = context;
        }

        public abstract void Render();
    }

    public class RequestProcessorAsync : BaseRequestProcessor
    {
        public RequestProcessorAsync (HttpContext context)
            :base(context)
	    {
	    }

        public override void Render()
        {
            var st = _context.Request.QueryString["SleepTime"];
            if (st != null)
            {
                this.SleepTime = Convert.ToInt32(st);
                _context.Response.Write("SleepTime = "+this.SleepTime+Environment.NewLine);
            }
            else
            {
                _context.Response.Write(_context.Request.QueryString);
            }
            _context.Response.Write("Begin Async Call" + Environment.NewLine);

            //AttachedToParent empêche la requête HTTP de se finir avant la tâche.
            var res = Task.Factory.StartNew(DoLongMethod,TaskCreationOptions.AttachedToParent).ContinueWith((t) =>
                {
                    _context.Response.Write("Return to Main Thread" + Environment.NewLine);
                },TaskContinuationOptions.AttachedToParent
            );
            _context.Response.Write("Main Method ended"+Environment.NewLine);
            
        }

        private void DoLongMethod()
        {
            Thread.Sleep(SleepTime);
            _context.Response.StatusCode = 254;
            _context.Response.Write("Async Call Ended" + Environment.NewLine);
        }
    }

    public class RequestProcessor : BaseRequestProcessor
    {
        public RequestProcessor(HttpContext context)
            :base(context)
        {
        }

        public override void Render()
        {
            var st = _context.Request.QueryString["SleepTime"];
            if (st != null)
            {
                this.SleepTime = Convert.ToInt32(st);
                _context.Response.Write("SleepTime = "+this.SleepTime+Environment.NewLine);
            }
            else
            {
                _context.Response.Write(_context.Request.QueryString);
            }
            _context.Response.Write("Begin Sync Call" + Environment.NewLine);
            DoLongMethod();
            _context.Response.Write("Main Method ended"+Environment.NewLine);
            
        }

        private void DoLongMethod()
        {
            Thread.Sleep(SleepTime);
            _context.Response.StatusCode = 254;
            _context.Response.Write("Long Method Ended" + Environment.NewLine);
        }
    }
}