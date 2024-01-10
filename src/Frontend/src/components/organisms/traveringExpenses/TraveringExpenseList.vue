<template>
  <el-table :data="traveringExpenses" border show-summary :summary-method="getSummaries" height="100%">
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
        {{ `${format(scope.row.date, "yyyy年MM月dd日")}` }}
      </template>
    </el-table-column>
    <el-table-column label="区間" align="center" width="250">
      <template #default="scope">
        <div v-if="scope.row.startSection && scope.row.endSection">
          {{ scope.row.startSection }} → {{ scope.row.endSection }}
        </div>
      </template>
    </el-table-column>
    <el-table-column align="center" width="150">
      <template #header>
        <custom-filter title="交通区分" width="100px">
          <traffic-category-selector v-model="filter.categoryId" clearable :traffic-categories="trafficCategories" />
        </custom-filter>
      </template>
      <template #default="scope">
        {{ trafficCategories.find((x) => x.id === scope.row.categoryId)?.name }}
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
          :is-receipt="trafficCategories.find((x) => x.id === scope.row.categoryId)?.isReceipt ?? false"
          :submission-method="scope.row.submissionMethod"
          :receipt-id="scope.row.receiptId"
          :file-name="`領収書_${format((scope.row.date as Date), 'yyyy年MM月dd日')}_${displayName}${scope.row.remarks ? '_' + scope.row.remarks : ''}.pdf`"
        />
      </template>
    </el-table-column>
    <el-table-column align="center" width="100">
      <template #header>
        <custom-filter title="ステータス" width="80px">
          <approve-status-selector v-model="filter.status" clearable />
        </custom-filter>
      </template>
      <template #default="scope">
        <approve-label :user-id="scope.row.reviewerId" :comment="scope.row.comment" :status="scope.row.status" />
      </template>
    </el-table-column>
    <el-table-column width="140" align="center" fixed="right">
      <template #header>
        <add-button :disabled="addDisabled" @add="() => $emit('addApplication')" />
      </template>
      <template #default="scope">
        <edit-button
          :disabled="scope.row.status === '承認' || scope.row.status === '申請中'"
          @edit="() => $emit('update', scope.row.id)"
        />
        <delete-button
          :disabled="scope.row.status === '承認' || scope.row.status === '申請中'"
          :loading="deleteLoading"
          @delete="() => $emit('delete', scope.row.id)"
        />
      </template>
    </el-table-column>
  </el-table>
</template>

<script setup lang="ts">
import { ElTable, ElTableColumn } from "element-plus";
import { computed, reactive } from "vue";
import AddButton from "@/components/molecules/buttons/AddButton.vue";
import ReceiptLabel from "../ReceiptDownload.vue";
import ApproveLabel from "../../molecules/labels/ApproveLabel.vue";
import { distinct, sum } from "../../../arrayExtensions";
import { format } from "date-fns";
import CustomFilter from "../../molecules/filters/CustomFilter.vue";
import WorkNameSelector from "../../molecules/selectors/WorkNameSelector.vue";
import TrafficCategorySelector from "../../molecules/selectors/TrafficCategorySelector.vue";
import ApproveStatusSelector from "../../molecules/selectors/ApproveStatusSelector.vue";
import type { ApproveStatus, TrafficCategory, TraveringExpenseListData } from "@/types";
import DeleteButton from "@/components/molecules/buttons/DeleteButton.vue";
import EditButton from "@/components/molecules/buttons/EditButton.vue";

const props = defineProps<{
  addDisabled?: boolean;
  deleteLoading?: boolean;
  datas: TraveringExpenseListData[];
  displayName: string;
  trafficCategories: TrafficCategory[];
}>();
defineEmits<{
  (event: "update", id: string): void;
  (event: "delete", id: string): void;
  (event: "addApplication"): void;
}>();

const traveringExpenses = computed(() =>
  props.datas
    .filter((x) => !filter.date || x.date.getDate() === filter.date.getDate())
    .filter((x) => !filter.workName || x.workName === filter.workName)
    .filter((x) => !filter.categoryId || x.categoryId === filter.categoryId)
    .filter((x) => !filter.status || x.status === filter.status)
);

const dates = computed(() =>
  distinct([...props.datas].map((x) => x.date.getTime()).sort((x, y) => x - y)).map((x) => new Date(x))
);

const filter = reactive<{
  date?: Date;
  categoryId?: string;
  workName?: string;
  status?: ApproveStatus;
}>({});

function getSummaries() {
  const amountSummary = sum(traveringExpenses.value.map((x) => x.amount));
  return ["", "", "", "", "", "", "", "合計金額", `${amountSummary.toLocaleString()}円`];
}
</script>

<style>
td > div.cell,
th > div.cell {
  white-space: nowrap !important;
}
</style>
