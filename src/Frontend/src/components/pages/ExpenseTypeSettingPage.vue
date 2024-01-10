<template>
  <menu-bar>
    <expense-type-list
      :datas="expenseTypeStore.orderedExpenseTypes"
      @add="showDialogs.add = true"
      @update="(id) => openUpdateForm(id)"
    />
    <el-dialog v-model="showDialogs.add" title="経費種別追加" width="400">
      <expense-type-add-form :loading="loadings.add" @add="(expenseType) => addExpenseType(expenseType)" />
    </el-dialog>
    <el-dialog v-model="showDialogs.update" title="経費種別編集" width="400">
      <expense-type-update-form
        :expense-type="selectedExpenseType"
        @update="(expenseType) => updateExpenseType(expenseType)"
        :loading="loadings.update"
        v-if="selectedExpenseType"
      />
    </el-dialog>
  </menu-bar>
</template>

<script setup lang="ts">
import { useExpenseTypeStore } from "@/stores/expenseTypeStore";
import MenuBar from "../MenuBar.vue";
import ExpenseTypeList from "../organisms/expenseTypeSettings/ExpenseTypeList.vue";
import { reactive, ref } from "vue";
import type { ExpenseType } from "@/types";
import ExpenseTypeAddForm from "../organisms/expenseTypeSettings/ExpenseTypeAddForm.vue";
import ExpenseTypeUpdateForm from "../organisms/expenseTypeSettings/ExpenseTypeUpdateForm.vue";

const expenseTypeStore = useExpenseTypeStore();
const loadings = reactive({ add: false, update: false });
const showDialogs = reactive({ add: false, update: false });
const selectedExpenseType = ref<ExpenseType>();

function openUpdateForm(id: string) {
  selectedExpenseType.value = expenseTypeStore.dictionary[id];
  showDialogs.update = true;
}

async function addExpenseType(expenseType: Omit<ExpenseType, "id">) {
  loadings.add = true;
  try {
    await expenseTypeStore.add(expenseType.name, expenseType.details, expenseType.isReceipt);
  } finally {
    loadings.add = false;
    showDialogs.add = false;
  }
}

async function updateExpenseType(expenseType: ExpenseType) {
  loadings.update = true;
  try {
    await expenseTypeStore.update(
      expenseType.id,
      expenseType.name,
      expenseType.details,
      expenseType.isReceipt,
      expenseType.isActive
    );
  } finally {
    loadings.update = false;
    showDialogs.update = false;
  }
}
</script>
