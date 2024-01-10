import axios from "axios";
import { addMonths, format } from "date-fns";
import { defineStore } from "pinia";
import { computed, ref } from "vue";
import { toDictionary } from "../arrayExtensions";
import { SundryExpenseListData } from "../datas/sundryExpenseListData";
import { dateSorter } from "../sorters";
import { WebAPIRequestor } from "../webAPIRequestor";

export const useSundryExpenseStore = defineStore("sundryExpense", () => {
    const sundryExoenses = ref<SundryExpenseListData[]>([]);
    const selectableMonths = ref<Date[]>([]);

    const orderedSundryExoenses = computed(() => Array.from(sundryExoenses.value).sort((x, y) => dateSorter(x.date, y.date)));
    const dictionary = computed(() => toDictionary(orderedSundryExoenses.value, x => x.id, x => x));

    async function initialize() {
        sundryExoenses.value = await requestGets(new Date().getFullYear(), new Date().getMonth() + 1);
        selectableMonths.value = await requestSelectableMonthGets();
    }

    async function reload(year: number, month: number) {
        sundryExoenses.value = await requestGets(year, month);
    }
    

    async function add(
        date: Date, 
        expenseTypeId: string, 
        details: string | null, 
        participationNumber: number, 
        amount: number, 
        submissionMethod: string | null, 
        receipt: File | null){
            const id = await requestAdd(date, expenseTypeId, details, participationNumber, amount, submissionMethod, receipt);
            const sundryExoense = await requestGet(id);
            if (!sundryExoense) return;
            sundryExoenses.value = [...sundryExoenses.value, sundryExoense]
    }

    async function update(
        id: string,
        date: Date, 
        expenseTypeId: string, 
        details: string | null, 
        participationNumber: number, 
        amount: number, 
        submissionMethod: string | null, 
        receipt: File | null){
            await requestUpdate(id, date, expenseTypeId, details, participationNumber, amount, submissionMethod, receipt);
            const sundryExoense = await requestGet(id);
            if (!sundryExoense) throw new Error("対象の諸経費精算が存在しません。");
            sundryExoenses.value = [...sundryExoenses.value.filter(x => x.id !== id), sundryExoense];
    }

    async function remove(id: string) {
        await requestDelete(id);
        sundryExoenses.value = sundryExoenses.value.filter((x) => x.id !== id);
    }

    return { sundryExoenses, selectableMonths, dictionary, orderedSundryExoenses, initialize, reload, add, update, remove };
});

async function requestSelectableMonthGets() {
    const result = await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        return (await axios.get<Date[]>(`api/SundryExpenses/SelectableMonths`, config)).data;
    });

    const thisMonth = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
    if (result.length < 2) return [thisMonth, addMonths(thisMonth, -1)];
    if (result.some(x => x.getFullYear() === thisMonth.getFullYear() && x.getMonth() === thisMonth.getMonth())){
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

async function requestAdd(date: Date, expenseTypeId: string, details: string | null, participationNumber: number, amount: number, submissionMethod: string | null, receipt: File | null) {
    const formData = new FormData();
    formData.append("date", format(date, 'yyyy/MM/dd'));
    formData.append('expenseTypeId', expenseTypeId);
    formData.append('details', details ?? "");
    formData.append('participationNumber', participationNumber.toString());
    formData.append("amount", amount.toString());
    formData.append('submissionMethod', submissionMethod ?? "");
    formData.append('file', receipt ?? "");

    return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        return (await axios.post("api/SundryExpenses", formData, config)).data as string;
    });
}

async function requestUpdate(id: string, date: Date, expenseTypeId: string, details: string | null, participationNumber: number, amount: number, submissionMethod: string | null, receipt: File | null) {
    const formData = new FormData();
    formData.append("date", format(date, 'yyyy/MM/dd'));
    formData.append('expenseTypeId', expenseTypeId);
    formData.append('details', details ? details : "");
    formData.append('participationNumber', participationNumber.toString());
    formData.append("amount", amount.toString());
    formData.append('submissionMethod', submissionMethod ? submissionMethod : "");
    formData.append('file', receipt ?? "");

    return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        return (await axios.patch(`api/SundryExpenses/${id}`, formData, config)).data as string;
    });
}

async function requestDelete(id: string){
    return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        await axios.delete(`api/SundryExpenses/${id}`, config);
    });
}