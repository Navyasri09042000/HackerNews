namespace NewsLetter.Server.Model
{
    public record StoryDto(
      int Id,
      string Title,
      string Link,
      long UnixTime
  );
}
