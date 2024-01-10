<template>
  <Story title="organisms/sundryExpenses/SundryExpenseAddForm">
    <Variant title="通常">
      <sundry-expense-add-form :expense-types="expenseTypes" @add="(x) => logEvent('add', { ...x })" />
    </Variant>
    <Variant title="ロード中">
      <sundry-expense-add-form :expense-types="expenseTypes" loading />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import type { ExpenseType } from "@/types";
import SundryExpenseAddForm from "./SundryExpenseAddForm.vue";
import { logEvent } from "histoire/client";

const expenseTypes: ExpenseType[] = [
  { id: "1", name: "会議費", details: null, isReceipt: true, isActive: true },
  { id: "2", name: "書籍購入費", details: null, isReceipt: false, isActive: true },
  { id: "3", name: "福利厚生", details: null, isReceipt: true, isActive: false },
];
</script>
