using Microsoft.AspNetCore.Mvc;
using Usecase.Receipts;

namespace WebAPI.Controllers;

/// <summary>領収書コントローラー</summary>
[ApiController]
[Route("api/[Controller]")]
public class ReceiptsController : ControllerBase
{
    private readonly ReceiptDownloader receiptDownloader;
    private readonly ReceiptZipDownloader receiptZipDownloader;

    public ReceiptsController(ReceiptDownloader receiptDownloader, ReceiptZipDownloader receiptZipDownloader)
    {
        this.receiptDownloader = receiptDownloader;
        this.receiptZipDownloader = receiptZipDownloader;
    }

    /// <summary>領収書を取得する</summary>
    [HttpGet("{receiptId}")]
    public async Task<IActionResult> GetAsync(string receiptId)
    {
        using var memoryStream = new MemoryStream();
        var (stream, fileName) = await receiptDownloader.DownloadAsync(receiptId);
        stream.CopyTo(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return File(memoryStream.ToArray(), "application/pdf", fileName);
    }

    /// <summary>領収書をZipファイルで一括ダウンロードする</summary>
    [HttpPost]
    public async Task<IActionResult> DownloadAsync(ReceiptZipDownloadRequest request)
    {
        var bytes = await receiptZipDownloader.DownloadAsync(request.ExpenseIds);
        return File(bytes, "application/zip", "領収書.zip");
    }

    public record ReceiptZipDownloadRequest(string[] ExpenseIds);
}