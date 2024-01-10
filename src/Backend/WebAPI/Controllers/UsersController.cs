using System;
using Microsoft.AspNetCore.Mvc;
using Query.Datas;
using Query.QueryServices;
using Usecase.Users;

namespace WebAPI.Controllers;

/// <summary>ユーザーコントローラー</summary>
[ApiController]
[Route("api/[Controller]")]
public class UsersController : ControllerBase
{
    private readonly UserQueryService userQueryService;
    private readonly UserAdminChecker userAdminChecker;
    private readonly UserAdder userAdder;
    private readonly UserUpdater userUpdater;

    public UsersController(UserQueryService userQueryService, UserAdminChecker userAdminChecker, UserAdder userAdder, UserUpdater userUpdater)
    {
        this.userQueryService = userQueryService;
        this.userAdminChecker = userAdminChecker;
        this.userAdder = userAdder;
        this.userUpdater = userUpdater;
    }

    /// <summary>ユーザー一覧を取得する</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserData>>> GetAllAsync()
    {
        return Ok(await userQueryService.GetAllAsync());
    }

    /// <summary>ユーザーを取得する</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserData>> GetAsync(string id)
    {
        return Ok(await userQueryService.GetAsync(id));
    }

    /// <summary>ログインユーザーが管理者か判定する</summary>
    [HttpGet("IsAdmin")]
    public async Task<ActionResult<bool>> GetIsAdminAsync()
    {
        return Ok(await userAdminChecker.CheckAsync());
    }

    /// <summary>ユーザーを追加する</summary>
    [HttpPost]
    public async Task<ActionResult<string>> AddAsync(UserAddRequest request)
    {
        return Ok(await userAdder.AddAsync(request.Mail, request.IsAdmin));
    }

    /// <summary>ユーザーを更新する</summary>
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, UserUpdateRequest request)
    {
        await userUpdater.UpdateAsync(id, request.IsAdmin, request.IsActive);
        return Ok();
    }

    public record UserAddRequest(string Mail, bool IsAdmin);

    public record UserUpdateRequest(bool IsAdmin, bool IsActive);
}