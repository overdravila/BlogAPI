namespace BlogAPI.Biz.Models
{
    public class PostResponse
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateOfPublishing { get; set; }
        public string PostStatus { get; set; }
        public string Author { get; set; }
    }
}