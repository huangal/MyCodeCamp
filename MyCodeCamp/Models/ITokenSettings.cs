namespace MyCodeCamp.Models
{
    public interface ITokenSettings
    {
        string SecretKey { get;  }
        string Issuer { get; set; }
        string Audience { get; set; }
    }
}