using Microsoft.AspNetCore.Mvc;
using DiffMatchPatch;

[ApiController]
[Route("document")]
public class DocumentController : ControllerBase
{
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("ok");
    }

    [HttpPost("apply-diffs")]
    public async Task<IActionResult> ApplyDiff(int documentId, [FromBody] List<Patch> patches)
    {
        var document = "";
        var dmp = new diff_match_patch();
        dmp.patch_apply(patches, document);
        return Ok();
    }
}