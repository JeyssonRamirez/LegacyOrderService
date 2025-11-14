using Swashbuckle.AspNetCore.Annotations;

namespace Core.DataTransferObject
{
    public class RecordApiResult<T> : BaseApiResult
    {
        /// <summary>
        /// RecordId
        /// </summary>        
        public new T Data { get; set; }
    }
    public class RecordIdApiResult : BaseApiResult
    {
        /// <summary>
        /// RecordId
        /// </summary>
        [SwaggerSchema(Description = "Record Id")]
        public new long Data { get; set; }
    }
}
