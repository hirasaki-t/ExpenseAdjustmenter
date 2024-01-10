import axios from "axios";
import { defineStore } from "pinia";
import { ref } from "vue";
import { WebAPIRequestor } from "../webAPIRequestor";

export const useSystemSettingStore = defineStore("systemSetting", () => {
    const deadline = ref<Date>();

    async function initialize() {
        deadline.value = await requestGetDeadline();
    }

    return { deadline, initialize };
});

async function requestGetDeadline() {
    return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        return (await axios.get<Date>("api/SystemSettings/Deadline", config)).data;
    });
}