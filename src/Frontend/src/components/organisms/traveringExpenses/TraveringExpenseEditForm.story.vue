<template>
  <Story title="organisms/traveringExpenses/TraveringExpenseEditForm">
    <Variant title="通常">
      <travering-expense-edit-form
        :traffic-categories="trafficCategories"
        :travering-expense="{ ...modelValue }"
        @update="(x) => logEvent('update', x)"
      />
    </Variant>
    <Variant title="ロード中">
      <travering-expense-edit-form
        :traffic-categories="trafficCategories"
        :travering-expense="{ ...modelValue }"
        loading
      />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import type { TrafficCategory, TraveringExpense } from "@/types";
import TraveringExpenseEditForm from "./TraveringExpenseEditForm.vue";
import { ref } from "vue";
import { logEvent } from "histoire/client";

const trafficCategories: TrafficCategory[] = [
  { id: "1", name: "電車", details: null, isReceipt: false, isActive: true },
  { id: "2", name: "タクシー", details: null, isReceipt: true, isActive: true },
];

const modelValue = ref<TraveringExpense>({
  id: "1",
  date: new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate()),
  workName: "社内業務",
  startSection: null,
  endSection: null,
  categoryId: trafficCategories[0].id,
  submissionMethod: null,
  receipt: null,
  amount: 0,
  remarks: null,
});
</script>
