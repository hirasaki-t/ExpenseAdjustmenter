import axios from "axios";
import { defineStore } from "pinia";
import { computed, ref } from "vue";
import { toDictionary } from "../arrayExtensions";
import { ApproveHistory } from "../datas/approveHistory";
import { WebAPIRequestor } from "../webAPIRequestor";
import { useSundryExpenseStore } from "./sundryExpenseStore";
import { useTraveringExpenseStore } from "./traveringExpenseStore";
import { dateSorter } from "../sorters";
import { addMonths } from "date-fns";

export const useApproveHistoryStore = defineStore("approveHistory", () => { 
    const approveHistories = ref<ApproveHistory[]>([]);
    const orderedApproveHistories = computed(() => Array.from(approveHistories.value).sort((x, y) => dateSorter(x.date, y.date)));
    const dictionary = computed(() => toDictionary(approveHistories.value, x => x.id, x => x));
    const selectableMonths = ref<Date[]>([]);

    const traveringExpenseStore = useTraveringExpenseStore();
    const sundryExpenseStore = useSundryExpenseStore();
    
    async function initialize() {
        const thisMonth = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
        selectableMonths.value = await requestSelectableMonthGets();
        approveHistories.value = selectableMonths.value.some(x => x.getFullYear() === thisMonth.getFullYear() && x.getMonth() === thisMonth.getMonth()) ? 
            await requestGets(thisMonth.getFullYear(), thisMonth.getMonth() + 1) : await requestGets(addMonths(thisMonth, -1).getFullYear(), thisMonth.getMonth());
    }

    async function reload(year: number, month: number) {
        approveHistories.value = await requestGets(year, month);
    }

    async function application(date: Date){
        await requestApplication(date.getFullYear(), date.getMonth());
        traveringExpenseStore.initialize();
        sundryExpenseStore.initialize();
        initialize();
    }

    async function approve(expenseIds: string[], comment: string | null){
        await requestApprove(expenseIds, comment);
        initialize();
    }

    async function reject(expenseIds: string[], comment: string | null){
        await requestReject(expenseIds, comment);
        initialize();
    }

    return { approveHistories, orderedApproveHistories, dictionary, selectableMonths, initialize, reload, application, approve, reject };
});

async function requestSelectableMonthGets() {
    return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        return (await axios.get<Date[]>(`api/ApproveHistories/SelectableMonths`, config)).data;
    });
}

async function requestGets(year: number, month: number){
    return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        return (await axios.get<ApproveHistory[]>(`api/ApproveHistories/${year}/${month}`, config)).data;
    });
}

async function requestApplication(year: number, month: number){
    await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        await axios.post("api/ApproveHistories/Application", { date: new Date(year, month, 1) }, config);
    });
}

async function requestApprove(expenseIds: string[], comment: string | null){
    await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        await axios.post("api/ApproveHistories/Approve", { expenseIds, comment }, config);
    });
}

async function requestReject(expenseIds: string[], comment: string | null){
    await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        await axios.post("api/ApproveHistories/Reject", { expenseIds, comment }, config);
    });
}