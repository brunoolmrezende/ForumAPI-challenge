using AutoMapper;
using Forum.Communication.Request;
using Forum.Communication.Response;
using Forum.Domain.Dtos;
using Forum.Domain.Repository.Topic;
using Forum.Exceptions.ExceptionBase;

namespace Forum.Application.UseCases.Topic.Filter
{
    public class FilterTopicUseCase(
        ITopicReadOnlyRepository repository,
        IMapper mapper) : IFilterTopicUseCase
    {
        private readonly ITopicReadOnlyRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseTopicsJson> Execute(RequestFilterTopicJson request)
        {
            Validate(request);

            var filters = new FilterTopicDto
            {
                TopicTitle = request.Title,
                TopicContent = request.Content,
                OrderBy = request.OrderBy
            };

            var recipes = await _repository.Filter(filters);

            return new ResponseTopicsJson
            {
                Topics = _mapper.Map<List<ResponseTopicDetailsJson>>(recipes)
            };
        }

        private void Validate(RequestFilterTopicJson request)
        {
            var validator = new FilterTopicValidator();

            var result = validator.Validate(request);

            if (result.IsValid is false)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
