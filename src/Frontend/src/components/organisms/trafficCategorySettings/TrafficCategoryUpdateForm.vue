<template>
  <div>
    <traffic-category-form v-model="formValue" ref="formRef" />
    <div style="text-align: right">
      <update-button :loading="loading" @update="$emit('update', { ...formValue, id: props.trafficCategory.id })" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from "vue";
import UpdateButton from "../../molecules/buttons/UpdateButton.vue";
import TrafficCategoryForm from "./TrafficCategoryForm.vue";
import type { TrafficCategory } from "@/types";
import type TrafficCategoryFormVue from "./TrafficCategoryForm.vue";

const props = defineProps<{ trafficCategory: TrafficCategory; loading?: boolean }>();
defineEmits<{ (event: "update", trafficCategory: TrafficCategory): void }>();

const formRef = ref<InstanceType<typeof TrafficCategoryFormVue>>();
const formValue = ref<TrafficCategory>({ ...props.trafficCategory });

watch(
  () => props.trafficCategory,
  (x) => (formValue.value = { ...x })
);
</script>
