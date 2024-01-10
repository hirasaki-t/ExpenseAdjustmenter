<template>
  <menu-bar>
    <div style="display: flex; flex-direction: column; gap: 10px; height: 100%">
      <div style="display: flex; gap: 10px">
        <month-selector
          :selectable-months="traveringExpenseStore.selectableMonths.sort((x, y) => y.getTime() - x.getTime())"
          v-model="selectedMonth"
        />
        <application-button :loading="loadings.application" :disabled="!canApplication" @application="application" />
      </div>
      <travering-expenses-list
        :datas="traveringExpenseStore.orderedTraveringExpenses"
        :categories="categoryStore.trafficCategories"
        :add-disabled="!applicable"
        :delete-loading="loadings.delete"
        :display-name="azureDirectoryStore.graphUser?.displayName ?? ''"
        @add-application="() => (showDialogs.add = true)"
        @update="showUpdate"
        @delete="removeApplication"
      />
    </div>
  </menu-bar>
  <el-dialog title="旅費・交通費追加" v-model="showDialogs.add" width="500px">
    <travering-expense-add-form
      :loading="loadings.add"
      :categories="categoryStore.orderedTrafficCategories"
      v-if="showDialogs.add"
      @add="addApplication"
    />
  </el-dialog>
  <el-dialog title="旅費・交通費編集" v-model="showDialogs.update" width="500px">
    <travering-expense-edit-form
      :loading="loadings.update"
      :categories="categoryStore.orderedTrafficCategories"
      :travering-expense="updateTraveringExpense"
      @update="updateApplication"
    />
  </el-dialog>
</template>

<script setup lang="ts">
import { ref, watch, computed, reactive } from "vue";
import MenuBar from "../MenuBar.vue";
import MonthSelector from "../molecules/selectors/MonthSelector.vue";
import ApplicationButton from "../molecules/buttons/ApplicationButton.vue";
import TraveringExpenseAddForm from "../organisms/traveringExpenses/TraveringExpenseAddForm.vue";
import TraveringExpensesList from "../organisms/traveringExpenses/TraveringExpenseList.vue";
import TraveringExpenseEditForm from "../organisms/traveringExpenses/TraveringExpenseEditForm.vue";
import { useTraveringExpenseStore } from "../../stores/traveringExpenseStore";
import { useSystemSettingStore } from "../../stores/systemSettingStore";
import { useApproveHistoryStore } from "@/stores/approveHistoryStore";
import { useAzureDirectoryStore } from "@/stores/azureDirectoryStore";
import { useTrafficCategoryStore } from "@/stores/trafficCategoryStore";
import type { TraveringExpense } from "@/types";
import { formatDateToYearMonth } from "@/formatter";

const traveringExpenseStore = useTraveringExpenseStore();
const categoryStore = useTrafficCategoryStore();
const azureDirectoryStore = useAzureDirectoryStore();

const showDialogs = reactive({ add: false, update: false });
const loadings = reactive({ add: false, update: false, application: false, delete: false });

const applicable = computed(() => {
  const deadline = useSystemSettingStore().deadline;
  if (
    !deadline ||
    (formatDateToYearMonth(deadline) === formatDateToYearMonth(new Date()) && deadline.getDate() < new Date().getDate())
  )
    return false;
  return true;
});
const selectedMonth = ref<Date>(new Date(new Date().getFullYear(), new Date().getMonth(), 1));
const canApplication = computed(
  () =>
    traveringExpenseStore.traveringExpenses.length !== 0 &&
    traveringExpenseStore.traveringExpenses.filter((x) => x.status === "否認" || x.status === null).length > 0
);
const updateTraveringExpense = ref<TraveringExpense>(undefined! /* 更新時かならず設定するので初期値は警告無視 */);

watch(selectedMonth, async (newSelectedDate, oldSelectedDate) => {
  if (!newSelectedDate) return;
  if (
    oldSelectedDate?.getFullYear() !== newSelectedDate.getFullYear() ||
    oldSelectedDate?.getMonth() !== newSelectedDate.getMonth()
  )
    traveringExpenseStore.reload(newSelectedDate.getFullYear(), newSelectedDate.getMonth() + 1);
});

function showUpdate(id: string) {
  const traveringExpense = traveringExpenseStore.traveringExpenses.find((x) => x.id === id);
  if (!traveringExpense) throw new Error("更新対象の申請が存在しません");
  updateTraveringExpense.value = {
    ...traveringExpense,
    receipt: null,
  };
  showDialogs.update = true;
}

async function application() {
  loadings.application = true;
  try {
    await useApproveHistoryStore().application(selectedMonth.value);
  } finally {
    loadings.application = false;
  }
}

async function addApplication(traveringExpenses: Omit<TraveringExpense, "id">[]) {
  loadings.add = true;
  try {
    for (const traveringExpense of traveringExpenses) await traveringExpenseStore.add(traveringExpense);
    showDialogs.add = false;
  } finally {
    loadings.add = false;
  }
}

async function updateApplication(traveringExpense: TraveringExpense) {
  loadings.update = true;
  try {
    await traveringExpenseStore.update(traveringExpense);
    showDialogs.update = false;
  } finally {
    loadings.update = false;
  }
}

async function removeApplication(id: string) {
  loadings.delete = true;
  try {
    await traveringExpenseStore.remove(id);
  } finally {
    loadings.delete = false;
  }
}
</script>
