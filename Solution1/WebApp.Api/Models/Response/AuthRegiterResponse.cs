using System.Collections.Generic;

namespace WebApp.Api.Models.Response
{
    public class AuthRegiterResponse
    {
        public string UserId { get; set; }
        public string Msg { get; set; }
        public List<string> ErrorMessages { get; set; }
        public bool Success { get; set; }
    }
}
