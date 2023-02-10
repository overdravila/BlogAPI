namespace BlogAPI.Contracts.Requests
{
    public class CommentRequest
    {
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int AuthorId { get; set; }
        public int PostId { get; set; }
    }
}
