using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gali.AppServer.Resources
{
    public class Result
    {
        public Result()
        {
            this.State = ResultsStates.success;
            this.ValidationResults = new ValidatorResults();
        }

        public string State { get; set; }
        public string Message { get; set; }
        public ValidatorResults ValidationResults { get; set; }
        public Exception Exception { get; set; }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }
    }

    public class ResultsStates
    {
        public const string success = "success";
        public const string unsuccess = "unsuccess";
        public const string empty = "empty";
        public const string invalid = "invalid";
        public const string error = "error";
    }
}
