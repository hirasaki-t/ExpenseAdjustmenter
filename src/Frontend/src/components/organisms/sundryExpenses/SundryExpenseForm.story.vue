<template>
  <Story title="organisms/sundryExpenses/SundryExpenseForm">
    <Variant title="通常">
      <sundry-expense-form :expense-types="expenseTypes" :model-value="{ ...modelValue }" />
    </Variant>
    <Variant title="検証">
      <sundry-expense-form
        ref="formRef"
        :expense-types="expenseTypes"
        v-model="modelValue"
        @update:model-value="(x) => logEvent('update:model-value', { ...x })"
      />
      <el-button @click="() => validate()">検証</el-button>
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import type { ExpenseType, SundryExpense } from "@/types";
import SundryExpenseForm from "./SundryExpenseForm.vue";
import { ref } from "vue";
import { logEvent } from "histoire/client";

const formRef = ref<InstanceType<typeof SundryExpenseForm>>();

const expenseTypes: ExpenseType[] = [
  { id: "1", name: "会議費", details: null, isReceipt: true, isActive: true },
  { id: "2", name: "書籍購入費", details: null, isReceipt: false, isActive: true },
  { id: "3", name: "福利厚生", details: null, isReceipt: true, isActive: false },
];

const modelValue = ref<Omit<SundryExpense, "id">>({
  date: new Date(2023, 4, 1),
  expenseTypeId: "1",
  submissionMethod: null,
  details: null,
  participationNumber: 0,
  receipt: null,
  amount: 0,
  status: "申請中",
});

function validate() {
  if (!formRef.value) return;
  formRef.value.validate();
}
</script>
