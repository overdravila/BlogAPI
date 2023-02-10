namespace BlogAPI.Contracts.Requests
{
    public class PostRejectionCommentRequest
    {
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public int PostId { get; set; }
    }
}
