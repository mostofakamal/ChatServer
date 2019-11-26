namespace WebApi.Core.Domain.Entities
{
    public class MessageHistory : BaseEntity
    {
        public int GroupId { get; set; }

        public int PlayerId { get; set; }

        public Group Group { get; set; }

        public Player Player { get; set; }

        public string Message { get; set; }
    }
}