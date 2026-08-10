using System;
using System.Collections.Generic;
using System.Text;

namespace ModularCommerce.Shared.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string? ErrorMessage { get; }

        private Result(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Success() => new(true, null);
        public static Result Fail(string errorMessage) => new(false, errorMessage);
    }

    public class Result<T>
    {
        public T? Value { get; }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string? ErrorMessage { get; }

        private Result(bool isSuccess, T? value, string? errorMessage) {
            IsSuccess = isSuccess;
            Value = value;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Success(T value) => new(true, value, null);
        public static Result<T> Fail(string errorMessage) => new(false, default, errorMessage);
    }
}
