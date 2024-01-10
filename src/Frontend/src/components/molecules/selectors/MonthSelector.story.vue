<template>
  <Story title="molecules/selectors/MonthSelector" :layout="{ type: 'grid' }">
    <Variant title="空">
      <month-selector :selectable-months="[]" />
    </Variant>
    <Variant title="選択可能">
      <month-selector :selectable-months="months" />
    </Variant>
    <Variant title="初期選択">
      <month-selector :selectable-months="months" :model-value="months[0]" />
    </Variant>
    <Variant title="プレースホルダ">
      <month-selector :selectable-months="months" placeholder="プレースホルダ" />
    </Variant>
    <Variant title="無効選択">
      <month-selector :selectable-months="months" v-model="modelValue" />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import { ref } from "vue";
import MonthSelector from "./MonthSelector.vue";

const months = [new Date(2023, 1, 1), new Date(2023, 2, 1), new Date(2023, 3, 1)];
const modelValue = ref(new Date(2023, 4, 1));
</script>
