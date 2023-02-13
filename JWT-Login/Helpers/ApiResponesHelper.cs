using JWT_Login.Models;

namespace JWT_Login.Helpers
{
    public static class ApiResponesHelper
    {
        public static ApiResponse GetApiResponses(string message, bool result)
        {
            return new ApiResponse() { Message= message, Success=result };
        }
    }
}
