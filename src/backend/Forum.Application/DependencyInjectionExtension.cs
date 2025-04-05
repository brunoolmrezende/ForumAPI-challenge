using AutoMapper;
using Forum.Application.Services.AutoMapper;
using Forum.Application.UseCases.Login.DoLogin;
using Forum.Application.UseCases.Topic.Register;
using Forum.Application.UseCases.Topic.Update;
using Forum.Application.UseCases.User.Register;
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

            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();

            services.AddScoped<IRegisterTopicUseCase, RegisterTopicUseCase>();
            services.AddScoped<IUpdateTopicUseCase, UpdateTopicUseCase>();
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
