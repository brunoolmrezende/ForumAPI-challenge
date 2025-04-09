namespace Forum.Domain.Dtos
{
    public class FilterTopicDto
    {
        public string? TopicTitle { get; set; } = string.Empty;
        public string? TopicContent { get; set; } = string.Empty;
        public string? OrderBy { get; set; } = "createdOn";
    }
}
