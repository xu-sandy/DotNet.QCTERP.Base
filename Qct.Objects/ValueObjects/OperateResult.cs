using Newtonsoft.Json;

namespace Qct.Objects.ValueObjects
{
    public class OperateResult : OperateResult<object, object>
    {
        public static OperateResult Success(string message = null, string code = "Success", object data = null)
        {
            return new OperateResult()
            {
                Successed = true,
                Message = message,
                Code = code,
                Data = data
            };
        }
        public static OperateResult Fail(string message = null, string code = "Fail", object data = null)
        {
            return new OperateResult()
            {
                Successed = false,
                Message = message,
                Code = code,
                Data = data
            };
        }
        public static OperateResult Result(bool result)
        {
            return new OperateResult()
            {
                Successed = result,
                Message = ""
            };
        }
    }
    public class OperateResult<TData> : OperateResult<TData, object>
    {
    }

    public class OperateResult<TData, TErrorData>
    {
        [JsonProperty("successed")]
        public bool Successed { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public TData Data { get; set; }
        public TErrorData ErrorData { get; set; }
        [JsonProperty("descript")]
        public string Descript { get; set; }
    }
}
