namespace Common.Responses.Wrappers
{
    public class ResponseWrapper : IResponseWrapper
    {
        public ResponseWrapper()
        {

        }
        public List<string> Messages { get; set; } = new();
        public bool IsSuccessful { get; set; }

        public static IResponseWrapper Fail()
        {
            return new ResponseWrapper { IsSuccessful = false };
        }
        public static IResponseWrapper Fail(string message)
        {
            return new ResponseWrapper { IsSuccessful = false, Messages = new List<string> { message } };
        }
        public static IResponseWrapper Fail(List<string> messages)
            => new ResponseWrapper { IsSuccessful = false, Messages = messages };

        public static Task<IResponseWrapper> FailAsync() => Task.FromResult(Fail());

        public static Task<IResponseWrapper> FailAsync(string message) => Task.FromResult(Fail(message));

        public static Task<IResponseWrapper> FailAsync(List<string> messages) => Task.FromResult(Fail(messages));


        public static IResponseWrapper Success() => new ResponseWrapper { IsSuccessful = true };
        public static IResponseWrapper Success(string message)
            => new ResponseWrapper { IsSuccessful = true, Messages = new List<string> { message } };
        public static Task<IResponseWrapper> SuccessAsync() => Task.FromResult(Success());
        public static Task<IResponseWrapper> SuccessAsync(string message) => Task.FromResult(Success(message));

    }

    public class ResponseWrapper<T> : ResponseWrapper, IResponseWrapper<T>
    {
        public ResponseWrapper()
        {

        }
        public T ResponseData { get; set; }

        public new static ResponseWrapper<T> Fail() => new ResponseWrapper<T> { IsSuccessful = false };
        public new static ResponseWrapper<T> Fail(string message)
            => new ResponseWrapper<T> { IsSuccessful = false, Messages = new List<string> { message } };
        public new static ResponseWrapper<T> Fail(List<string> messages)
            => new ResponseWrapper<T> { IsSuccessful = false, Messages = messages };

        public new static Task<ResponseWrapper<T>> FailAsync() => Task.FromResult(Fail());
        public new static Task<ResponseWrapper<T>> FailAsync(string message)
            => Task.FromResult(Fail(message));
        public new static Task<ResponseWrapper<T>> FailAsync(List<string> messages)
            => Task.FromResult(Fail(messages));

        public new static ResponseWrapper<T> Success() => new ResponseWrapper<T> { IsSuccessful = true };

        public new static ResponseWrapper<T> Success(string message)
           => new ResponseWrapper<T> { IsSuccessful = true, Messages = new List<string> { message } };

        public new static ResponseWrapper<T> Success(T data) => new ResponseWrapper<T> { IsSuccessful = true, ResponseData = data };

        public new static ResponseWrapper<T> Success(T data, string message)
            => new ResponseWrapper<T> { IsSuccessful = true, ResponseData = data, Messages = new List<string> { message } };

        public new static ResponseWrapper<T> Success(T data, List<string> messages)
            => new ResponseWrapper<T> { IsSuccessful = true, ResponseData = data, Messages = messages };

        public new static Task<ResponseWrapper<T>> SuccessAsync()=> Task.FromResult(Success());
        public new static Task<ResponseWrapper<T>> SuccessAsync(string message) => Task.FromResult(Success(message));
        public new static Task<ResponseWrapper<T>> SuccessAsync(T data) => Task.FromResult(Success(data));
        public new static Task<ResponseWrapper<T>> SuccessAsync(T data,string message) 
            => Task.FromResult(Success(data,message));


    }
}
