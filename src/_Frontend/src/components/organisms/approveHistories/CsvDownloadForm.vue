<template>
    <el-form label-placement="left" label-width="120px">
        <el-form-item label="旅費・交通費">
            <div style="display:flex; gap:10px">
                <el-button type="success" @click="downloadTraveringExpensesCsv"> ダウンロード </el-button>
                <el-checkbox v-model="isOnlyTraveringApproved">承認済のみ</el-checkbox>
            </div>
        </el-form-item>
        <el-form-item label="諸経費">
            <div style="display:flex; gap:10px">
                <el-button type="success" @click="downloadSundryExpensesCsv"> ダウンロード </el-button>
                <el-checkbox v-model="isOnlySundryApproved">承認済のみ</el-checkbox>
            </div>
        </el-form-item>
    </el-form>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { ElForm, ElFormItem, ElButton, ElCheckbox } from "element-plus";
import { useApproveHistoryStore } from "../../../stores/approveHistoryStore";
import { FileDownloader } from "../../../fileDownloader"
import { useUserStore } from "../../../stores/userStore";
import { useCategoryStore } from "../../../stores/categoryStore";
import { useExpenseTypeStore } from "../../../stores/expenseTypeStore";

const approveHistoryStore = useApproveHistoryStore();
const approveHistories = computed(() => approveHistoryStore.approveHistories);
const userStore = useUserStore();
const userDictionary = userStore.dictionary;
const categoryStore = useCategoryStore();
const categoryDictionary = categoryStore.dictionary;
const expenseTypeStore = useExpenseTypeStore();
const exoneseTypeDictionary = expenseTypeStore.dictionary;

const isOnlyTraveringApproved = ref(true);
const isOnlySundryApproved = ref(true);

async function downloadTraveringExpensesCsv(){
    try{
        const dataString = approveHistories.value.filter(x => x.type == '旅費・交通費').filter(x => isOnlyTraveringApproved.value ? x.status === '承認' : true).map(x => {
            return (
                `${ x.date.getFullYear() }年${ x.date.getMonth() + 1 }月${ x.date.getDate() }日,` +
                `${ userDictionary[x.userId]?.name },` +
                `${ x.workName },` +
                `${ x.startSection ? x.startSection + '→' + x.endSection : '' },` +
                `${ categoryDictionary[x.categoryId!]?.name },` +
                `${ x.remarks ?? '' },` +
                `${ x.amount }円,` +
                `${ x.status }`
            );
        });
        new FileDownloader("CSV", "UTF-8 BOM").CreateAndDownloadLocalFile(
            ["日付,氏名,業務名,区間,区分,摘要,金額,ステータス", ...dataString].join("\r\n"), "旅費・交通費精算書.csv"
        );
    } finally {

    }
}

async function downloadSundryExpensesCsv(){
    try{
        const dataString = approveHistories.value.filter(x => x.type == '諸経費').filter(x => isOnlySundryApproved ? x.status === '承認' : true).map(x => {
            return (
                `${ x.date.getFullYear() }年${ x.date.getMonth() + 1 }月${ x.date.getDate() }日,` +
                `${ userDictionary[x.userId]?.name },` +
                `${ exoneseTypeDictionary[x.expenseTypeId!]?.name },` +
                `${ x.details },` +
                `${ x.participationNumber }人,` +
                `${ x.amount }円,` +
                `${ x.status }`
            );
        });
        new FileDownloader("CSV", "UTF-8 BOM").CreateAndDownloadLocalFile(
            ["日付,氏名,経費種別,詳細,人数,金額,ステータス", ...dataString].join("\r\n"), "諸経費精算書.csv"
        );
    } finally {

    }
}
</script>
