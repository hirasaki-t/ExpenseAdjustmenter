<template>
  <menu-bar>
    <div style="display: flex; flex-direction: column; gap: 10px; height: 100%">
      <div style="display: flex; gap: 10px">
        <month-selector
          :selectable-months="sundryExpenseStore.selectableMonths.sort((x, y) => y.getTime() - x.getTime())"
          v-model="selectedMonth"
        />
        <application-button :loading="loadings.application" @application="application" />
      </div>
      <sundry-expense-list
        :datas="sundryExpenseStore.orderedSundryExoenses"
        :expense-types="expenseTypeStore.orderedExpenseTypes"
        :delete-loading="loadings.delete"
        :display-name="azureDirectoryStore.graphUser?.displayName ?? ''"
        @add="() => (showDialogs.add = true)"
        @update="showUpdate"
        @delete="removeApplication"
      />
    </div>
  </menu-bar>
  <el-dialog title="諸経費追加" v-model="showDialogs.add" width="500px">
    <sundry-expense-add-form
      v-if="showDialogs.add"
      :loading="loadings.add"
      :expense-types="expenseTypeStore.orderedExpenseTypes"
      @add="addApplication"
    />
  </el-dialog>
  <el-dialog title="諸経費更新" v-model="showDialogs.update" width="500px">
    <sundry-expense-update-form
      :expense-types="expenseTypeStore.orderedExpenseTypes"
      :sundry-expense="updateSundryExpense"
      :loading="loadings.update"
      @update="updateApplication"
    />
  </el-dialog>
</template>

<script setup lang="ts">
import { ref, reactive, watch } from "vue";
import MenuBar from "../MenuBar.vue";
import { useSundryExpenseStore } from "../../stores/sundryExpenseStore";
import SundryExpenseList from "../organisms/sundryExpenses/SundryExpenseList.vue";
import SundryExpenseAddForm from "../organisms/sundryExpenses/SundryExpenseAddForm.vue";
import { useExpenseTypeStore } from "@/stores/expenseTypeStore";
import type { SundryExpense } from "@/types";
import SundryExpenseUpdateForm from "../organisms/sundryExpenses/SundryExpenseUpdateForm.vue";
import { useAzureDirectoryStore } from "@/stores/azureDirectoryStore";
import MonthSelector from "../molecules/selectors/MonthSelector.vue";
import ApplicationButton from "../molecules/buttons/ApplicationButton.vue";
import { useApproveHistoryStore } from "@/stores/approveHistoryStore";

const sundryExpenseStore = useSundryExpenseStore();
const expenseTypeStore = useExpenseTypeStore();
const azureDirectoryStore = useAzureDirectoryStore();

const showDialogs = reactive({ add: false, update: false });
const loadings = reactive({ application: false, add: false, update: false, delete: false });

const selectedMonth = ref<Date>(new Date(new Date().getFullYear(), new Date().getMonth(), 1));

const updateSundryExpense = ref<SundryExpense>(undefined! /* 更新時かならず設定するので初期値は警告無視 */);

watch(selectedMonth, async (newSelectedDate, oldSelectedDate) => {
  if (!newSelectedDate) return;
  if (
    oldSelectedDate?.getFullYear() !== newSelectedDate.getFullYear() ||
    oldSelectedDate?.getMonth() !== newSelectedDate.getMonth()
  )
    sundryExpenseStore.reload(newSelectedDate.getFullYear(), newSelectedDate.getMonth() + 1);
});

function showUpdate(id: string) {
  const sundryExpense = sundryExpenseStore.sundryExoenses.find((x) => x.id === id);
  if (!sundryExpense) throw new Error("更新対象の申請が存在しません");
  updateSundryExpense.value = { ...sundryExpense, receipt: null };
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

async function addApplication(sundryExpense: Omit<SundryExpense, "id">) {
  loadings.add = true;
  try {
    await sundryExpenseStore.add(sundryExpense);
    showDialogs.add = false;
  } finally {
    loadings.add = false;
  }
}

async function updateApplication(sundryExpense: SundryExpense) {
  loadings.update = true;
  try {
    await sundryExpenseStore.update(sundryExpense);
    showDialogs.update = false;
  } finally {
    loadings.update = false;
  }
}

async function removeApplication(id: string) {
  loadings.delete = true;
  try {
    await sundryExpenseStore.remove(id);
  } finally {
    loadings.delete = false;
  }
}
</script>
