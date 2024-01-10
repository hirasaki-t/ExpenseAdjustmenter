<template>
  <Story title="organisms/expenseTypes/ExpenseTypeAddForm">
    <Variant title="通常">
      <expense-type-add-form @add="(x) => logEvent('add', { ...x })" />
    </Variant>
    <Variant title="ロード中">
      <expense-type-add-form loading />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import ExpenseTypeAddForm from "./ExpenseTypeAddForm.vue";
import { logEvent } from "histoire/client";
</script>
