<template>
  <div style="display: inline-block">
    <travering-expense-form ref="formRef" :traffic-categories="trafficCategories" v-model="formValue" />
    <div style="text-align: right">
      <add-button :loading="loading" @add="add" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import AddButton from "../../molecules/buttons/AddButton.vue";
import type { TrafficCategory, TraveringExpense } from "@/types";
import TraveringExpenseForm from "./TraveringExpenseForm.vue";

const props = defineProps<{ loading?: boolean; trafficCategories: TrafficCategory[] }>();
const emits = defineEmits<{ (event: "add", traveringExpense: Omit<TraveringExpense, "id">[]): void }>();

const formRef = ref<InstanceType<typeof TraveringExpenseForm>>();
const formValue = ref<Omit<TraveringExpense, "id"> & { isGoBack: boolean }>({
  date: new Date(),
  workName: "社内業務",
  startSection: null,
  endSection: null,
  isGoBack: false,
  categoryId: props.trafficCategories[0]?.id,
  submissionMethod: null,
  receipt: null,
  amount: 0,
  remarks: null,
});

async function add() {
  if (!formRef.value) return;
  if (await formRef.value.validate()) {
    if (formValue.value.isGoBack) {
      emits("add", [
        { ...formValue.value },
        { ...formValue.value, startSection: formValue.value.endSection, endSection: formValue.value.startSection },
      ]);
    } else {
      emits("add", [{ ...formValue.value }]);
    }
  }
}
</script>
