using System;
namespace MyCodeCamp.Helpers
{
    public class AppSettings: IAppSettings
    {
        public string Secret { get; set; }
    }
}
