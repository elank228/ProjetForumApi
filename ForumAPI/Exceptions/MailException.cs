using System;

namespace ForumApi.Exeptions
{
    public class MailException : Exception
    {
        public MailException(string message) : base(message)
        {

        }
    }
}