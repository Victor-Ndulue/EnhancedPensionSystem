﻿using System.Text.Json;

namespace EnhancedPensionSystem_Application.Helpers.ObjectFormatter;

public class StandardResponse<T>
{
    public T Data { get; set; }
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public DateTime? RequestDate { get; set; } = DateTime.UtcNow;

    public StandardResponse(int statusCode, bool success, string msg, T data)
    {
        Data = data;
        Succeeded = success;
        StatusCode = statusCode;
        Message = msg;
    }
    public StandardResponse()
    {
    }

    /// <summary>
    /// Application custom response message, 400 response means User Error
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    public static StandardResponse<T> Failed(T? data, string errorMessage = "Request failed", int statusCode = 400)
    {
        return new StandardResponse<T> { Succeeded = false, Message = errorMessage, StatusCode = statusCode, Data = data };
    }

    /// <summary>
    /// Application custom response message, 200 means successful
    /// </summary>
    /// <param name="successMessage"></param>
    /// <param name="data"></param>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    public static StandardResponse<T> Success(T? data, int statusCode = 200, string? successMessage = "Success")
    {
        return new StandardResponse<T> { Succeeded = true, Message = successMessage, Data = data, StatusCode = statusCode };
    }

    /// <summary>
    /// Application custom response message, 202 means accepted 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    public static StandardResponse<T> Pending(T? data, string? message = "Accepted", int statusCode = 202)
    {
        return new StandardResponse<T> { Succeeded = true, Message = message, Data = data, StatusCode = statusCode };
    }

    /// <summary>
    /// Application custom response message, 503 means Service Unavailable
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    public static StandardResponse<T> UnExpectedError(T? data, string? message = "Service currently unavailable", int statusCode = 503)
    {
        return new StandardResponse<T> { Succeeded = false, Message = message, Data = data, StatusCode = statusCode };
    }

    public override string ToString() => JsonSerializer.Serialize(this);
}
