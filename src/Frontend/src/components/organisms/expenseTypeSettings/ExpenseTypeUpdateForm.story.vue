<template>
  <Story title="organisms/expenseTypes/ExpenseTypeUpdateForm">
    <Variant title="通常">
      <expense-type-update-form :expense-type="{ ...modelValue }" @update="(x) => logEvent('update', x)" />
    </Variant>
    <Variant title="ロード中">
      <expense-type-update-form :expense-type="{ ...modelValue }" loading />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import type { ExpenseType } from "@/types";
import ExpenseTypeUpdateForm from "./ExpenseTypeUpdateForm.vue";
import { ref } from "vue";
import { logEvent } from "histoire/client";

const modelValue = ref<ExpenseType>({
  id: "1",
  name: "会議費",
  details: "5000円以下",
  isReceipt: true,
  isActive: true,
});
</script>
