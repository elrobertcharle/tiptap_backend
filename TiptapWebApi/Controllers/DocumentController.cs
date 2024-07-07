using Microsoft.AspNetCore.Mvc;
using TiptapWebApi.Services;

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


        [HttpGet("{documentId}")]
        public async Task<IActionResult> Get(int documentId)
        {
            if (!await _documentService.ExistsAsync(documentId))
                return NotFound();
            return Ok(await _documentService.GetByIdAsync(documentId));
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

        [HttpPost()]
        public async Task<IActionResult> Create()
        {
            using StreamReader sr = new StreamReader(Request.Body);
            var content = await sr.ReadToEndAsync();

            // just for testing
            if (string.IsNullOrEmpty(content))
                content = "{\"type\": \"doc\",\"content\": [{\"type\": \"paragraph\",\"content\": [{\"type\": \"text\",\"text\": \"hello\"}]}]}";

            var documentId = await _documentService.CreateAsync(content);
            return Ok(documentId);
        }
    }
}