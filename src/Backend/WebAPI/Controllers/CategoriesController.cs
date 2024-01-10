using Microsoft.AspNetCore.Mvc;
using Query.Datas;
using Query.QueryServices;
using Usecase.Categories;

namespace WebAPI.Controllers;

/// <summary>交通区分コントローラー</summary>
[ApiController]
[Route("api/[Controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CategoryQueryService categoryQueryService;
    private readonly CategoryAdder categoryAdder;
    private readonly CategoryUpdater categoryUpdater;

    public CategoriesController(CategoryQueryService categoryQueryService, CategoryAdder categoryAdder, CategoryUpdater categoryUpdater)
    {
        this.categoryQueryService = categoryQueryService;
        this.categoryAdder = categoryAdder;
        this.categoryUpdater = categoryUpdater;
    }

    /// <summary>交通区分一覧を取得する</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryData>>> GetAllAsync()
    {
        return Ok(await categoryQueryService.GetAllAsync());
    }

    /// <summary>交通区分を取得する</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<CategoryData>>> GetAsync(string id)
    {
        return Ok(await categoryQueryService.GetAsync(id));
    }

    /// <summary>交通区分を追加する</summary>
    [HttpPost]
    public async Task<ActionResult<string>> AddAsync(CategoryAddRequest request)
    {
        return Ok(await categoryAdder.AddAsync(request.Name, request.Details, request.IsReceipt));
    }

    /// <summary>交通区分を更新する</summary>
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, CategoryUpdateRequest request)
    {
        await categoryUpdater.UpdateAsync(id, request.Name, request.Details, request.IsReceipt, request.IsActive);
        return Ok();
    }

    public record CategoryAddRequest(string Name, string? Details, bool IsReceipt);

    public record CategoryUpdateRequest(string Name, string? Details, bool IsReceipt, bool IsActive);
}