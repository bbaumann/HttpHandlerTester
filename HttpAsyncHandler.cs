using System;
using System.Threading.Tasks;
using System.Web;

//Originally from Com.Hertkorn.TaskBasedIHttpAsyncHandler.Framework
namespace httpTester
{
    /// <summary>
    /// Root class for HttpAsyncHandler using TPL. Based upon http://www.fsmpi.uni-bayreuth.de/~dun3/archives/task-based-ihttpasynchandler/532.html
    /// </summary>
    public abstract class HttpAsyncHandler : IHttpAsyncHandler
    {
        // In very high performance situations you might want to change the argument to HttpContext
        // and save one object allocation per request.
        public abstract void ProcessRequestAsync(HttpContext context);

        public abstract bool IsReusable { get; }

        IAsyncResult IHttpAsyncHandler.BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            if (cb == null)
            {
                throw new InvalidOperationException("cb is null");
            }

            var task = CreateTask(context,cb);


            if (task.Status == TaskStatus.Created)
            {
                // Got a cold task -> start it
                task.Start();
            }

            return task;
        }

        void IHttpAsyncHandler.EndProcessRequest(IAsyncResult result)
        {
            var task = result as Task;

            // Await task --> Will throw pending exceptions 
            // (to avoid an exception being thrown on the finalizer thread)
            task.Wait();

            task.Dispose();
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            var task = CreateTask(context,null);

            if (task.Status == TaskStatus.Created)
            {
                task.RunSynchronously();
            }
            else
            {
                task.Wait();
            }
        }

        public Task CreateTask(HttpContext context,AsyncCallback cb)
        {
            Task task = new Task(() => ProcessRequestAsync(context));
            
            if (task == null)
            {
                throw new InvalidOperationException("ProcessRequestAsync must return void");
            }
            if (cb != null)
            {
                task.ContinueWith((t) => cb(t));
            }
            
            return task;
        }
    }
}
