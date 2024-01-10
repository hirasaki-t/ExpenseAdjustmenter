<template>
  <Story title="molecules/selectors/ExpenseTypeSelector" :layout="{ type: 'grid' }">
    <Variant title="空">
      <expense-type-selector :expense-types="[]" />
    </Variant>
    <Variant title="選択可能">
      <expense-type-selector :expense-types="expenseTypes" />
    </Variant>
    <Variant title="初期選択">
      <expense-type-selector :expense-types="expenseTypes" model-value="1" clearable />
    </Variant>
    <Variant title="無効選択">
      <expense-type-selector :expense-types="expenseTypes" v-model="modelValue" />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import type { ExpenseType } from "@/types";
import ExpenseTypeSelector from "./ExpenseTypeSelector.vue";
import { ref } from "vue";

const expenseTypes: ExpenseType[] = [
  { id: "1", name: "経費種別1", details: null, isReceipt: false, isActive: true },
  { id: "2", name: "経費種別2", details: null, isReceipt: false, isActive: true },
  { id: "3", name: "経費種別3", details: null, isReceipt: false, isActive: false },
];

const modelValue = ref("3");
</script>
