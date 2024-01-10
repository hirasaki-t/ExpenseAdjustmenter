<template>
  <div style="display: inline-block">
    <sundry-expense-form ref="formRef" :expense-types="expenseTypes" v-model="formValue" />
    <div style="text-align: right">
      <add-button :loading="loading" @add="add" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import AddButton from "../../molecules/buttons/AddButton.vue";
import SundryExpenseForm from "./SundryExpenseForm.vue";
import type { ExpenseType, SundryExpense } from "@/types";

const props = defineProps<{ expenseTypes: ExpenseType[]; loading?: boolean }>();
const emits = defineEmits<{ (event: "add", sundryExpense: Omit<SundryExpense, "id">): void }>();

const formRef = ref<InstanceType<typeof SundryExpenseForm>>();
const formValue = ref<Omit<SundryExpense, "id">>({
  date: new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate()),
  expenseTypeId: props.expenseTypes[0]?.id ?? "",
  submissionMethod: null,
  details: null,
  participationNumber: 1,
  receipt: null,
  amount: 0,
  status: null,
});

async function add() {
  if (!formRef.value) return;
  if (await formRef.value.validate()) {
    emits("add", formValue.value);
  }
}
</script>
