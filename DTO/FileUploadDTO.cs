using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DTO
{
    public class FileUploadDTO
    {
        public IFormFile File { get; set; }
        public string FileName => File?.FileName;

        public Stream OpenReadStream()
        {
            throw new NotImplementedException();
        }
    }
}