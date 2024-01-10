<template>
  <Story title="organisms/expenseTypes/ExpenseTypeForm">
    <Variant title="通常">
      <expense-type-form v-model="modelValue" />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import type { ExpenseType } from "@/types";
import ExpenseTypeForm from "./ExpenseTypeForm.vue";
import { ref } from "vue";

const modelValue = ref<Omit<ExpenseType, "id">>({
  name: "会議費",
  details: "5000円以上",
  isReceipt: true,
  isActive: true,
});
</script>
