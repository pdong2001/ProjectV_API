using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Utils.Helpers.TypeScriptHepler;

namespace Services.Contracts
{
    public class ServiceResponse<TData> : ServiceResponse
    {

        public TData? Data { get; set; }

        public void SetValue(TData data)
        {
            Success = data != null;
            Data = data;
        }

        public void SetFailed(string? message = null)
        {
            Success = false;
            Message = message;
        }
    }

    [NoTScript]
    public class ServiceResponse
    {
        public string? Message { get; set; }
        public bool Success { get; set; } = false;

        public static ServiceResponse CreateSuccess(string? message = null)
        {
            return new ServiceResponse { Message = message, Success = true };
        }
        public static ServiceResponse CreateFailed(string? message = null)
        {
            return new ServiceResponse { Message = message, Success = false };
        }
    }
}
