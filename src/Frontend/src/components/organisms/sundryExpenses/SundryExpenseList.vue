<template>
  <el-table :data="sundryExpenses" border show-summary :summary-method="getSummaries" height="100%">
    <el-table-column align="center" width="150">
      <template #header>
        <custom-filter title="日付" width="100px">
          <el-select
            :model-value="filter.date"
            @update:model-value="(x) => (filter.date = !x ? undefined : new Date(x))"
            placeholder="日付"
          >
            <el-option
              v-for="date in dates"
              :key="date.getTime()"
              :value="date.getTime()"
              :label="`${date.getDate()}日`"
            />
          </el-select>
        </custom-filter>
      </template>
      <template #default="scope">
        {{ formatDate(scope.row.date) }}
      </template>
    </el-table-column>
    <el-table-column align="center" width="250">
      <template #header>
        <custom-filter title="経費種別" width="150px">
          <expense-type-selector v-model="filter.expenseTypeId" :expense-types="expenseTypes" clearable />
        </custom-filter>
      </template>
      <template #default="scope">
        {{ expenseTypes.find((x) => x.id === scope.row.id)?.name }}
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
          :is-receipt="expenseTypes.find((x) => x.id === scope.row.id)?.isReceipt ?? false"
          :submission-method="scope.row.submissionMethod"
          :receipt-id="scope.row.receiptId"
          :file-name="`領収書_${formatDate(scope.row.date)}_${displayName}_${scope.row.details}.pdf`"
        />
      </template>
    </el-table-column>
    <el-table-column align="center" width="100">
      <template #header>
        <custom-filter title="ステータス" width="100px">
          <approve-status-selector v-model="filter.status" clearable />
        </custom-filter>
      </template>
      <template #default="scope">
        <approve-label :status="scope.row.status" :comment="scope.row.comment" :user-id="scope.row.reviewerId" />
      </template>
    </el-table-column>
    <el-table-column width="140" align="center" fixed="right">
      <template #header>
        <add-button @add="() => $emit('add')" />
      </template>
      <template #default="scope">
        <edit-button
          :disabled="scope.row.status === '承認' || scope.row.status === '申請中'"
          @edit="() => $emit('update', scope.row.id)"
        />
        <delete-button
          :loading="deleteLoading"
          :disabled="scope.row.status === '承認' || scope.row.status === '申請中'"
          @delete="() => $emit('delete', scope.row.id)"
        />
      </template>
    </el-table-column>
  </el-table>
</template>

<script setup lang="ts">
import type { ApproveStatus, ExpenseType, SundryExpenseListData } from "@/types";
import { ElTable, ElTableColumn, ElSelect, ElOption } from "element-plus";
import { computed, reactive } from "vue";
import { distinct, sum } from "@/arrayExtensions";
import { formatDate } from "@/formatter";
import ReceiptLabel from "../ReceiptDownload.vue";
import ApproveLabel from "../../molecules/labels/ApproveLabel.vue";
import CustomFilter from "../../molecules/filters/CustomFilter.vue";
import ExpenseTypeSelector from "../../molecules/selectors/ExpenseTypeSelector.vue";
import ApproveStatusSelector from "../../molecules/selectors/ApproveStatusSelector.vue";
import AddButton from "@/components/molecules/buttons/AddButton.vue";
import EditButton from "@/components/molecules/buttons/EditButton.vue";
import DeleteButton from "@/components/molecules/buttons/DeleteButton.vue";

defineEmits<{ (event: "add"): void; (event: "update", id: string): void; (event: "delete", id: string): void }>();
const props = defineProps<{
  datas: SundryExpenseListData[];
  expenseTypes: ExpenseType[];
  displayName: string;
  deleteLoading?: boolean;
}>();

const sundryExpenses = computed(() =>
  props.datas
    .filter((x) => !filter.date || x.date.getDate() === filter.date.getDate())
    .filter((x) => !filter.expenseTypeId || x.expenseTypeId === filter.expenseTypeId)
    .filter((x) => !filter.status || x.status === filter.status)
);

const dates = computed(() =>
  distinct([...props.datas].map((x) => x.date.getTime()))
    .sort((x, y) => x - y)
    .map((x) => new Date(x))
);

const filter = reactive<{
  date?: Date;
  expenseTypeId?: string;
  status?: ApproveStatus;
}>({});

function getSummaries() {
  const amountSummary = sum(sundryExpenses.value.map((x) => x.amount));
  return ["", "", "", "", "", "", "合計金額", `${amountSummary.toLocaleString()}円`];
}
</script>

<style>
td > div.cell,
th > div.cell {
  white-space: nowrap !important;
}
</style>
