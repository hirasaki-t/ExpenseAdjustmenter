using Microsoft.AspNetCore.Mvc;
using Query.QueryServices;

namespace WebAPI.Controllers;

/// <summary>システム設定コントローラー</summary>
[ApiController]
[Route("api/[Controller]")]
public class SystemSettingsController : ControllerBase
{
    private readonly DeadlineQueryService deadlineQueryService;

    public SystemSettingsController(DeadlineQueryService deadlineQueryService)
    {
        this.deadlineQueryService = deadlineQueryService;
    }

    /// <summary>締日を取得する</summary>
    [HttpGet("Deadline")]
    public ActionResult<DateOnly> GetDeadlineAsync()
    {
        return Ok(deadlineQueryService.GetDeadline());
    }
}