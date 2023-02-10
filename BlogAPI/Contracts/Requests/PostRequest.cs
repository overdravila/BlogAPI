namespace BlogAPI.Contracts.Requests
{
    public class PostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateOfPublishing { get; set; }
        public int AuthorId { get; set; }
    }
}
