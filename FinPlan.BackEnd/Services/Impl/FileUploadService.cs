using FinPlan.BackEnd.Data;
using FinPlan.BackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FinPlan.BackEnd.Services.Impl
{
    public class FileUploadService : IFileUploadService
    {
        private const int MULTIPART_BOUNDARY_LENGTH_LIMIT = 70;

        private readonly ICsvParser _csvParser;
        public FileUploadService(ICsvParser csvParser)
        {
            _csvParser = csvParser;
        }

        public async Task<IEnumerable<Transaction>> HandleUploadRequestAsync(IFormFile file)
        {
            using (var fileStream = File.Create(Path.Combine("../data", $"{Path.GetRandomFileName()}{Path.GetExtension(file.FileName)}")))
            {
                await file.CopyToAsync(fileStream);
                return await _csvParser.ParseFileAsync(file.OpenReadStream());
            }
        }
        public async Task HandleUploadRequestAsync(HttpRequest request)
        {
            if (!IsMultipartContentType(request.ContentType))
                return;
            var formKvpAccum = new KeyValueAccumulator();
            var boundary = GetBoundary(MediaTypeHeaderValue.Parse(request.ContentType),
                                       MULTIPART_BOUNDARY_LENGTH_LIMIT);
            var mpFormReader = new MultipartReader(boundary, request.Body);
            var section = await mpFormReader.ReadNextSectionAsync();
            using (var fileStream = File.Create(Path.Join(string.Join(Path.GetRandomFileName(), ".csv"))))
            {
                while (section != null)
                {
                    var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDispositionHeader);
                    if (hasContentDispositionHeader)
                    {
                        if (HasFileContentDisposition(contentDispositionHeader))
                        {
                            await section.Body.CopyToAsync(fileStream);
                        }
                    }
                    section = await mpFormReader.ReadNextSectionAsync();
                }
            }
        }

        private async Task<string> ProcessFileStreamAsync(MultipartSection section, ContentDispositionHeaderValue contentDisposition, string[] allowedExts, long sizeLimit)
        {
            string resultingFileName = string.Empty;
            try
            {
                using (var fs = new FileStream(Path.Join("../../../", string.Join(Path.GetRandomFileName(), ".csv")), FileMode.Create))
                {
                    await section.Body.CopyToAsync(fs);
                    resultingFileName = fs.Name;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return resultingFileName;
        }

        // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
        // The spec at https://tools.ietf.org/html/rfc2046#section-5.1 states that 70 characters is a reasonable limit.
        public string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        {
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException(
                    $"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary;
        }

        public bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public bool HasFormDataContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="key";
            return contentDisposition != null
                && contentDisposition.DispositionType.Equals("form-data")
                && string.IsNullOrEmpty(contentDisposition.FileName.Value)
                && string.IsNullOrEmpty(contentDisposition.FileNameStar.Value);
        }

        public bool HasFileContentDisposition(ContentDispositionHeaderValue contentDisposition)
        {
            // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
            return contentDisposition != null
                && contentDisposition.DispositionType.Equals("form-data")
                && (!string.IsNullOrEmpty(contentDisposition.FileName.Value)
                    || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
        }
    }
}
