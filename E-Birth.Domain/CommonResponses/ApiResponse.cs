using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Birth.Domain.CommonResponses
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public List<string> Errors { get; set; }
        public static ApiResponse<T> Success(T data, int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Data = data,
                StatusCode = statusCode,
            };
        }
        public static ApiResponse<T> ErrorResponse(List<string> errors, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Errors = errors,
                StatusCode = statusCode,
            };
        }
        public static ApiResponse<T> ErrorResponse(string error, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Errors = new List<string> { error },
                StatusCode = statusCode,
            };
        }
    }
}
