using Microsoft.AspNetCore.Mvc;
using DiffMatchPatch;
using TiptapWebApi.Services;
using System.Collections.Generic;

namespace TiptapWebApi.Controllers
{
    [ApiController]
    [Route("document")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        public DocumentController(IDocumentService documentService)
        {
            ArgumentNullException.ThrowIfNull(documentService, nameof(documentService));
            _documentService = documentService;
        }


        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("ok");
        }

        [HttpPatch("{documentId}")]
        public async Task<IActionResult> ApplyPatches(int documentId)
        {
            if (!await _documentService.ExistsAsync(documentId))
                return NotFound();

            using StreamReader sr = new StreamReader(Request.Body);
            var patchesStr = await sr.ReadToEndAsync();
            await _documentService.UpdateAsync(documentId, patchesStr);
            return Ok();
        }

        [HttpPost("{documentId}")]
        public async Task<IActionResult> Create()
        {
            var documentId = await _documentService.CreateAsync();
            return Ok(documentId);
        }
    }
}