using Microsoft.AspNetCore.Mvc;
using Query.Datas;
using Query.QueryServices;
using Usecase.ExpenseTypes;

namespace WebAPI.Controllers;

/// <summary>経費種別コントローラー</summary>
[ApiController]
[Route("api/[Controller]")]
public class ExpenseTypesController : ControllerBase
{
    private readonly ExpenseTypeQueryService expenseTypeQueryService;
    private readonly ExpenseTypeAdder expenseTypeAdder;
    private readonly ExpenseTypeUpdater expenseTypeUpdater;

    public ExpenseTypesController(ExpenseTypeQueryService expenseTypeQueryService, ExpenseTypeAdder expenseTypeAdder, ExpenseTypeUpdater expenseTypeUpdater)
    {
        this.expenseTypeQueryService = expenseTypeQueryService;
        this.expenseTypeAdder = expenseTypeAdder;
        this.expenseTypeUpdater = expenseTypeUpdater;
    }

    /// <summary>経費種別一覧を取得する</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExpenseTypeData>>> GetAllAsync()
    {
        return Ok(await expenseTypeQueryService.GetAllAsync());
    }

    /// <summary>経費種別を取得する</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseTypeData>> GetAsync(string id)
    {
        return Ok(await expenseTypeQueryService.GetAsync(id));
    }

    /// <summary>経費種別を追加する</summary>
    [HttpPost]
    public async Task<ActionResult<string>> AddAsync(ExpenseTypeAddRequest request)
    {
        return Ok(await expenseTypeAdder.AddAsync(request.Name, request.Details, request.IsReceipt));
    }

    /// <summary>経費種別を更新する</summary>
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, ExpenseTypeUpdateRequest request)
    {
        await expenseTypeUpdater.UpdateAsync(id, request.Name, request.Details, request.IsReceipt, request.IsActive);
        return Ok();
    }

    public record ExpenseTypeAddRequest(string Name, string? Details, bool IsReceipt);

    public record ExpenseTypeUpdateRequest(string Name, string? Details, bool IsReceipt, bool IsActive);
}