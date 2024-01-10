<template>
  <div>
    <expense-type-form v-model="formValue" ref="formRef" />
    <div style="text-align: right">
      <update-button :loading="loading" @update="$emit('update', { ...formValue, id: props.expenseType.id })" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from "vue";
import ExpenseTypeForm from "./ExpenseTypeForm.vue";
import UpdateButton from "../../molecules/buttons/UpdateButton.vue";
import type { ExpenseType } from "@/types";

const props = defineProps<{ expenseType: ExpenseType; loading?: boolean }>();
defineEmits<{ (event: "update", expenseType: ExpenseType): void }>();

const formRef = ref<InstanceType<typeof ExpenseTypeForm>>();
const formValue = ref<ExpenseType>({ ...props.expenseType });

watch(
  () => props.expenseType,
  (x) => (formValue.value = { ...x })
);
</script>
