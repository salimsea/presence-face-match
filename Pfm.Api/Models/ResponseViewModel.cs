using System;
namespace Pfm.Api.ViewModels
{
    public class ResponseViewModel<T>
    {
        public bool IsSuccess { get; set; }
        public string? ReturnMessage { get; set; }
        public T? Data { get; set; }
    }
}

