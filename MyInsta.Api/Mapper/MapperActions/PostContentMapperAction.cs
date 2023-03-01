using MyInsta.Api.Models.Attach;
using MyInsta.Api.Services;
using AutoMapper;
using MyInsta.DAL.Entities.Attaches;

namespace MyInsta.Api.Mapper.MapperActions
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