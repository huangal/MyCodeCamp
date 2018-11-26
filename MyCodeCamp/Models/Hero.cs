using System;
namespace MyCodeCamp.Models
{

    public class CampUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSecret { get; set; }

    }



    public class Hero
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSecret { get; set; }

    }

    public class WeblogConfiguration
    {
        public string ApplicationName { get; set; }
        public string ApplicationBasePath { get; set; } = "/";
        public int PostPageSize { get; set; } = 10000;
        public int HomePagePostCount { get; set; } = 30;
        public string PayPalEmail { get; set; }
        public EmailConfiguration Email { get; set; } = new EmailConfiguration();
    }

    public class EmailConfiguration
    {
        public string MailServer { get; set; }
        public string MailServerUsername { get; set; }
        public string MailServerPassword { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string AdminSenderEmail { get; set; }
    }

    public class ConfigAdmin{

        public WeblogConfiguration Weblog { get; set; } = new WeblogConfiguration();
    }

    public class Weblog
    {
        public string ApplicationName { get; set; }
        public string ApplicationBasePath { get; set; } = "/";
        public int PostPageSize { get; set; } = 10000;
        public int HomePagePostCount { get; set; } = 30;
        public string PayPalEmail { get; set; }
        public EmailConfiguration Email { get; set; } = new EmailConfiguration();
    }

    /*
 "Weblog": {
    "ApplicationName": "Rick Strahl's WebLog (local)",
    "ApplicationBasePath": "/",
    "ConnectionString": "server=.;database=WeblogCore;integrated security=true;MultipleActiveResultSets=True",
    "PostPageSize": 7600,
    "HomePagePostCount": 25,
    "Email": {
      "MailServer": "mail.site.com",
      "MailServerUsername": "name@site.com",
      "MailServerPassword": "nicetry",
      "SenderEmail": "admin@site.com",
      "SenderName": "West Wind Weblog",
      "AdminSenderEmail": "admin Administration"
    }
*/
}
