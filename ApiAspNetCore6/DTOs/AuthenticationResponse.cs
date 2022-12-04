namespace ApiAspNetCore6.DTOs
{
    public class AuthenticationResponse
    {
        public AuthenticationResponse()
        {
            
        }
        public string Token { get; set; }
        public DateTime Expires { get; set; }

    }

    }
}
