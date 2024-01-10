<template>
  <Story title="organisms/sundryExpenses/SundryExpenseUpdateForm">
    <Variant title="通常">
      <sundry-expense-update-form
        :expense-types="expenseTypes"
        :sundry-expense="sundryExpense"
        @update="(x) => logEvent('update', { ...x })"
      />
    </Variant>
    <Variant title="ロード中">
      <sundry-expense-update-form :expense-types="expenseTypes" :sundry-expense="sundryExpense" loading />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import type { ExpenseType, SundryExpense } from "@/types";
import SundryExpenseUpdateForm from "./SundryExpenseUpdateForm.vue";
import { logEvent } from "histoire/client";

const sundryExpense: SundryExpense = {
  id: "1",
  date: new Date(2023, 4, 1),
  expenseTypeId: "1",
  submissionMethod: "紙/持参",
  details: "摘要",
  participationNumber: 5,
  receipt: null,
  amount: 5000,
  status: "申請中",
};

const expenseTypes: ExpenseType[] = [
  { id: "1", name: "会議費", details: null, isReceipt: true, isActive: true },
  { id: "2", name: "書籍購入費", details: null, isReceipt: false, isActive: true },
  { id: "3", name: "福利厚生", details: null, isReceipt: true, isActive: false },
];
</script>
