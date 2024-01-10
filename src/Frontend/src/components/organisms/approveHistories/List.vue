<template>
  <el-table
    :data="approveHistories"
    border
    show-summary
    :summary-method="getSummaries"
    @selection-change="selected"
    height="100%"
  >
    <el-table-column type="selection" align="center" />
    <el-table-column property="date" align="center" width="150">
      <template #header>
        <custom-filter title="日付" width="80px">
          <el-select v-model="date" clearable placeholder="日付">
            <el-option
              v-for="date in dates"
              :label="`${format(date, 'yyyy年MM月dd日')}`"
              :value="date"
              :key="date.getTime()"
            />
          </el-select>
        </custom-filter>
      </template>
      <template #default="scope">
        {{ `${format(scope.row.date, "yyyy年MM月dd日")}` }}
      </template>
    </el-table-column>
    <el-table-column align="center" width="200">
      <template #header>
        <custom-filter title="氏名" width="80px">
          <el-select v-model="userName" clearable placeholder="氏名">
            <el-option
              v-for="userName in userNames"
              :label="(userName as string)"
              :value="(userName as string)"
              :key="(userName as string)"
            />
          </el-select>
        </custom-filter>
      </template>
      <template #default="scope">
        {{ userDictionary[scope.row.userId].name }}
      </template>
    </el-table-column>
    <el-table-column property="type" align="center" width="150">
      <template #header>
        <custom-filter title="分類" width="100px">
          <el-select v-model="type" clearable placeholder="分類">
            <el-option
              v-for="expenseType in types"
              :label="(expenseType as string)"
              :value="(expenseType as string)"
              :key="(expenseType as string)"
            />
          </el-select>
        </custom-filter>
      </template>
    </el-table-column>
    <el-table-column label="摘要/詳細" align="center">
      <template #default="scope">
        {{ scope.row.type === "旅費・交通費" ? scope.row.remarks : scope.row.details }}
      </template>
    </el-table-column>
    <el-table-column label="領収書" header-align="center" align="center" width="100">
      <template #default="scope">
        <span v-if="scope.row.type === '旅費・交通費'">
          <receipt-download
            :is-receipt="scope.row.categoryId ? categoryDictionary[scope.row.categoryId].isReceipt : false"
            :submission-method="scope.row.submissionMethod"
            :receipt-id="scope.row.receiptId"
            :file-name="`領収書_${format((scope.row.date as Date), 'yyyy年MM月dd日')}_${userDictionary[scope.row.userId].name.replace(' ', '')}_${scope.row.remarks}.pdf`"
          />
        </span>
        <span v-else-if="scope.row.type === '諸経費'">
          <receipt-download
            :is-receipt="scope.row.expenseTypeId ? expenseTypeDictionary[scope.row.expenseTypeId].isReceipt : false"
            :submission-method="scope.row.submissionMethod"
            :receipt-id="scope.row.receiptId"
            :file-name="`領収書_${format((scope.row.date as Date), 'yyyy年MM月dd日')}_${userDictionary[scope.row.userId].name.replace(' ', '')}_${scope.row.details}.pdf`"
          />
        </span>
      </template>
    </el-table-column>
    <el-table-column label="金額" property="amount" header-align="center" align="right" width="100">
      <template #default="scope">
        {{ `${(scope.row.amount as number).toLocaleString()}円` }}
      </template>
    </el-table-column>
    <el-table-column align="center" width="100">
      <template #header>
        <custom-filter title="ステータス" width="100px">
          <approve-status-selector v-model="status" clearable />
        </custom-filter>
      </template>
      <template #default="scope">
        <approve-label :user-id="scope.row.userId" :comment="scope.row.comment" :status="scope.row.status" />
      </template>
    </el-table-column>
    <el-table-column width="140" align="center" fixed="right">
      <template #default="scope">
        <stack-container>
          <span>
            <el-button type="info" @click="openDetailsForm(scope.row.id)">確認</el-button>
          </span>
          <span>
            <el-button @click="openHistoryForm(scope.row.expenseId)">履歴</el-button>
          </span>
        </stack-container>
      </template>
    </el-table-column>
    <details-form
      v-if="detailsFormShow"
      :id="selectedRowId"
      :show="detailsFormShow"
      @complete="detailsFormShow = false"
    />
    <history-dialog
      v-if="historyFormShow"
      :id="selectedRowId"
      :show="historyFormShow"
      @complete="historyFormShow = false"
    />
  </el-table>
