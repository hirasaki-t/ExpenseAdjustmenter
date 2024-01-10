<template>
  <Story title="organisms/traveringExpenses/TraveringExpenseAddForm">
    <Variant title="通常">
      <travering-expense-add-form :traffic-categories="trafficCategories" @add="(x) => logEvent('add', x)" />
    </Variant>
    <Variant title="ロード中">
      <travering-expense-add-form :traffic-categories="trafficCategories" loading />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import type { TrafficCategory } from "@/types";
import { logEvent } from "histoire/client";
import TraveringExpenseAddForm from "./TraveringExpenseAddForm.vue";

const trafficCategories: TrafficCategory[] = [
  { id: "1", name: "電車", details: null, isReceipt: false, isActive: true },
  { id: "2", name: "タクシー", details: null, isReceipt: true, isActive: true },
];
</script>
