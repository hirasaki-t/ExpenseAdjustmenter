<template>
  <div style="display: inline-block">
    <sundry-expense-form ref="formRef" v-model="formValue" :expense-types="expenseTypes" />
    <div style="display: flex; justify-content: flex-end">
      <update-button :loading="loading" @update="update" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import UpdateButton from "../../molecules/buttons/UpdateButton.vue";
import SundryExpenseForm from "./SundryExpenseForm.vue";
import type { ExpenseType, SundryExpense } from "@/types";

const emits = defineEmits<{ (event: "update", sundryExpense: SundryExpense): void }>();
const props = defineProps<{ sundryExpense: SundryExpense; expenseTypes: ExpenseType[]; loading?: boolean }>();

const formRef = ref<InstanceType<typeof SundryExpenseForm>>();

const formValue = ref<Omit<SundryExpense, "id">>({ ...props.sundryExpense });

async function update() {
  if (!formRef.value) return;
  if (await formRef.value.validate()) {
    emits("update", { ...formValue.value, id: props.sundryExpense.id });
  }
}
</script>
