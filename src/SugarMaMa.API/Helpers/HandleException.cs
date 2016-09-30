using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SugarMaMa.API.Helpers
{
    /// <summary>
    /// All this does is return exception details in the response if the app is in the development environment
    /// </summary>
    public class HandleException : TypeFilterAttribute
    {
        public HandleException() : base(typeof(HandleExceptionImpl))
        { }

        public class HandleExceptionImpl : ExceptionFilterAttribute
        {
            private readonly IHostingEnvironment _hostingEnv;

            public HandleExceptionImpl(IHostingEnvironment hostingEnv)
            {
                _hostingEnv = hostingEnv;
            }

            public override void OnException(ExceptionContext context)
            {
                if (_hostingEnv.IsDevelopment())
                {
                    context.ExceptionHandled = true;
                    context.Result = new ObjectResult(new
                    {
                        message = context.Exception.Message,
                        stackTrace = context.Exception.StackTrace
                    })
                    { StatusCode = 500 };

                    return;
                }
            }
        }
    }
}
