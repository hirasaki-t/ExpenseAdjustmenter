import axios from "axios";
import { defineStore } from "pinia";
import { computed, ref } from "vue";
import { toDictionary } from "../arrayExtensions";
import { idSorter } from "../sorters";
import { WebAPIRequestor } from "../webAPIRequestor";
import type { ExpenseType } from "@/types";

export const useExpenseTypeStore = defineStore("expenseType", () => {
  const expenseTypes = ref<ExpenseType[]>([]);

  const orderedExpenseTypes = computed(() => Array.from(expenseTypes.value).sort(idSorter));
  const dictionary = computed(() =>
    toDictionary(
      orderedExpenseTypes.value,
      (x) => x.id,
      (x) => x
    )
  );

  async function initialize() {
    expenseTypes.value = await requestGetAll();
  }

  async function add(name: string, details: string | null, isReceipt: boolean) {
    const id = await requestAdd(name, details, isReceipt);
    const expenseType = await requestGet(id);
    if (!expenseType) return;
    expenseTypes.value = [...expenseTypes.value, expenseType];
  }

  async function update(id: string, name: string, details: string | null, isReceipt: boolean, isActive: boolean) {
    await requestUpdate(id, name, details, isReceipt, isActive);
    const expenseType = await requestGet(id);
    if (!expenseType) throw new Error("対象の経費種別が存在しません。");
    expenseTypes.value = [...expenseTypes.value.filter((x) => x.id !== id), expenseType];
  }

  return { expenseTypes, orderedExpenseTypes, dictionary, initialize, add, update };
});

async function requestGetAll() {
  return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
    return (await axios.get<ExpenseType[]>("api/ExpenseTypes", config)).data;
  });
}

async function requestGet(id: string) {
  return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
    const result = (await axios.get<ExpenseType | undefined>(`api/ExpenseTypes/${id}`, config)).data;
    return !result ? undefined : result;
  });
}

async function requestAdd(name: string, details: string | null, isReceipt: boolean) {
  return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
    return (await axios.post("api/ExpenseTypes", { name, details, isReceipt }, config)).data as string;
  });
}

async function requestUpdate(id: string, name: string, details: string | null, isReceipt: boolean, isActive: boolean) {
  return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
    await axios.patch(`api/ExpenseTypes/${id}`, { name, details, isReceipt, isActive }, config);
  });
}
