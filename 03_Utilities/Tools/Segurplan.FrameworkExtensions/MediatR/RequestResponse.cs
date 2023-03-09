using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Segurplan.FrameworkExtensions.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Segurplan.FrameworkExtensions.MediatR {
    public class RequestResponse : IRequestResponse {
        public static IRequestResponse Aggregate(IEnumerable<IRequestResponse> requestResponses) => requestResponses.All(r => r.Status == RequestStatus.Ok) ? RequestResponse.Ok() : RequestResponse.Error();
        public static IRequestResponse<T> Aggregate<T>(IEnumerable<IRequestResponse<T>> requestResponses) => requestResponses.All(r => r.Status == RequestStatus.Ok) ? RequestResponse.Ok<T>() : RequestResponse.Error<T>();
        public static IRequestResponse Error() => new RequestResponse(RequestStatus.Error);
        public static IRequestResponse Error(Exception ex) => new RequestResponse(ex);

        public static IRequestResponse<T> Error<T>() => new RequestResponse<T>(RequestStatus.Error, default);
        public static IRequestResponse<T> Error<T>(Exception ex) => new RequestResponse<T>(ex);

        public static IRequestResponse<T> Ok<T>(T value = default, ValidationResult validationResult = default) => new RequestResponse<T>(RequestStatus.Ok, value, validationResult);

        public static IRequestResponse Ok(ValidationResult validationResult = default) => new RequestResponse(RequestStatus.Ok, validationResult);

        public static IRequestResponse<T> NotOk<T>(T value, ValidationResult validationResult = default) => new RequestResponse<T>(RequestStatus.NotOk, value, validationResult);

        public static IRequestResponse<T> NotOk<T>(ValidationResult validationResult = default) => new RequestResponse<T>(RequestStatus.NotOk, default, validationResult);

        public static IRequestResponse NotOk(ValidationResult validationResult = default) => new RequestResponse(RequestStatus.NotOk, validationResult);

        public static IRequestResponse<T> NotFound<T>(T value = default) => new RequestResponse<T>(RequestStatus.NoContent, value);

        public static IRequestResponse<T> NotFound<T>() => new RequestResponse<T>(RequestStatus.NoContent, default);

        public static IRequestResponse NotFound() => new RequestResponse(RequestStatus.NoContent);

        public static IRequestResponse<T> Unauthorized<T>() => new RequestResponse<T>(RequestStatus.Unauthorized, default);

        public static IRequestResponse Unauthorized() => new RequestResponse(RequestStatus.Unauthorized);

        public static TResponse OfType<TResponse>(DelegateFactory delegateFactory, string nameToken, params object[] parameters) {
            var response = OfType(delegateFactory, typeof(TResponse), nameToken, parameters);

            return (TResponse)response;
        }

        public static IRequestResponse OfType(DelegateFactory delegateFactory, Type responseType, string nameToken, params object[] parameters) {
            var parameterTypes = parameters.Select(p => p?.GetType() ?? typeof(object)).ToArray();
            var delegateParameters = parameterTypes.Concat(new[] { responseType }).ToArray();

            var responseMethod = delegateFactory.CreateDelegate(Expression.GetFuncType(delegateParameters), new DelegateFactory.DelegateDescription() {
                GenericParameters = responseType.GetGenericArguments()?.ToArray(),
                MethodName = nameToken,
                TargetType = typeof(RequestResponse),
                ParameterTypes = parameterTypes
            });

            return (IRequestResponse)responseMethod.DynamicInvoke(parameters);
        }

        public virtual IActionResult Convert() {
            switch (Status) {
                case RequestStatus.Ok:
                    return new OkResult();
                case RequestStatus.NotOk:
                    return new BadRequestResult();
                case RequestStatus.NoContent:
                    return new NoContentResult();
                case RequestStatus.Unauthorized:
                    return new UnauthorizedResult();
                case RequestStatus.Error:
                default: {
                        var result = new ObjectResult(Exception) {
                            StatusCode = StatusCodes.Status500InternalServerError
                        };

                        return result;
                    }
            }
        }

        protected RequestResponse(RequestStatus status, ValidationResult validationResult = default) {
            Status = status;
            ValidationResult = validationResult;
        }

        protected RequestResponse(string message) {
            Status = RequestStatus.Error;
            Message = message;
        }

        protected RequestResponse(Exception ex) {
            Status = RequestStatus.Error;
            Message = ex?.Message;
            Exception = ex;
        }

        public RequestStatus Status { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public ValidationResult ValidationResult { get; set; }
    }

    public class RequestResponse<T> : RequestResponse, IRequestResponse<T> {
        protected internal RequestResponse(RequestStatus status, T value, ValidationResult validationResult = default)
            : base(status, validationResult) {
            Value = value;
        }

        protected internal RequestResponse(Exception ex)
            : base(ex) {
        }

        protected internal RequestResponse(string message)
            : base(message) {
        }

        public override IActionResult Convert() {
            switch (Status) {
                case RequestStatus.Ok:
                    return new OkObjectResult(Value);
                case RequestStatus.NotOk:
                    return new BadRequestObjectResult(Value);
                default:
                    return base.Convert();
            }
        }

        public IRequestResponse<T1> As<T1>() {
            return new RequestResponse<T1>(Exception) {
                Status = Status
            };
        }

        public T Value { get; }

        public static implicit operator RequestResponse<T>(T value) => new RequestResponse<T>(RequestStatus.Ok, value);

        public static implicit operator RequestResponse<T>(Exception ex) => new RequestResponse<T>(ex);
    }

}
