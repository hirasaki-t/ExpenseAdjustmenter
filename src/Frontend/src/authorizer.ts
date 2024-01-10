import { PublicClientApplication } from "@azure/msal-browser";
import axios from "axios";
import { WebAPIRequestor } from "./webAPIRequestor";

/** Azureの認証クラス */
export class Authorizer {
    private static readonly clientId = "";
    private static readonly authority = "";
    private static readonly scopes = [
        "user.read",
        "user.readbasic.all",
    ];
    private static isAdmin = false;

    public static readonly msal = new PublicClientApplication({
        auth: {
            clientId: this.clientId,
            authority: this.authority,
        },
        cache: {
            cacheLocation: "localStorage",
            storeAuthStateInCookie: false,
        },
    });

    /** アプリケーションにサインインする */
    static async SignInAsync() {
        const response = await this.msal.handleRedirectPromise();
        if (response?.account?.username !== undefined) {
            this.msal.setActiveAccount(response.account);
            this.isAdmin = await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
                return (await axios.get("api/Users/IsAdmin", config)).data;
            });
            return true;
        }

        try {
            await this.msal.loginRedirect({ scopes: this.scopes, redirectUri: new URL(window.location.href).origin });
        } catch (ex) {
            return false;
        }
    }

    /** アクセストークンを取得する */
    static async GetAccessToken(scopes?: string[]) {
        scopes = scopes ?? Authorizer.scopes;
        const accounts = this.msal.getAllAccounts();
        if (accounts.length < 0) throw new Error("未認証です");
        const response = await this.msal.acquireTokenSilent({
            scopes: scopes,
            account: accounts[0],
            forceRefresh: false,
        });
        return response.accessToken;
    }

    static GetIsAdmin() {
        return this.isAdmin;
    }
}
