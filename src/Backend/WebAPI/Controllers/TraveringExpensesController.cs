using System.Text.RegularExpressions;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Query.Datas;
using Query.QueryServices;
using Usecase.TraveringExpenses;

namespace WebAPI.Controllers;

/// <summary>旅費・交通費コントローラー</summary>
[ApiController]
[Route("api/[Controller]")]
public class TraveringExpensesController : ControllerBase
{
    private readonly TraveringExpenseQueryService traveringExpenseQueryService;
    private readonly TraveringExpenseAdder traveringExpenseAdder;
    private readonly TraveringExpenseUpdater traveringExpenseUpdater;
    private readonly TraveringExpenseDeleter traveringExpenseDeleter;
    private readonly GraphServiceClient graphServiceClient;

    public TraveringExpensesController(
        TraveringExpenseQueryService traveringExpenseQueryService,
        TraveringExpenseAdder traveringExpenseAdder,
        TraveringExpenseUpdater traveringExpenseUpdater,
        TraveringExpenseDeleter traveringExpenseDeleter,
        GraphServiceClient graphServiceClient)
    {
        this.traveringExpenseQueryService = traveringExpenseQueryService;
        this.traveringExpenseAdder = traveringExpenseAdder;
        this.traveringExpenseUpdater = traveringExpenseUpdater;
        this.traveringExpenseDeleter = traveringExpenseDeleter;
        this.graphServiceClient = graphServiceClient;
    }

    /// <summary>旅費・交通費の申請がある年月一覧を取得する</summary>
    [HttpGet("SelectableMonths")]
    public async Task<ActionResult<IEnumerable<DateOnly>>> GetAsync()
    {
        var mail = (await graphServiceClient.Me.Request().Select(x => x.Mail).GetAsync()).Mail;
        return Ok(await traveringExpenseQueryService.GetSelectableMonthsAsync(mail));
    }

    /// <summary>旅費・交通費一覧を取得する</summary>
    [HttpGet("{year}/{month}")]
    public async Task<ActionResult<IEnumerable<TraveringExpenseData>>> GetAsync(int year, int month)
    {
        var mail = (await graphServiceClient.Me.Request().Select(x => x.Mail).GetAsync()).Mail;
        return Ok(await traveringExpenseQueryService.GetMonthDatasAsync(new DateOnly(year, month, 1), mail));
    }

    /// <summary>旅費・交通費を取得する</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TraveringExpenseData?>> GetAsync(string id)
    {
        var mail = (await graphServiceClient.Me.Request().Select(x => x.Mail).GetAsync()).Mail;
        return Ok(await traveringExpenseQueryService.GetAsync(id, mail));
    }

    /// <summary>旅費・交通費を追加する</summary>
    [HttpPost]
    public async Task<ActionResult<string>> AddAsync([FromForm]TraveringExpenseAddRequest request)
    {
        using var stream = request.File?.OpenReadStream();
        if (!DateOnly.TryParse(request.Date, out var targetDate)) throw new DomainException("日付の値が不正です。");

        return Ok(await traveringExpenseAdder.AddAsync(
            targetDate,
            request.WorkName,
            request.StartSection,
            request.EndSection,
            request.CategoryId,
            request.SubmissionMethod,
            request.File == null ? null : stream,
            int.Parse(request.Amount),
            request.Remarks
        ));
    }

    /// <summary>旅費・交通費を更新する</summary>
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromForm]TraveringExpenseUpdateRequest request)
    {
        using var stream = request.File?.OpenReadStream();
        if (!DateOnly.TryParse(request.Date, out var targetDate)) throw new DomainException("日付の値が不正です。");

        await traveringExpenseUpdater.UpdateAsync(
            id,
            targetDate,
            request.WorkName,
            request.StartSection,
            request.EndSection,
            request.CategoryId,
            request.SubmissionMethod,
            stream == null ? null : stream,
            request.Amount,
            request.Remarks);

        return Ok();
    }

    /// <summary>旅費・交通費を削除する</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        await traveringExpenseDeleter.DeleteAsync(id);
        return Ok();
    }

    public record TraveringExpenseAddRequest(string Date, string WorkName, string? StartSection, string? EndSection, string CategoryId, string? SubmissionMethod, IFormFile? File, string Amount, string? Remarks);

    public record TraveringExpenseUpdateRequest(string Date, string WorkName, string? StartSection, string? EndSection, string CategoryId, string? SubmissionMethod, IFormFile? File, int Amount, string? Remarks);
}