﻿using Microsoft.AspNetCore.Http;
using ProtoBuf;
using TaikoLocalServer.Utils;

namespace TaikoLocalServer.Controllers;

[Route("/v12r03/chassis/playresult.php")]
[ApiController]
public class PlayResultController : ControllerBase
{
    private readonly ILogger<PlayResultController> logger;
    public PlayResultController(ILogger<PlayResultController> logger) {
        this.logger = logger;
    }

    [HttpPost]
    [Produces("application/protobuf")]
    public IActionResult UploadPlayResult([FromBody] PlayResultRequest request)
    {
        logger.LogInformation("PlayResult request : {Request}", request.Stringify());
        var decompressed = GZipBytesUtil.DecompressGZipBytes(request.PlayresultData);

        var playResultData = Serializer.Deserialize<PlayResultDataRequest>(new ReadOnlySpan<byte>(decompressed));
        
        logger.LogInformation("Play result data {Data}", playResultData.Stringify());

        var response = new PlayResultResponse
        {
            Result = 1
        };

        return Ok(response);
    }
}