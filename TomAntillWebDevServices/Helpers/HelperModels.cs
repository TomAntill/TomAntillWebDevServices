using SendGrid.Helpers.Mail;

namespace TomAntillWebDevServices.Helpers
{
    public class HelperModels
    {
        public class SendToModel
        {
            public SendToModel(string emailAddress, string templateId, int id)
            {
                TemplateId = templateId;
                EmailAddress = new EmailAddress(emailAddress, emailAddress);
                EmailSettingsId = id;
            }
            public EmailAddress EmailAddress { get; set; }
            public string TemplateId { get; set; }
            public int EmailSettingsId { get; set; }
        }
    }
}
