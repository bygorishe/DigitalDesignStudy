namespace Api.Models.Mail
{
    public class MailModel
    {
        public List<string> To { get; }
        public List<string> Bcc { get; }
        public List<string> Cc { get; }
        public string? From { get; }
        public string? DisplayName { get; }
        public string? ReplyTo { get; }
        public string? ReplyToName { get; }
        public string Subject { get; }
        public string? Body { get; }
        public IFormFileCollection? Attachments { get; set; }

        public MailModel(List<string> to, string subject, string? body = null, string? from = null, string? displayName = null, string? replyTo = null, string? replyToName = null, List<string>? bcc = null, List<string>? cc = null)
        {
            // Receiver
            To = to;
            Bcc = bcc ?? new List<string>();
            Cc = cc ?? new List<string>();

            // Sender
            From = from;
            DisplayName = displayName;
            ReplyTo = replyTo;
            ReplyToName = replyToName;

            // Content
            Subject = subject;
            Body = body;
        }
    }

    //public class MailModelWithAttach : MailModel
    //{
    //    public IFormFileCollection? Attachments { get; set; }
    //}
}
