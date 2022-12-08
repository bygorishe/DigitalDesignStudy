using Api.Models.Attach;
using Api.Services;
using AutoMapper;
using DAL.Entities.Attaches;

namespace Api.Mapper.MapperActions
{
    public class PostContentMapperAction : IMappingAction<PostImage, AttachExternalModel>
    {
        private LinkGeneratorService _links;
        public PostContentMapperAction(LinkGeneratorService linkGeneratorService)
        {
            _links = linkGeneratorService;
        }
        public void Process(PostImage source, AttachExternalModel destination, ResolutionContext context)
            => _links.FixContent(source, destination);
    }
}