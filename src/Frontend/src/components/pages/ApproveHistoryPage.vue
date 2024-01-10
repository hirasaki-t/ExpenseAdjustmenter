<template>
  <menu-bar>
    <div style="display: flex; flex-direction: column; gap: 10px; height: 100%">
      <div style="display: flex; gap: 10px">
        <el-select
          style="width: 120px"
          :model-value="selectedMonth.getTime()"
          @update:model-value="(selectedMonthNumber) => updateSelectedMonth(selectedMonthNumber)"
        >
          <el-option
            v-for="date in selectableMonths.sort((x, y) => y.getTime() - x.getTime())"
            :label="`${format(date, 'yyyy年MM月')}`"
            :value="date.getTime()"
            :key="date.getTime()"
          />
        </el-select>
        <el-button type="info" :disabled="selectedIds.length < 1" @click="batchFormShow = true">一括確認</el-button>
      </div>
      <div style="flex-grow: 1">
        <approve-history-list @update:model-value="updateSelectedIds" />
      </div>
      <div style="display: flex; flex-direction: row-reverse; gap: 10px">
        <receipts-download-button :disabled="!approveHistories.some((x) => x.submissionMethod === '電子')" />
        <csv-download-button :loading="csvDownloadLoading" @click="csvFormShow = true" />
      </div>
      <batch-check-form :show="batchFormShow" :selectedIds="selectedIds" @complete="batchFormShow = false" />
    </div>
    <el-dialog title="CSVダウンロード" v-model="csvFormShow" width="400">
      <csv-download-form
        @download-sundry-expense-csv="(onlyApproved) => downloadSundryExpenseCsv(onlyApproved)"
        @download-travering-expense-csv="(onlyApproved) => downloadTraveringExpenseCsv(onlyApproved)"
      />
    </el-dialog>
  </menu-bar>
</template>

<script setup lang="ts">
import { ElSelect, ElOption, ElButton, ElDialog } from "element-plus";
import MenuBar from "../MenuBar.vue";
import ApproveHistoryList from "../organisms/approveHistories/List.vue";
import ReceiptsDownloadButton from "../organisms/approveHistories/ReceiptsDownloadButton.vue";
import BatchCheckForm from "../organisms/approveHistories/BatchCheckForm.vue";
import { watch, ref, computed } from "vue";
import { useApproveHistoryStore } from "../../stores/approveHistoryStore";
import {format } from "date-fns";
import { onBeforeRouteLeave } from "vue-router";
import CsvDownloadForm from "../organisms/approveHistories/CsvDownloadForm.vue";
import CsvDownloadButton from "../molecules/buttons/CsvDownloadButton.vue";
import { FileDownloader } from "@/fileDownloader";
import { useUserStore } from "@/stores/userStore";
import { useTrafficCategoryStore } from "@/stores/trafficCategoryStore";
import { useExpenseTypeStore } from "@/stores/expenseTypeStore";

const approveHistoryStore = useApproveHistoryStore();
const approveHistories = computed(() => approveHistoryStore.approveHistories);
const userStore = useUserStore();
const userDictionary = userStore.dictionary;
const trafficCategoryStore = useTrafficCategoryStore();
const trafficCategoryDictionary = trafficCategoryStore.dictionary;
const expenseTypeStore = useExpenseTypeStore();
const expenseTypeDictionary = expenseTypeStore.dictionary;
const thisMonth = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
const selectableMonths = computed(() =>
  approveHistoryStore.selectableMonths.some((x) => x) ? approveHistoryStore.selectableMonths : [thisMonth]
);
const selectedMonth = ref(selectableMonths.value.sort((x, y) => y.getTime() - x.getTime()).find(x => x) ?? thisMonth);

const csvFormShow = ref(false);
const csvDownloadLoading = ref(false);
const batchFormShow = ref(false);
const selectedIds = ref<string[]>([]);

function updateSelectedMonth(selectedMonthNumber: number) {
  selectedMonth.value = new Date(selectedMonthNumber);
}

function updateSelectedIds(ids: string[]) {
  selectedIds.value = ids;
}

watch(selectedMonth, async (newSelectedDate, oldSelectedDate) => {
  if (!newSelectedDate) return;
  if (
    oldSelectedDate?.getFullYear() !== newSelectedDate.getFullYear() ||
    oldSelectedDate?.getMonth() !== newSelectedDate.getMonth()
  )
    approveHistoryStore.reload(newSelectedDate.getFullYear(), newSelectedDate.getMonth() + 1);
});

onBeforeRouteLeave(() => {
  selectedMonth.value = thisMonth;
  return true;
});

async function downloadTraveringExpenseCsv(onlyApproved: boolean) {
  csvFormShow.value = false;
  try {
    csvDownloadLoading.value = true;
    const dataString = approveHistories.value
      .filter((x) => x.type == "旅費・交通費")
      .filter((x) => (onlyApproved ? x.status === "承認" : true))
      .map((x) => {
        return (
          `${x.date.getFullYear()}年${x.date.getMonth() + 1}月${x.date.getDate()}日,` +
          `${userDictionary[x.userId]?.name},` +
          `${x.workName},` +
          `${x.startSection ? x.startSection + "→" + x.endSection : ""},` +
          `${trafficCategoryDictionary[x.categoryId!]?.name},` +
          `${x.remarks ?? ""},` +
          `${x.amount}円,` +
          `${x.status}`
        );
      });
    new FileDownloader("CSV", "UTF-8 BOM").CreateAndDownloadLocalFile(
      ["日付,氏名,業務名,区間,区分,摘要,金額,ステータス", ...dataString].join("\r\n"),
      "旅費・交通費精算書.csv"
    );
  } finally {
    csvDownloadLoading.value = false;
  }
}

async function downloadSundryExpenseCsv(onlyApproved: boolean) {
  csvFormShow.value = false;
  try {
    csvDownloadLoading.value = true;
    const dataString = approveHistories.value
      .filter((x) => x.type == "諸経費")
      .filter((x) => (onlyApproved ? x.status === "承認" : true))
      .map((x) => {
        return (
          `${x.date.getFullYear()}年${x.date.getMonth() + 1}月${x.date.getDate()}日,` +
          `${userDictionary[x.userId]?.name},` +
          `${expenseTypeDictionary[x.expenseTypeId!]?.name},` +
          `${x.details},` +
          `${x.participationNumber}人,` +
          `${x.amount}円,` +
          `${x.status}`
        );
      });
    new FileDownloader("CSV", "UTF-8 BOM").CreateAndDownloadLocalFile(
      ["日付,氏名,経費種別,詳細,人数,金額,ステータス", ...dataString].join("\r\n"),
      "諸経費精算書.csv"
    );
  } finally {
    csvDownloadLoading.value = false;
  }
}
</script>
