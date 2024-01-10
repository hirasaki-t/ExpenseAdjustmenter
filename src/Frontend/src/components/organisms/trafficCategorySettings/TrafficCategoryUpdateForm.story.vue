<template>
  <Story title="organisms/trafficCategories/TrafficCategoryUpdateForm">
    <Variant title="通常">
      <traffic-category-update-form :traffic-category="{ ...modelValue }" @update="(x) => logEvent('update', x)" />
    </Variant>
    <Variant title="ロード中">
      <traffic-category-update-form :traffic-category="{ ...modelValue }" loading />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import type { TrafficCategory } from "@/types";
import TrafficCategoryUpdateForm from "./TrafficCategoryUpdateForm.vue";
import { ref } from "vue";
import { logEvent } from "histoire/client";

const modelValue = ref<TrafficCategory>({
  id: "1",
  name: "飛行機",
  details: null,
  isReceipt: true,
  isActive: true,
});
</script>
