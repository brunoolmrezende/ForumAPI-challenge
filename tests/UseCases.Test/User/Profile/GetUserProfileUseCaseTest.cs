using CommonTestUtilities.AutoMapper;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using Forum.Application.UseCases.User.Profile;

namespace UseCases.Test.User.Profile
{
    public class GetUserProfileUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var useCase = CreateUseCase(user);

            var result = await useCase.Execute();

            result.Should().NotBeNull();
            result.Email.Should().Be(user.Email);
            result.Name.Should().Be(user.Name);
            result.ImageUrl.Should().Be(user.ImageUrl);
            result.TopicsCount.Should().BeGreaterThanOrEqualTo(0);
            result.CommentsCount.Should().BeGreaterThanOrEqualTo(0);
        }

        private static GetUserProfileUseCase CreateUseCase(Forum.Domain.Entities.User user)
        {
            var loggedUser = LoggedUserBuilder.Build(user);
            var repository = new UserReadOnlyRepositoryBuilder().GetProfile(user).Build();
            var mapper = MapperBuilder.Build();

            return new GetUserProfileUseCase(loggedUser, repository, mapper);
        }
    }
}
