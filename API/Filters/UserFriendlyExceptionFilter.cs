using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using Utils.Exceptions;

namespace API.Filters
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        private readonly DataContext _context;

        public HttpResponseExceptionFilter(DataContext context)
        {
            _context = context;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                var exception = context.Exception;
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
                var request = context.HttpContext.Request;
                if (request.Body.CanRead)
                {
                    if (request.Body.CanSeek)
                        request.Body.Position = 0;
                    using (StreamReader sr = new StreamReader(request.Body))
                    {
                        _context.Logs.SingleInsert(new Data.Models.Log
                        {
                            Url = request.Path.Value,
                            Params = request.QueryString.Value,
                            CreatedAt = DateTime.Now,
                            Exception = exception.Message,
                            Payload = sr.ReadToEndAsync().Result,
                            Trace = exception.StackTrace,
                            User = context.HttpContext.User.Identity?.Name,
                            Method = request.Method,
                            Id = Guid.NewGuid()
                        });
                    }
                }
                context.ExceptionHandled = true;
                if (exception is UserFriendlyException userFriendly)
                {
                    context.Result = new BadRequestObjectResult(userFriendly.Message);
                }
                else
                {
                    context.Result = new ObjectResult(null)
                    {
                        StatusCode = 500
                    };
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
