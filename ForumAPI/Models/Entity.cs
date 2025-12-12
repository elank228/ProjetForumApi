namespace ForumAPI.Models
{
    public class Entity
    {
        public DateTime? CreationDateTime { get; set; }
        public DateTime? ModificationDateTime { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        protected Entity() { 
            CreationDateTime = DateTime.UtcNow;
        }

    }
}