</template>

<script setup lang="ts">
import { ElTable, ElTableColumn, ElButton, ElSelect, ElOption } from "element-plus";
import StackContainer from "../../atomos/StackContainer.vue";
import { useUserStore } from "../../../stores/userStore";
import { useApproveHistoryStore } from "../../../stores/approveHistoryStore";
import DetailsForm from "./DetailsForm.vue";
import { computed, ref } from "vue";
import ReceiptDownload from "../ReceiptDownload.vue";
import { useTrafficCategoryStore } from "../../../stores/trafficCategoryStore";
import { useExpenseTypeStore } from "../../../stores/expenseTypeStore";
import ApproveLabel from "../../molecules/labels/ApproveLabel.vue";
import CustomFilter from "../../molecules/filters/CustomFilter.vue";
import { distinct, sum } from "../../../arrayExtensions";
import ApproveStatusSelector from "../../molecules/selectors/ApproveStatusSelector.vue";
import HistoryDialog from "./HistoryDialog.vue";
import { format } from "date-fns";
import type { ApproveHistory, ApproveStatus } from "@/types";

const emit = defineEmits<{ (event: "update:modelValue", modelValiue: string[]): void }>();
const userStore = useUserStore();
const userDictionary = userStore.dictionary;
const approveHistoryStore = useApproveHistoryStore();
const approveHistories = computed(() =>
  approveHistoryStore.orderedApproveHistories
    .filter((x) => !date.value || x.date.getDate() === date.value.getDate())
    .filter((x) => !userName.value || userDictionary[x.userId].name === userName.value)
    .filter((x) => !status.value || x.status === status.value)
    .filter((x) => !type.value || x.type === type.value)
);
const trafficCategoryStore = useTrafficCategoryStore();
const categoryDictionary = trafficCategoryStore.dictionary;
const expenseTypeStore = useExpenseTypeStore();
const expenseTypeDictionary = expenseTypeStore.dictionary;

const userNames = computed(() =>
  distinct(approveHistoryStore.approveHistories.map((x) => userDictionary[x.userId]?.name))
);
const types = computed(() => distinct(approveHistoryStore.approveHistories.map((x) => x.type)));
const dates = computed(() =>
  distinct(approveHistoryStore.approveHistories.map((x) => x.date.getTime()).sort((x, y) => x - y))?.map(
    (x) => new Date(x as number)
  )
);

const detailsFormShow = ref(false);
const historyFormShow = ref(false);
const selectedRowId = ref("");

const date = ref<Date>();
const userName = ref("");
const type = ref("");
const status = ref<ApproveStatus>();

function openDetailsForm(id: string) {
  selectedRowId.value = id;
  detailsFormShow.value = true;
}

function openHistoryForm(id: string) {
  selectedRowId.value = id;
  historyFormShow.value = true;
}

function getSummaries() {
  const amountSummary = sum(approveHistories.value.map((x) => x.amount));
  return ["", "", "", "", "", "", "", "合計金額", `${amountSummary.toLocaleString()}円`];
}

function selected(approveHistories: ApproveHistory[]) {
  emit(
    "update:modelValue",
    approveHistories.map((x) => x.expenseId)
  );
}
</script>

<style>
td > div.cell,
th > div.cell {
  white-space: nowrap !important;
}
</style>
