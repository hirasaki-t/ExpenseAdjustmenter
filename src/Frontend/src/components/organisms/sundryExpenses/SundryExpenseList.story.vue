<template>
  <Story title="organisms/sundryExpenses/SundryExpenseList">
    <Variant title="空">
      <sundry-expense-list :expense-types="expenseTypes" :datas="[]" display-name="山田太郎" />
    </Variant>
    <Variant title="通常">
      <sundry-expense-list
        :expense-types="expenseTypes"
        :datas="sundryExpenses"
        display-name="山田太郎"
        @add="() => logEvent('add', {})"
        @update="(x) => logEvent('update', { x })"
        @delete="(x) => logEvent('delete', { x })"
      />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import type { ExpenseType, SundryExpenseListData } from "@/types";
import SundryExpenseList from "./SundryExpenseList.vue";
import { logEvent } from "histoire/client";

const expenseTypes: ExpenseType[] = [
  { id: "1", name: "会議費", details: null, isReceipt: true, isActive: true },
  { id: "2", name: "書籍購入費", details: null, isReceipt: false, isActive: true },
  { id: "3", name: "福利厚生", details: null, isReceipt: true, isActive: false },
];

const sundryExpenses: SundryExpenseListData[] = [
  {
    id: "1",
    date: new Date(2023, 4, 1),
    expenseTypeId: "1",
    details: "詳細",
    participationNumber: 1,
    receiptId: null,
    submissionMethod: "紙/持参",
    amount: 500,
    reviewerId: null,
    comment: null,
    status: null,
  },
  {
    id: "2",
    date: new Date(2023, 4, 2),
    expenseTypeId: "2",
    details: "詳細",
    participationNumber: 1,
    receiptId: null,
    submissionMethod: null,
    amount: 1500,
    reviewerId: "1",
    comment: "コメント",
    status: "否認",
  },
];
</script>
