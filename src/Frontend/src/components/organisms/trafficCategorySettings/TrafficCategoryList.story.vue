<template>
  <Story title="organisms/trafficCategories/TrafficCategoryList">
    <Variant title="空">
      <traffic-category-list :datas="[]" @add="() => logEvent('add', {})" @update="(x) => logEvent('update', { x })" />
    </Variant>
    <Variant title="データあり">
      <traffic-category-list
        :datas="datas"
        @add="() => logEvent('add', {})"
        @update="(x) => logEvent('update', { x })"
      />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import type { TrafficCategory } from "@/types";
import { logEvent } from "histoire/client";
import TrafficCategoryList from "./TrafficCategoryList.vue";

const datas: TrafficCategory[] = [
  {
    id: "1",
    name: "電車",
    details: null,
    isReceipt: false,
    isActive: true,
  },
  {
    id: "2",
    name: "バス",
    details: null,
    isReceipt: false,
    isActive: true,
  },
  {
    id: "3",
    name: "タクシー",
    details: "事由必須",
    isReceipt: true,
    isActive: true,
  },
];
</script>
