namespace Forum.Infrastructure.DataAccess.Migrations
{
    public abstract class DatabaseVersions
    {
        public const int TABLE_USERS = 1;
        public const int TABLE_TOPICS = 2;
        public const int TABLE_COMMENTS = 3;
        public const int TABLE_TOPIC_LIKES = 4;
        public const int TABLE_REFRESH_TOKENS = 5;
        public const int IMAGES_FOR_USERS = 6;
    }
}
