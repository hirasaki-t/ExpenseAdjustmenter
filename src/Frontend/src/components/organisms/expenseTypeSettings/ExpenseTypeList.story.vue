<template>
  <Story title="organisms/expenseTypes/ExpenseTypeList">
    <Variant title="空">
      <expense-type-list :datas="[]" />
    </Variant>
    <Variant title="データあり">
      <expense-type-list :datas="datas" @add="() => logEvent('add', {})" @update="(x) => logEvent('update', { x })" />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import type { ExpenseType } from "@/types";
import ExpenseTypeList from "./ExpenseTypeList.vue";
import { logEvent } from "histoire/client";

const datas: ExpenseType[] = [
  {
    id: "1",
    name: "会議費",
    details: "5000円以下",
    isReceipt: true,
    isActive: true,
  },
  {
    id: "2",
    name: "接待費",
    details: "5001円以上",
    isReceipt: true,
    isActive: true,
  },
  {
    id: "3",
    name: "雑費",
    details: null,
    isReceipt: false,
    isActive: true,
  },
];
</script>
