using System.ComponentModel;

namespace Core.DataTransferObject
{
    public enum MessageCodeType
    {
        [Description("OK")]
        OK = 200,

        [Description("Bad Request")]
        BadRequest = 400,
        [Description("UnAuthorized")]
        UnAuthorized = 401,
        [Description("Forbidden")]
        Forbidden = 403,
        [Description("Not Found")]
        NotFound = 404,


        [Description("User already exist With This Email")]
        UserAlreadyExistWithThisEmail = 418,
        [Description("User already exist With This Identification Number")]
        UserAlreadyExistWithThisIdentification = 419,
        [Description("User already exist")]
        UserAlreadyExist = 420,

        //use 452 to 499...

        [Description("No data found")]
        NotRecordFound = 452,

        [Description("Internal server error")]
        InternalServerError = 500,

    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IgnoredForSwaggerAttribute : Attribute
    {
    }
}
