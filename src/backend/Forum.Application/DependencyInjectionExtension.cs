using AutoMapper;
using Forum.Application.Services.AutoMapper;
using Forum.Application.UseCases.Comment.Delete;
using Forum.Application.UseCases.Comment.Register;
using Forum.Application.UseCases.Comment.Update;
using Forum.Application.UseCases.Forum;
using Forum.Application.UseCases.Like;
using Forum.Application.UseCases.Login.DoLogin;
using Forum.Application.UseCases.Token;
using Forum.Application.UseCases.Topic.Delete;
using Forum.Application.UseCases.Topic.Filter;
using Forum.Application.UseCases.Topic.GetById;
using Forum.Application.UseCases.Topic.Register;
using Forum.Application.UseCases.Topic.Update;
using Forum.Application.UseCases.User.Change_Password;
using Forum.Application.UseCases.User.Delete.Delete_User_Account;
using Forum.Application.UseCases.User.Delete.Request;
using Forum.Application.UseCases.User.Delete_Image;
using Forum.Application.UseCases.User.Image;
using Forum.Application.UseCases.User.Profile;
using Forum.Application.UseCases.User.Register;
using Forum.Application.UseCases.User.Update;
using Microsoft.Extensions.DependencyInjection;

namespace Forum.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            AddAutoMapper(services);
            AddUseCases(services);
        }

        private static void AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
            services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
            services.AddScoped<IAddUpdateImageUseCase, AddUpdateImageUseCase>();
            services.AddScoped<IDeleteImageUseCase, DeleteImageUseCase>();
            services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
            services.AddScoped<IRequestDeleteUserUseCase, RequestDeleteUserUseCase>();
            services.AddScoped<IDeleteUserAccountUseCase, DeleteUserAccountUseCase>();

            services.AddScoped<IUseRefreshTokenUseCase, UseRerfreshTokenUseCase>();

            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();

            services.AddScoped<IRegisterTopicUseCase, RegisterTopicUseCase>();
            services.AddScoped<IUpdateTopicUseCase, UpdateTopicUseCase>();
            services.AddScoped<IDeleteTopicUseCase, DeleteTopicUseCase>();
            services.AddScoped<IGetTopicByIdUseCase, GetTopicByIdUseCase>();
            services.AddScoped<IGetAllTopicsUseCase, GetAllTopicsUseCase>();
            services.AddScoped<IFilterTopicUseCase, FilterTopicUseCase>();

            services.AddScoped<IRegisterCommentUseCase, RegisterCommentUseCase>();
            services.AddScoped<IUpdateCommentUseCase, UpdateCommentUseCase>();
            services.AddScoped<IDeleteCommentUseCase, DeleteCommentUseCase>();

            services.AddScoped<IToggleLikeUseCase, ToggleLikeUseCase>();
        }

        private static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddScoped(options => new MapperConfiguration(autoMapperOptions =>
            {
                autoMapperOptions.AddProfile(new AutoMapping());
            }).CreateMapper());
        }
    }
}
