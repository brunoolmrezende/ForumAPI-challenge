namespace Forum.Domain.Dtos
{
    public sealed record ImageUploadResultDto
    {
        public string PublicId { get; init; } = null!;
        public string Url { get; init; } = null!;
    }
}
