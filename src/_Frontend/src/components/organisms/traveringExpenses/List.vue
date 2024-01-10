<template>
    <el-table :data="traveringExpenses" border show-summary :summary-method="getSummaries" @selection-change="selection" height="100%">
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
        <el-table-column label="区間" align="center" width="250">
            <template #default="scope">
                <div v-if="scope.row.startSection || scope.row.endSection">
                    {{ scope.row.startSection }} → {{ scope.row.endSection }}
                </div>
            </template>
        </el-table-column>
        <el-table-column align="center" width="150">
            <template #header>
                <custom-filter title="交通区分" width="100px">
                    <category-selector v-model="filter.categoryId" clearable />
                </custom-filter>
            </template>
            <template #default="scope">
                {{ categoryDictionary[scope.row.categoryId].name }}
            </template>
        </el-table-column>
        <el-table-column align="center" width="150">
            <template #header>
                <custom-filter title="業務名" width="100px">
                    <work-name-selector v-model="filter.workName" clearable />
                </custom-filter>
            </template>
            <template #default="scope">
                {{ scope.row.workName }}
            </template>
        </el-table-column>
        <el-table-column label="摘要" property="remarks" align="center" />
        <el-table-column label="金額" header-align="center" align="right" width="100">
            <template #default="scope">
                {{ `${(scope.row.amount as number).toLocaleString()}円` }}
            </template>
        </el-table-column>
        <el-table-column label="領収書" align="center" width="100">
            <template #default="scope">
                <receipt-label 
                    :is-receipt="categoryDictionary[scope.row.categoryId].isReceipt" 
                    :submission-method="scope.row.submissionMethod" 
                    :receipt-id="scope.row.receiptId" 
                    :file-name="`領収書_${format((scope.row.date as Date), 'yyyy年MM月dd日')}_${graphUser?.displayName.replace(' ', '')}${scope.row.remarks ? '_' + scope.row.remarks : ''}.pdf`"
                />
            </template>
        </el-table-column>
        <el-table-column align="center" width="100">
            <template #header>
                <custom-filter title="ステータス" width="80px">
                    <status-selector v-model="filter.status" clearable />
                </custom-filter>
            </template>
            <template #default="scope">
                <approve-label :user-id="scope.row.reviewerId" :comment="scope.row.comment" :status="scope.row.status" />
            </template>
        </el-table-column>
        <el-table-column width="140" align="center" fixed="right">
            <template #header>
                <add-button :disable="props.disable"/>
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
import { computed, reactive } from "vue";
import StackContainer from "../../atomos/StackContainer.vue";
import AddButton from "./AddButton.vue";
import DeleteButton from "./DeleteButton.vue";
import ReceiptLabel from "../ReceiptDownload.vue";
import { useCategoryStore } from "../../../stores/categoryStore";
import ApproveLabel from "../../molecules/labels/ApproveLabel.vue";
import EditButton from "./EditButton.vue";
import { TraveringListData } from "../../../datas/traveringListData";
import { useTraveringExpenseStore } from "../../../stores/traveringExpenseStore";
import { useAzureDirectoryStore } from "../../../stores/azureDirectoryStore";
import { distinct, sum } from "../../../arrayExtensions";
import { format } from "date-fns";
import CustomFilter from "../../molecules/filters/CustomFilter.vue";
import WorkNameSelector from "../../molecules/selectors/WorkNameSelector.vue";
import CategorySelector from "../../molecules/selectors/CategorySelector.vue";
import StatusSelector from "../../molecules/selectors/StatusSelector.vue";

const traveringExpenseStore = useTraveringExpenseStore();
const traveringExpenses = computed(() => traveringExpenseStore.orderedTraveringExpenses
    .filter(x => !filter.date || x.date.getDate() === filter.date.getDate())
    .filter(x => !filter.workName || x.workName === filter.workName)
    .filter(x => !filter.categoryId || x.categoryId === filter.categoryId)
    .filter(x => !filter.status || x.status === filter.status));
const categoryStore = useCategoryStore();
const categoryDictionary = categoryStore.dictionary;
const azureDirectoryStore = useAzureDirectoryStore();
const graphUser = azureDirectoryStore.graphUser;
const dates = computed(() => distinct(traveringExpenseStore.traveringExpenses.map(x => x.date.getTime()).sort((x, y) => x - y))?.map(x => new Date((x as number))));

const filter = reactive<{
    date?: Date,
    categoryId?: string,
    workName?: string,
    status?: string,
}>({
});

const props = defineProps<{ disable: boolean }>();
const emit = defineEmits<{ (event: "update:modelValue", modelValiue: string[]): void }>();

function selection(traveringListDatas: TraveringListData[]){
    emit("update:modelValue", traveringListDatas.map(x => x.id));
}

function getSummaries(){
    const amountSummary = sum(traveringExpenses.value.map(x => x.amount));
    return ["", "", "", "", "", "", "", "合計金額", `${amountSummary.toLocaleString()}円`];
}
</script>

<style>
td > div.cell, th > div.cell {
    white-space:nowrap !important;
}
</style>