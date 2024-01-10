import axios from "axios";
import { addMonths, format } from "date-fns";
import { defineStore } from "pinia";
import { computed, ref } from "vue";
import { toDictionary } from "../arrayExtensions";
import { dateSorter } from "../sorters";
import { WebAPIRequestor } from "../webAPIRequestor";
import type { SundryExpense, SundryExpenseListData } from "@/types";

export const useSundryExpenseStore = defineStore("sundryExpense", () => {
  const sundryExoenses = ref<SundryExpenseListData[]>([]);
  const selectableMonths = ref<Date[]>([]);

  const orderedSundryExoenses = computed(() =>
    Array.from(sundryExoenses.value).sort((x, y) => dateSorter(x.date, y.date))
  );
  const dictionary = computed(() =>
    toDictionary(
      orderedSundryExoenses.value,
      (x) => x.id,
      (x) => x
    )
  );

  async function initialize() {
    sundryExoenses.value = await requestGets(new Date().getFullYear(), new Date().getMonth() + 1);
    selectableMonths.value = await requestSelectableMonthGets();
  }

  async function reload(year: number, month: number) {
    sundryExoenses.value = await requestGets(year, month);
  }

  async function add(sundryExpense: Omit<SundryExpense, "id">) {
    const formData = new FormData();
    formData.append("date", format(sundryExpense.date, "yyyy/MM/dd"));
    formData.append("expenseTypeId", sundryExpense.expenseTypeId);
    formData.append("details", sundryExpense.details ?? "");
    formData.append("participationNumber", sundryExpense.participationNumber.toString());
    formData.append("amount", sundryExpense.amount.toString());
    formData.append("submissionMethod", sundryExpense.submissionMethod ?? "");
    formData.append("file", sundryExpense.receipt ?? "");

    const id = await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
      return (await axios.post("api/SundryExpenses", formData, config)).data as string;
    });

    const sundryExoense = await requestGet(id);
    if (!sundryExoense) return;
    sundryExoenses.value = [...sundryExoenses.value, sundryExoense];
  }

  async function update(sundryExpense: SundryExpense) {
    const formData = new FormData();
    formData.append("date", format(sundryExpense.date, "yyyy/MM/dd"));
    formData.append("expenseTypeId", sundryExpense.expenseTypeId);
    formData.append("details", sundryExpense.details ? sundryExpense.details : "");
    formData.append("participationNumber", sundryExpense.participationNumber.toString());
    formData.append("amount", sundryExpense.amount.toString());
    formData.append("submissionMethod", sundryExpense.submissionMethod ? sundryExpense.submissionMethod : "");
    formData.append("file", sundryExpense.receipt ?? "");

    await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
      return (await axios.patch(`api/SundryExpenses/${sundryExpense.id}`, formData, config)).data as string;
    });

    const sundryExoense = await requestGet(sundryExpense.id);
    if (!sundryExoense) throw new Error("対象の諸経費精算が存在しません。");
    sundryExoenses.value = [...sundryExoenses.value.filter((x) => x.id !== sundryExpense.id), sundryExoense];
  }

  async function remove(id: string) {
    await requestDelete(id);
    sundryExoenses.value = sundryExoenses.value.filter((x) => x.id !== id);
  }

  return {
    sundryExoenses,
    selectableMonths,
    dictionary,
    orderedSundryExoenses,
    initialize,
    reload,
    add,
    update,
    remove,
  };
});

async function requestSelectableMonthGets() {
  const result = await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
    return (await axios.get<Date[]>(`api/SundryExpenses/SelectableMonths`, config)).data;
  });

  const thisMonth = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
  if (result.length < 2) return [thisMonth, addMonths(thisMonth, -1)];
  if (result.some((x) => x.getFullYear() === thisMonth.getFullYear() && x.getMonth() === thisMonth.getMonth())) {
    return result;
  } else {
    return [...result, thisMonth];
  }
}

async function requestGets(year: number, month: number) {
  return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
    return (await axios.get<SundryExpenseListData[]>(`api/SundryExpenses/${year}/${month}`, config)).data;
  });
}

async function requestGet(id: string) {
  return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
    const result = (await axios.get<SundryExpenseListData | undefined>(`api/SundryExpenses/${id}`, config)).data;
    return !result ? undefined : result;
  });
}

async function requestDelete(id: string) {
  return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
    await axios.delete(`api/SundryExpenses/${id}`, config);
  });
}
