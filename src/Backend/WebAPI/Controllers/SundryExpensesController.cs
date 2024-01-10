using System.Text.RegularExpressions;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Query.Datas;
using Query.QueryServices;
using Usecase.SundryExpenses;

namespace WebAPI.Controllers;

/// <summary>諸経費コントローラー</summary>
[ApiController]
[Route("api/[Controller]")]
public class SundryExpensesController : ControllerBase
{
    private readonly SundryExpenseQueryService sundryExpenseQueryService;
    private readonly SundryExpenseAdder sundryExpenseAdder;
    private readonly SundryExpenseUpdater sundryExpenseUpdater;
    private readonly SundryExpenseDeleter sundryExpenseDeleter;
    private readonly GraphServiceClient graphServiceClient;

    public SundryExpensesController(
        SundryExpenseQueryService sundryExpenseQueryService,
        SundryExpenseAdder sundryExpenseAdder,
        SundryExpenseUpdater sundryExpenseUpdater,
        SundryExpenseDeleter sundryExpenseDeleter,
        GraphServiceClient graphServiceClient)
    {
        this.sundryExpenseQueryService = sundryExpenseQueryService;
        this.sundryExpenseAdder = sundryExpenseAdder;
        this.sundryExpenseUpdater = sundryExpenseUpdater;
        this.sundryExpenseDeleter = sundryExpenseDeleter;
        this.graphServiceClient = graphServiceClient;
    }

    /// <summary>諸経費の選択可能な年月一覧を取得する</summary>
    [HttpGet("SelectableMonths")]
    public async Task<ActionResult<IEnumerable<DateOnly>>> GetAsync()
    {
        var mail = (await graphServiceClient.Me.Request().Select(x => x.Mail).GetAsync()).Mail;
        return Ok(await sundryExpenseQueryService.GetSelectableMonthsAsync(mail));
    }

    /// <summary>諸経費一覧を取得する</summary>
    [HttpGet("{year}/{month}")]
    public async Task<ActionResult<IEnumerable<TraveringExpenseData>>> GetAsync(int year, int month)
    {
        var mail = (await graphServiceClient.Me.Request().Select(x => x.Mail).GetAsync()).Mail;
        return Ok(await sundryExpenseQueryService.GetMonthDatasAsync(new DateOnly(year, month, 1), mail));
    }

    /// <summary>諸経費を取得する</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TraveringExpenseData?>> GetAsync(string id)
    {
        var mail = (await graphServiceClient.Me.Request().Select(x => x.Mail).GetAsync()).Mail;
        return Ok(await sundryExpenseQueryService.GetAsync(id, mail));
    }

    /// <summary>諸経費を追加する</summary>
    [HttpPost]
    public async Task<ActionResult<string>> AddAsync([FromForm]SundryExpenseAddRequest request)
    {
        using var stream = request.File?.OpenReadStream();
        if (!DateOnly.TryParse(request.Date, out var targetDate)) throw new DomainException("日付の値が不正です。");

        return Ok(await sundryExpenseAdder.AddAsync(targetDate, request.ExpenseTypeId, request.Details, request.ParticipationNumber, request.Amount, request.SubmissionMethod, stream));
    }

    /// <summary>諸経費を更新する</summary>
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromForm]SundryExpenseUpdateRequest request)
    {
        using var stream = request.File?.OpenReadStream();
        if (!DateOnly.TryParse(request.Date, out var targetDate)) throw new DomainException("日付の値が不正です。");

        await sundryExpenseUpdater.UpdateAsync(id, targetDate, request.ExpenseTypeId, request.Details, request.ParticipationNumber, request.Amount, request.SubmissionMethod, stream);
        return Ok();
    }

    /// <summary>諸経費を削除する</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        await sundryExpenseDeleter.DeleteAsync(id);
        return Ok();
    }

    public record SundryExpenseAddRequest(string Date, string ExpenseTypeId, string? Details, int ParticipationNumber, int Amount, string? SubmissionMethod, IFormFile? File);

    public record SundryExpenseUpdateRequest(string Date, string ExpenseTypeId, string? Details, int ParticipationNumber, int Amount, string? SubmissionMethod, IFormFile? File);
}