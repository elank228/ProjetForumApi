namespace ForumApi.Exeptions
{
    public class MaxSizeReachedException : Exception
    {
        public MaxSizeReachedException(string message) : base(message)
        {

        }
    }
}