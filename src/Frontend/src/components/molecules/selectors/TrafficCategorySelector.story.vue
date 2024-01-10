<template>
  <Story title="molecules/selectors/CategorySelector" :layout="{ type: 'grid' }">
    <Variant title="空">
      <traffic-category-selector :traffic-categories="[]" />
    </Variant>
    <Variant title="選択可能">
      <traffic-category-selector :traffic-categories="trafficCategories" />
    </Variant>
    <Variant title="初期選択">
      <traffic-category-selector :traffic-categories="trafficCategories" model-value="1" clearable />
    </Variant>
    <Variant title="無効選択">
      <traffic-category-selector :traffic-categories="trafficCategories" v-model="modelValue" />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import type { TrafficCategory } from "@/types";
import TrafficCategorySelector from "./TrafficCategorySelector.vue";
import { ref } from "vue";

const trafficCategories: TrafficCategory[] = [
  { id: "1", name: "カテゴリ1", details: null, isReceipt: false, isActive: true },
  { id: "2", name: "カテゴリ2", details: null, isReceipt: false, isActive: true },
  { id: "3", name: "カテゴリ3", details: null, isReceipt: false, isActive: false },
];

const modelValue = ref("3");
</script>
