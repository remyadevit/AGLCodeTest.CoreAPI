using System;
using AGLTest.Common.Response;
using Microsoft.AspNetCore.Mvc;

namespace AGLCodeTest.API
{
    public static class ResponseExtension
    {
        public static ActionResult<TValue> ToActionResult<TValue>(this Response @this) where TValue : class
        {
            switch (@this)
            {
                case null:
                    throw new ArgumentNullException(nameof(@this));
                case ErrResponse errorResponse:
                    return new ObjectResult(errorResponse.Value)
                    {
                        StatusCode = (int)errorResponse.HttpStatusCode
                    };

                case ResponseValue<TValue> valueResponse:
                    return new ObjectResult(valueResponse.Value)
                    {
                        StatusCode = (int)valueResponse.HttpStatusCode
                    };
            }

            return new StatusCodeResult((int)@this.HttpStatusCode);
        }
    }
}
