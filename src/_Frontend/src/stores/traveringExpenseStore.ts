import axios from "axios";
import { defineStore } from "pinia";
import { computed, ref } from "vue";
import { toDictionary } from "../arrayExtensions";
import { TraveringListData } from "../datas/traveringListData";
import { dateSorter } from "../sorters";
import { WebAPIRequestor } from "../webAPIRequestor";
import { addMonths, format } from "date-fns";

export const useTraveringExpenseStore = defineStore("traveringExpense", () => {
    const traveringExpenses = ref<TraveringListData[]>([]);
    const selectableMonths = ref<Date[]>([]);

    const orderedTraveringExpenses = computed(() => Array.from(traveringExpenses.value).sort((x, y) => dateSorter(x.date, y.date)));
    const dictionary = computed(() => toDictionary(orderedTraveringExpenses.value, x => x.id, x => x));

    async function initialize() {
        traveringExpenses.value = await requestGets(new Date().getFullYear(), new Date().getMonth() + 1);
        selectableMonths.value = await requestSelectableMonthGets();
    }

    async function reload(year: number, month: number) {
        traveringExpenses.value = await requestGets(year, month);
    }
    
    async function add(
        date: Date, 
        workName: string, 
        startSection: string | null, 
        endSection: string | null, 
        categoryId: string, 
        submissionMethod: string | null, 
        receipt: File | null, 
        amount: number, 
        remarks: string | null){
            const id = await requestAdd(date, workName, startSection, endSection, categoryId, submissionMethod, receipt, amount, remarks);
            const traveringExpense = await requestGet(id);
            if (!traveringExpense) return;
            traveringExpenses.value = [...traveringExpenses.value, traveringExpense]
    }

    async function update(
        id: string,
        date: Date, 
        workName: string, 
        startSection: string | null, 
        endSection: string | null, 
        categoryId: string, 
        submissionMethod: string | null, 
        receipt: File | null, 
        amount: number, 
        remarks: string | null){
            await requestUpdate(id, date, workName, startSection, endSection, categoryId, submissionMethod, receipt, amount, remarks);
            const traveringExpense = await requestGet(id);
            if (!traveringExpense) throw new Error("対象の旅費・交通費経費精算が存在しません。");
            traveringExpenses.value = [...traveringExpenses.value.filter(x => x.id !== id), traveringExpense];
    }

    async function remove(id: string) {
        await requestDelete(id);
        traveringExpenses.value = traveringExpenses.value.filter((x) => x.id !== id);
    }

    return { traveringExpenses, selectableMonths, dictionary, orderedTraveringExpenses, initialize, reload, add, update, remove };
});

async function requestSelectableMonthGets() {
    const result = await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        return (await axios.get<Date[]>(`api/TraveringExpenses/SelectableMonths`, config)).data;
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
        return (await axios.get<TraveringListData[]>(`api/TraveringExpenses/${year}/${month}`, config)).data;
    });
}

async function requestGet(id: string) {
    return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        const result = (await axios.get<TraveringListData | undefined>(`api/TraveringExpenses/${id}`, config)).data;
        return !result ? undefined : result;
    });
}

async function requestAdd(date: Date, workName: string, startSection: string | null, endSection: string | null, categoryId: string, submissionMethod: string | null, receipt: File | null, amount: number, remarks: string | null) {
    const formData = new FormData();
    formData.append("date", format(date, 'yyyy/MM/dd'));
    formData.append('workName', workName);
    formData.append('startSection', startSection ?? "");
    formData.append('endSection', endSection ?? "");
    formData.append('categoryId', categoryId ?? "");
    formData.append('submissionMethod', submissionMethod ?? "");
    formData.append('file', receipt ?? "");
    formData.append("amount", amount.toString());
    formData.append('remarks', remarks ?? "");

    return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        return (await axios.post("api/TraveringExpenses", formData, config)).data as string;
    });
}

async function requestUpdate(id: string, date: Date, workName: string, startSection: string | null, endSection: string | null, categoryId: string, submissionMethod: string | null, receipt: File | null, amount: number, remarks: string | null) {
    const formData = new FormData();
    formData.append("date", format(date, 'yyyy/MM/dd'));
    formData.append('workName', workName);
    formData.append('startSection', startSection ?? "");
    formData.append('endSection', endSection ?? "");
    formData.append('categoryId', categoryId ?? "");
    formData.append('submissionMethod', submissionMethod ?? "");
    formData.append('file', receipt ?? "");
    formData.append("amount", amount.toString());
    formData.append('remarks', remarks ?? "");

    return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        return (await axios.patch(`api/TraveringExpenses/${id}`, formData, config)).data as string;
    });
}

async function requestDelete(id: string){
    return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        await axios.delete(`api/TraveringExpenses/${id}`, config);
    });
}