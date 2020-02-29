﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinPlan.BackEnd.Attributes;
using FinPlan.BackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FinPlan.BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FinanceController : ControllerBase
    {
        private readonly ILogger<FinanceController> _logger;
        private readonly IFileUploadService _fileUploadService;
        public FinanceController(ILogger<FinanceController> logger, IFileUploadService fileService)
        {
            _logger = logger;
            _fileUploadService = fileService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [GenerateAntiforgeryTokenCookie]
        [DisableFormValueModelBinding]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadAccountStatementFile(IFormFile file)
        {
            await _fileUploadService.HandleUploadRequestAsync(file);
            return Ok();
        }
    }
}
