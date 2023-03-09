using System;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Segurplan.FrameworkExtensions.MediatR {
    public interface IRequestResponse : IConvertToActionResult {
        RequestStatus Status { get; }

        Exception Exception { get; }

        string Message { get; }

        ValidationResult ValidationResult { get; set; }
    }

    public interface IRequestResponse<out T> : IRequestResponse {
        T Value { get; }

        IRequestResponse<T1> As<T1>();
    }
}
