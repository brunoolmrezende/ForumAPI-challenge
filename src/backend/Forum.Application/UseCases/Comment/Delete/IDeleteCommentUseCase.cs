﻿namespace Forum.Application.UseCases.Comment.Delete
{
    public interface IDeleteCommentUseCase
    {
        Task Execute(long topicId, long commentId);
    }
}
