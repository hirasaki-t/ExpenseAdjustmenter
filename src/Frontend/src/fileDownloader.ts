import axios from "axios";
import { WebAPIRequestor } from "./webAPIRequestor";

/** ファイルタイプ */
export type FileTypes = "CSV";

/** エンコードの種類 */
export type Encodings = "UTF-8 BOM";

/** ファイルのダウンローダー */
export class FileDownloader {
    private static readonly bom = new Uint8Array([0xef, 0xbb, 0xbf]);
    private readonly fileType: FileTypes;
    private readonly encoding: Encodings;

    constructor(fileType: FileTypes, encoding: Encodings) {
        this.fileType = fileType;
        this.encoding = encoding;
    }

    /** ダウンロードを実行する */
    public async DownloadAsync(url: string, fileName: string) {
        WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
            const response = await axios.get(url, config);
            this.CreateAndDownloadLocalFile(response.data, fileName);
        });
    }

    /** ローカルでファイルを生成し、ダウンロードする */
    public CreateAndDownloadLocalFile(fileData: string, fileName: string) {
        const data = this.ToBlob(fileData);
        const objectURL = URL.createObjectURL(data);
        try {
            const downloadElement = document.createElement("a");
            downloadElement.href = objectURL;
            downloadElement.download = fileName;
            downloadElement.click();
        } finally {
            URL.revokeObjectURL(objectURL);
        }
    }

    private ToBlob(data: string) {
        switch (this.encoding) {
            case "UTF-8 BOM":
                return new Blob([FileDownloader.bom, data], { type: this.GetType() });
            default:
                throw new Error("未登録の文字コードです");
        }
    }

    private GetType() {
        switch (this.fileType) {
            case "CSV":
                return "text/csv";
            default:
                throw new Error("未登録のファイルタイプです。");
        }
    }
}