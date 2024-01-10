<template>
  <div style="display: inline-block">
    <travering-expense-form ref="formRef" :traffic-categories="trafficCategories" v-model="formValue" />
    <div style="display: flex; justify-content: flex-end">
      <update-button :loading="loading" @update="update" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from "vue";
import UpdateButton from "../../molecules/buttons/UpdateButton.vue";
import type { TrafficCategory, TraveringExpense } from "@/types";
import TraveringExpenseForm from "./TraveringExpenseForm.vue";

const props = defineProps<{
  traveringExpense: TraveringExpense;
  trafficCategories: TrafficCategory[];
  loading?: boolean;
}>();
const emit = defineEmits<{ (event: "update", traveringExpense: TraveringExpense): void }>();

const formRef = ref<InstanceType<typeof TraveringExpenseForm>>();
const formValue = ref<TraveringExpense>({ ...props.traveringExpense });

watch(
  () => props.traveringExpense,
  (x) => (formValue.value = { ...x })
);

async function update() {
  if (!formRef.value) return;
  if (await formRef.value.validate()) {
    emit("update", { ...formValue.value, id: props.traveringExpense.id });
  }
}
</script>
