<template>
    <menu-bar>
        <div style="display:flex; flex-direction: column; gap:10px; height: 100%">
            <div style="display:flex; gap:10px">
                <el-select 
                    style="width: 120px" 
                    :model-value="selectedMonth.getTime()" 
                    @update:model-value="(selectedMonthNumber) => updateSelectedMonth(selectedMonthNumber)">
                    <el-option
                        v-for="date in selectableMonths.sort((x, y) => y.getTime() - x.getTime())"
                        :label="`${format(date, 'yyyy年MM月')}`"
                        :value="date.getTime()"
                    />
                </el-select>
                <el-button type="info" :disabled="selectedIds.length < 1" @click="batchFormShow =true">一括確認</el-button>
            </div>
            <div style="flex-grow:1">
                <approve-history-list @update:model-value="updateSelectedIds" />
            </div>
            <div style="display:flex; flex-direction: row-reverse; gap:10px">
                <receipts-download-button :disabled="!approveHistories.some(x => x.submissionMethod === '電子')" />
                <el-button type="success" @click="show = true">CSVダウンロード</el-button>
            </div>
            <csv-download-dialog :date="selectedMonth" :show="show" @complete="show = false" />
            <batch-check-form :show="batchFormShow" :selectedIds="selectedIds" @complete="batchFormShow = false" />
        </div>
    </menu-bar>
</template>

<script setup lang="ts">
import { ElSelect, ElOption, ElButton } from "element-plus";
import MenuBar from "../MenuBar.vue";
import ApproveHistoryList from "../organisms/approveHistories/List.vue";
import CsvDownloadDialog from "../organisms/approveHistories/CsvDownloadDialog.vue";
import ReceiptsDownloadButton from "../organisms/approveHistories/ReceiptsDownloadButton.vue"
import BatchCheckForm from "../organisms/approveHistories/BatchCheckForm.vue";
import { watch, ref, computed } from "vue";
import { useApproveHistoryStore } from "../../stores/approveHistoryStore";
import { addMonths, format } from "date-fns";
import { onBeforeRouteLeave } from "vue-router";

const approveHistoryStore = useApproveHistoryStore();
const approveHistories = computed(() => approveHistoryStore.approveHistories);
const thisMonth = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
const selectableMonths = computed(() => approveHistoryStore.selectableMonths);
const selectedMonth = ref(selectableMonths.value.some(x => x.getFullYear() === thisMonth.getFullYear() && x.getMonth() === thisMonth.getMonth()) ?  thisMonth : addMonths(thisMonth, -1));
const show = ref(false);
const batchFormShow = ref(false);
const selectedIds = ref<string[]>([]);

function updateSelectedMonth(selectedMonthNumber: number) {
    selectedMonth.value = new Date(selectedMonthNumber);
}

function updateSelectedIds(ids: string[]){
    selectedIds.value = ids;
}

watch(selectedMonth, async (newSelectedDate, oldSelectedDate) => {
    if (!newSelectedDate) return;
    if (oldSelectedDate?.getFullYear() !== newSelectedDate.getFullYear() || oldSelectedDate?.getMonth() !== newSelectedDate.getMonth())
        approveHistoryStore.reload(newSelectedDate.getFullYear(), newSelectedDate.getMonth() + 1);
});

onBeforeRouteLeave(() => {
    selectedMonth.value = thisMonth;
    return true;
});
</script>