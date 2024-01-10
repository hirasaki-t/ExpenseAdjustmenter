import axios from "axios";
import { defineStore } from "pinia";
import { computed, ref } from "vue";
import { toDictionary } from "../arrayExtensions";
import { idSorter } from "../sorters";
import { WebAPIRequestor } from "../webAPIRequestor";
import type { TrafficCategory } from "@/types";

export const useTrafficCategoryStore = defineStore("category", () => {
  const trafficCategories = ref<TrafficCategory[]>([]);

  const orderedTrafficCategories = computed(() => Array.from(trafficCategories.value).sort(idSorter));
  const dictionary = computed(() =>
    toDictionary(
      orderedTrafficCategories.value,
      (x) => x.id,
      (x) => x
    )
  );

  async function initialize() {
    trafficCategories.value = await requestGetAll();
  }

  async function add(name: string, details: string | null, isReceipt: boolean) {
    const id = await requestAdd(name, details, isReceipt);
    const category = await requestGet(id);
    if (!category) return;
    trafficCategories.value = [...trafficCategories.value, category];
  }

  async function update(id: string, name: string, details: string | null, isReceipt: boolean, isActive: boolean) {
    await requestUpdate(id, name, details, isReceipt, isActive);
    const category = await requestGet(id);
    if (!category) throw new Error("対象の交通区分が存在しません。");
    trafficCategories.value = [...trafficCategories.value.filter((x) => x.id !== id), category];
  }

  return { trafficCategories, orderedTrafficCategories, dictionary, initialize, add, update };
});

async function requestGetAll() {
  return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
    return (await axios.get<TrafficCategory[]>("api/Categories", config)).data;
  });
}

async function requestGet(id: string) {
  return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
    const result = (await axios.get<TrafficCategory | undefined>(`api/Categories/${id}`, config)).data;
    return !result ? undefined : result;
  });
}

async function requestAdd(name: string, details: string | null, isReceipt: boolean) {
  return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
    return (await axios.post("api/Categories", { name, details, isReceipt }, config)).data as string;
  });
}

async function requestUpdate(id: string, name: string, details: string | null, isReceipt: boolean, isActive: boolean) {
  return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
    await axios.patch(`api/Categories/${id}`, { name, details, isReceipt, isActive }, config);
  });
}
