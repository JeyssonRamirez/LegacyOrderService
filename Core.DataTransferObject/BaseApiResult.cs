namespace Core.DataTransferObject
{
    public class BaseApiResult
    {
        
        public bool Success { get; set; }

        public MessageCodeType MessageCode { get; set; }

        
        public string Message { get; set; }

        
        public int Code
        {
            get { return (int)MessageCode; }
        }

        public virtual object Data { set; get; }
    }
}
