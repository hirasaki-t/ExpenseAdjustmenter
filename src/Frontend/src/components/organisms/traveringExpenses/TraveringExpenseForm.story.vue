<template>
  <Story title="organisms/traveringExpenses/TraveringExpenseForm">
    <Variant title="通常">
      <travering-expense-form :traffic-categories="trafficCategories" :model-value="{ ...modelValue }" />
    </Variant>
    <Variant title="検証">
      <travering-expense-form
        ref="formRef"
        :traffic-categories="trafficCategories"
        v-model="modelValue"
        @update:model-value="(x) => logEvent('update:model-value', { ...x })"
      />
      <el-button @click="() => validate()">検証</el-button>
    </Variant>
    <Variant title="往復無効">
      <travering-expense-form
        ref="formRef"
        :traffic-categories="trafficCategories"
        v-model="modelValue2"
        @update:model-value="(x) => logEvent('update:model-value', { ...x })"
      />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import type { TrafficCategory, TraveringExpense } from "@/types";
import TraveringExpenseForm from "./TraveringExpenseForm.vue";
import { ref } from "vue";
import { logEvent } from "histoire/client";

const formRef = ref<InstanceType<typeof TraveringExpenseForm>>();

const trafficCategories: TrafficCategory[] = [
  { id: "1", name: "電車", details: null, isReceipt: false, isActive: true },
  { id: "2", name: "タクシー", details: null, isReceipt: true, isActive: true },
];

const modelValue = ref<Omit<TraveringExpense, "id"> & { isGoBack: boolean }>({
  date: new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate()),
  workName: "社内業務",
  startSection: null,
  endSection: null,
  isGoBack: false,
  categoryId: trafficCategories[0].id,
  submissionMethod: null,
  receipt: null,
  amount: 0,
  remarks: null,
});

const modelValue2 = ref<Omit<TraveringExpense, "id">>({
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

function validate() {
  if (!formRef.value) return;
  formRef.value.validate();
}
</script>
