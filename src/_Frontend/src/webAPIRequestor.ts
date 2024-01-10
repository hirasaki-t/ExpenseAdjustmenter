import axios, { AxiosError, AxiosRequestConfig, AxiosRequestTransformer } from "axios";
import { formatToTimeZone } from "date-fns-timezone";
import { Authorizer } from "./authorizer";
import { AspErrorData } from "./aspErrorData";

export class WebAPIRequestor {
    /** 認証済みヘッダを取得する */
    private static async GetAuthenticatedConfig(scopes?: string[]): Promise<AxiosRequestConfig> {
        return { headers: { Authorization: `Bearer ${await Authorizer.GetAccessToken(scopes)}` } };
    }

    public static async ThrowIfRequestFailedAsync<T>(
        action: (config: AxiosRequestConfig) => Promise<T>,
        scopes?: string[]
    ) {
        try {
            const config = await WebAPIRequestor.GetAuthenticatedConfig(scopes);
            config.transformRequest = [dateTransformer].concat(
                axios.defaults.transformRequest as AxiosRequestTransformer[]
            );
            config.transformResponse = (data) => {
                if (typeof data === "string") {
                    try {
                        return JSON.parse(data, dateParseChallenge);
                    } catch (e) {
                        // ignore
                    }
                }
                return data;
            };
            return await action(config);
        } catch (ex) {
            const errorData = (ex as AxiosError<AspErrorData>).response?.data;
            throw new Error(errorData?.detail ?? `不明なエラーが発生しました。${(ex as Error).message}`);
        }
    }
}

const dateTransformer: AxiosRequestTransformer = (data) => {
    if (data instanceof Date) {
        return formatToTimeZone(data, "YYYY-MM-DDTHH:mm:ss+09:00", { timeZone: "Asia/Tokyo" });
    }
    if (Array.isArray(data)) {
        return data.map((val) => dateTransformer(val));
    }
    if (typeof data === "object" && !!data && !(data instanceof FormData)) {
        return Object.fromEntries(Object.entries(data).map(([key, val]) => [key, dateTransformer(val)]));
    }
    return data;
};

const regex = /[0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}:[0-9]{2}/;

// https://qiita.com/AshSuzuki/items/d1d870f951ee70ec47e5
function dateParseChallenge(_: string, value: unknown) {
    if (typeof value === "string" && regex.test(value)) {
        const time = Date.parse(value);
        if (!Number.isNaN(time)) {
            return new Date(time);
        }
    }
    return value;
}
