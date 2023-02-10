namespace BlogAPI.Biz.Models
{
    public class CommentResponse
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string Author { get; set; }
        public int PostId { get; set; }
    }
}