using AutoMapper;
using Forum.Communication.Request;
using Forum.Communication.Response;
using Forum.Domain.Entities;

namespace Forum.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToDomain();
            DomainToResponse();
        }

        private void RequestToDomain()
        {
            CreateMap<RequestRegisterUserJson, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<RequestTopicJson, Topic>();
        }

        private void DomainToResponse()
        {
            CreateMap<Topic, ResponseRegisteredTopicJson>();

            CreateMap<Comment, ResponseRegisteredCommentJson>();

            CreateMap<Comment, ResponseCommentJson>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.AuthorPhotoUrl, opt => opt.MapFrom(src => src.User.ImageUrl));

            CreateMap<User, ResponseUserSummaryJson>();

            CreateMap<Topic, ResponseTopicDetailsJson>()
                .ForMember(dest => dest.TotalLikes, opt => opt.MapFrom(src => src.Likes.Count))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.TotalComments, opt => opt.MapFrom(src => src.Comments.Count));
        }
    }
}
