namespace ForumApi.Exeptions
{
    public class WrongExtensionException : Exception
    {
        public WrongExtensionException(string message) : base(message)
        {

        }
    }
}