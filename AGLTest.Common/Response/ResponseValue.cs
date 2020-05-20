using System.Net;
using FluentResults;

namespace AGLTest.Common.Response
{
    public class ResponseValue<TValue> : Response where TValue : class
    {
        public ResponseValue(HttpStatusCode httpStatusCode, TValue value) : base(httpStatusCode)
        {
            Value = value;
        }

        public TValue Value { get; }
        public bool IsFailed => Value is Error;
        public bool IsSuccess => !IsFailed;
        public Error Error => IsFailed ? Value as Error : null;
    }
}
