using System;

namespace ForumApi.Exeptions
{
    [Serializable]
    public class BlobStorageException : Exception
    {
        public BlobStorageException(string message) : base(message)
        {

        }
    }
}