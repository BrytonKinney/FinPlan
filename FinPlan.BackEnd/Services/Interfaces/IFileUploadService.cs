using FinPlan.BackEnd.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinPlan.BackEnd.Services.Interfaces
{
    public interface IFileUploadService
    {
        Task HandleUploadRequestAsync(Microsoft.AspNetCore.Http.HttpRequest request);
        Task<IEnumerable<Transaction>> HandleUploadRequestAsync(IFormFile file);
    }
}
