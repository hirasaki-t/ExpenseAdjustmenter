<template>
    <el-table :data="sundryExpenses" border show-summary :summary-method="getSummaries" height="100%">
        <el-table-column align="center" width="150">
            <template #header>
                <custom-filter title="日付" width="100px">
                    <el-select v-model="filter.date" clearable placeholder="日付">
                        <el-option 
                            v-for="date in dates"
                            :label="`${format(date, 'yyyy年MM月dd日')}`" 
                            :value="date"/>
                    </el-select>
                </custom-filter>
            </template>
            <template #default="scope">
                {{ `${format(scope.row.date, 'yyyy年MM月dd日')}` }}
            </template>
        </el-table-column>
        <el-table-column align="center" width="250">
            <template #header>
                <custom-filter title="経費種別" width="150px">
                    <expense-type-selector v-model="filter.expenseTypeId" clearable />
                </custom-filter>
            </template>
            <template #default="scope">
                {{ expenseTypeDictionary[scope.row.expenseTypeId].name }}
            </template>
        </el-table-column>
        <el-table-column label="詳細" property="details" align="center" />
        <el-table-column label="人数" header-align="center" align="right" width="100">
            <template #default="scope">
                {{ `${scope.row.participationNumber}人` }}
            </template>
        </el-table-column>
        <el-table-column label="金額" header-align="center" align="right" width="100">
            <template #default="scope">
                {{ `${(scope.row.amount as number).toLocaleString()}円` }}
            </template>
        </el-table-column>
        <el-table-column label="領収書" property="receipt" align="center" width="100">
            <template #default="scope">
                <receipt-label 
                    :is-receipt="expenseTypeDictionary[scope.row.expenseTypeId].isReceipt" 
                    :submission-method="scope.row.submissionMethod" 
                    :receipt-id="scope.row.receiptId" 
                    :file-name="`領収書_${format((scope.row.date as Date), 'yyyy年MM月dd日')}_${graphUser?.displayName.replace(' ', '')}_${scope.row.details}.pdf`"
                />
            </template>
        </el-table-column>
        <el-table-column align="center" width="100">
            <template #header>
                <custom-filter title="ステータス" width="100px">
                    <status-selector v-model="filter.status" clearable />
                </custom-filter>
            </template>
            <template #default="scope">
                <approve-label :status="scope.row.status" :comment="scope.row.comment" :user-id="scope.row.reviewerId" />
            </template>
        </el-table-column>
        <el-table-column width="140" align="center" fixed="right">
            <template #header>
                <add-button />
            </template>
            <template #default="scope">
                <stack-container>
                    <span>
                        <edit-button :id="scope.row.id" :status="scope.row.status" />
                    </span>
                    <span>
                        <delete-button :id="scope.row.id" :status="scope.row.status" />
                    </span>
                </stack-container>
            </template>
        </el-table-column>
    </el-table>
</template>

<script setup lang="ts">
import { ElTable, ElTableColumn, ElSelect, ElOption } from "element-plus";
import { computed, reactive, ref } from "vue";
import { useSundryExpenseStore } from "../../../stores/sundryExpenseStore";
import StackContainer from "../../atomos/StackContainer.vue";
import AddButton from "./AddButton.vue";
import EditButton from "./EditButton.vue";
import DeleteButton from "./DeleteButton.vue";
import { useExpenseTypeStore } from "../../../stores/expenseTypeStore";
import ReceiptLabel from "../ReceiptDownload.vue";
import ApproveLabel from "../../molecules/labels/ApproveLabel.vue";
import { SundryExpenseListData } from "../../../datas/sundryExpenseListData";
import { useAzureDirectoryStore } from "../../../stores/azureDirectoryStore";
import { distinct, sum } from "../../../arrayExtensions";
import { format } from "date-fns";
import CustomFilter from "../../molecules/filters/CustomFilter.vue";
import ExpenseTypeSelector from "../../molecules/selectors/ExpenseTypeSelector.vue";
import StatusSelector from "../../molecules/selectors/StatusSelector.vue";

const emit = defineEmits<{ (event: "update:modelValue", modelValiue: string[]): void }>();

const sundryExpenseStore = useSundryExpenseStore();
const sundryExpenses = computed(() => sundryExpenseStore.orderedSundryExoenses
    .filter(x => !filter.date || x.date.getDate() === filter.date.getDate())
    .filter(x => !filter.expenseTypeId || x.expenseTypeId === filter.expenseTypeId)
    .filter(x => !filter.status || x.status === filter.status));
const expenseTypeStore = useExpenseTypeStore();
const expenseTypeDictionary = expenseTypeStore.dictionary;
const azureDirectoryStore = useAzureDirectoryStore();
const graphUser = azureDirectoryStore.graphUser;
const dates = computed(() => distinct(sundryExpenseStore.sundryExoenses.map(x => x.date.getTime()).sort((x, y) => x - y))?.map(x => new Date((x as number))));

const filter = reactive<{
    date?: Date,
    expenseTypeId?: string,
    status?: string,
}>({
});

function getSummaries(){
    const amountSummary = sum(sundryExpenses.value.map(x => x.amount));
    return ["", "", "", "", "", "", "合計金額", `${amountSummary.toLocaleString()}円`];
}

</script>

<style>
td > div.cell, th > div.cell {
    white-space:nowrap !important;
}
</style>