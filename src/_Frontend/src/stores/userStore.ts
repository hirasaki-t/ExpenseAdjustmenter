import axios from "axios";
import { defineStore } from "pinia";
import { computed, ref } from "vue";
import { toDictionary } from "../arrayExtensions";
import { User } from "../datas/user";
import { idSorter } from "../sorters";
import { WebAPIRequestor } from "../webAPIRequestor";

export const useUserStore = defineStore("user", () => {
    const users = ref<User[]>([]);

    const orderedUsers = computed(() => Array.from(users.value).sort(idSorter));
    const dictionary = computed(() => toDictionary(orderedUsers.value, x => x.id, x => x));

    async function initialize() {
        users.value = await requestGetAll();
    }

    async function add(mail: string, isAdmin: boolean){
        const id = await requestAdd(mail, isAdmin);
        const user = await requestGet(id);
        if (!user) return;
        users.value = [...users.value, user]
    }

    async function update(id: string, isAdmin: boolean, isActive: boolean){
        await requestUpdate(id, isAdmin, isActive);
        const user = await requestGet(id);
        if (!user) throw new Error("対象のユーザーが存在しません。");
        users.value = [...users.value.filter((x) => x.id !== id), user];
    }

    return { users, orderedUsers, dictionary, initialize, add, update };
});

async function requestGetAll() {
    return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        return (await axios.get<User[]>("api/Users", config)).data;
    });
}

async function requestGet(id: string) {
    return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        const result = (await axios.get<User | undefined>(`api/Users/${id}`, config)).data;
        return !result ? undefined : result;
    });
}

async function requestAdd(mail: string, isAdmin: boolean) {
    return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        return (await axios.post("api/Users", { mail, isAdmin }, config)).data as string;
    });
}

async function requestUpdate(id: string, isAdmin: boolean, isActive: boolean) {
    return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        await axios.patch(`api/Users/${id}`, { isAdmin, isActive }, config);
    });
}