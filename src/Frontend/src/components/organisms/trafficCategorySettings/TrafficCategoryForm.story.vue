<template>
  <Story title="organisms/trafficCategories/TrafficCategoryForm">
    <Variant title="通常">
      <travering-category-form v-model="modelValue" />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import type { TrafficCategory } from "@/types";
import TraveringCategoryForm from "./TrafficCategoryForm.vue";
import { ref } from "vue";

const modelValue = ref<Omit<TrafficCategory, "id">>({
  name: "新幹線",
  details: null,
  isReceipt: true,
  isActive: true,
});
</script>
