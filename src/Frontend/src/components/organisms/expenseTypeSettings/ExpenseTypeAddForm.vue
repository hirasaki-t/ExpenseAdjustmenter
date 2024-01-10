<template>
  <div>
    <expense-type-form v-model="formValue" ref="formRef" />
    <div style="text-align: right">
      <add-button :loading="loading" @add="add" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import ExpenseTypeForm from "./ExpenseTypeForm.vue";
import AddButton from "../../molecules/buttons/AddButton.vue";
import type { ExpenseType } from "@/types";

defineProps<{ loading?: boolean }>();
const emit = defineEmits<{ (event: "add", expenseType: Omit<ExpenseType, "id">): void }>();

const formRef = ref<InstanceType<typeof ExpenseTypeForm>>();
const formValue = ref<Omit<ExpenseType, "id">>({
  name: "",
  details: null,
  isReceipt: false,
  isActive: true,
});

async function add() {
  if (!formRef.value) return;
  if (await formRef.value.validate()) {
    emit("add", formValue.value);
  }
}
</script>
