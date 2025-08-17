namespace NewsLetter.Server.Model
{
    public class Stories
    {
        public int id { get; set; }
        public string? type { get; set; }
        public string? by { get; set; }
        public long time { get; set; }
        public string? title { get; set; }
        public string? url { get; set; }
        public bool? dead { get; set; }
        public bool? deleted { get; set; }
    }
}
