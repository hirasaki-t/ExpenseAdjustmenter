using Microsoft.AspNetCore.Mvc;
using Query.Datas;
using Query.QueryServices;
using Usecase.ApproveHistories;

namespace WebAPI.Controllers;

/// <summary>承認履歴コントローラー</summary>
[ApiController]
[Route("api/[Controller]")]
public class ApproveHistoriesController : ControllerBase
{
    private readonly ApproveHistoryQueryService approveHistoryQueryService;
    private readonly ApproveHistoryApplicationer approveHistoryApplicationer;
    private readonly ApproveHistoryApprover approveHistoryApprover;
    private readonly ApproveHistoryRejecter approveHistoryRejecter;

    public ApproveHistoriesController(
        ApproveHistoryQueryService approveHistoryQueryService,
        ApproveHistoryApplicationer approveHistoryApplicationer,
        ApproveHistoryApprover approveHistoryApprover,
        ApproveHistoryRejecter approveHistoryRejecter)
    {
        this.approveHistoryQueryService = approveHistoryQueryService;
        this.approveHistoryApplicationer = approveHistoryApplicationer;
        this.approveHistoryApprover = approveHistoryApprover;
        this.approveHistoryRejecter = approveHistoryRejecter;
    }

    /// <summary>承認履歴がある年月一覧を取得する</summary>
    [HttpGet("SelectableMonths")]
    public async Task<ActionResult<IEnumerable<DateOnly>>> GetAsync()
    {
        return Ok(await approveHistoryQueryService.GetSelectableMonthsAsync());
    }

    /// <summary>対象年月の承認履歴一覧を取得する</summary>
    [HttpGet("{year}/{month}")]
    public async Task<ActionResult<IEnumerable<ApproveHistoryData>>> GetsAsync(int year, int month)
    {
        return Ok(await approveHistoryQueryService.GetAsync(new DateOnly(year, month, 1)));
    }

    /// <summary>経費IDに紐付く承認履歴一覧を取得する</summary>
    [HttpGet("{expenseId}")]
    public async Task<ActionResult<IEnumerable<ApproveHistoryListData>>> GetAsync(string expenseId)
    {
        return Ok(await approveHistoryQueryService.GetAsync(expenseId));
    }

    /// <summary>経費精算を申請する</summary>
    [HttpPost("Application")]
    public async Task<IActionResult> AddRangeAsync(ApproveHistoryApplicationRequest request)
    {
        await approveHistoryApplicationer.ApplicationAsync(request.Date);
        return Ok();
    }

    /// <summary>経費精算の申請を承認する</summary>
    [HttpPost("Approve")]
    public async Task<IActionResult> ApproveAsync(ApproveHistoryApproveRequest request)
    {
        await approveHistoryApprover.ApproveAsync(request.ExpenseIds, request.Comment);
        return Ok();
    }

    /// <summary>経費精算の申請を否認する</summary>
    [HttpPost("Reject")]
    public async Task<IActionResult> RejectAsync(ApproveHistoryRejectRequest request)
    {
        await approveHistoryRejecter.RejectAsync(request.ExpenseIds, request.Comment);
        return Ok();
    }

    public record ApproveHistoryApplicationRequest(DateOnly Date);

    public record ApproveHistoryApproveRequest(string[] ExpenseIds, string? Comment);

    public record ApproveHistoryRejectRequest(string[] ExpenseIds, string? Comment);
}