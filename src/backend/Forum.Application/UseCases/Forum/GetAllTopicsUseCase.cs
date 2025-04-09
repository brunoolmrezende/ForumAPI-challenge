using AutoMapper;
using Forum.Communication.Response;
using Forum.Domain.Repository.Topic;

namespace Forum.Application.UseCases.Forum
{
    public class GetAllTopicsUseCase(
        ITopicReadOnlyRepository readOnlyRepository,
        IMapper mapper) : IGetAllTopicsUseCase
    {
        private readonly ITopicReadOnlyRepository _readOnlyRepository1 = readOnlyRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseTopicsJson> Execute()
        {
            var topics = await _readOnlyRepository1.GetAllTopics();

            return new ResponseTopicsJson
            {
                Topics = _mapper.Map<List<ResponseTopicDetailsJson>>(topics),
            };
        }
    }
}
