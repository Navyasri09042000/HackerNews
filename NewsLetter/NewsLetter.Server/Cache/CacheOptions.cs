namespace NewsLetter.Server.Cache
{
    public class CacheOptions
    {
        public int NewestIdsTtlSeconds { get; set; } = 60;      // cache list of newest IDs
        public int StoryTtlSeconds { get; set; } = 300;          // cache item details
        public int MaxItemsToHydrate { get; set; } = 300;        // safety limit
    }
}
