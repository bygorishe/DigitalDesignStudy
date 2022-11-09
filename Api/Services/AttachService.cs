using Api.Configs;
using Api.Models.Attach;
using AutoMapper;
using DAL;
using Microsoft.Extensions.Options;

namespace Api.Services
{
    public class AttachService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly AuthConfig _config;

        public AttachService(IMapper mapper, DataContext context, IOptions<AuthConfig> config)
        {
            _mapper = mapper;
            _context = context;
            _config = config.Value;
        }

        public async Task<MetadataModel> UploadFile(IFormFile file)
        {
            var tempPath = Path.GetTempPath();
            var meta = new MetadataModel
            {
                TempId = Guid.NewGuid(),
                Name = file.FileName,
                MimeType = file.ContentType,
                Size = file.Length,
            };

            var newPath = Path.Combine(tempPath, meta.TempId.ToString());

            var fileinfo = new FileInfo(newPath);
            if (fileinfo.Exists)
                throw new Exception("file exist");
            else
            {
                if (fileinfo.Directory == null)
                    throw new Exception("temp is null");
                else
                if (!fileinfo.Directory.Exists)
                    fileinfo.Directory?.Create();
                using (var stream = System.IO.File.Create(newPath))
                    await file.CopyToAsync(stream);
                return meta;
            }
        }
    }
}
