namespace Forum.Application.UseCases.Like.Comment
{
    public interface IToggleCommentLikeUseCase
    {
        Task Execute(long commentId);
    }
}
