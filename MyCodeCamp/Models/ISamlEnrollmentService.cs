namespace MyCodeCamp.Models
{
    public interface ISamlEnrollmentService
    {
        string ProcessSamlRequest(string samlRequest);
        string ProcessSamlResponse(string samlResponse, string relayState = "");
        void SetCookie(string key, string value, int? expireTime);
        string GetCookie(string key);
    }
}
