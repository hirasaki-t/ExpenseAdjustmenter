<template>
  <div>
    <traffic-category-form v-model="formValue" ref="formRef" />
    <div style="text-align: right">
      <add-button :loading="loading" @add="add" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import AddButton from "../../molecules/buttons/AddButton.vue";
import TrafficCategoryForm from "./TrafficCategoryForm.vue";
import type { TrafficCategory } from "@/types";

defineProps<{ loading?: boolean }>();
const emit = defineEmits<{ (event: "add", trafficCategory: Omit<TrafficCategory, "id">): void }>();

const formRef = ref<InstanceType<typeof TrafficCategoryForm>>();
const formValue = ref<Omit<TrafficCategory, "id">>({
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
