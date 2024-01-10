<template>
  <el-select
    :model-value="modelValue"
    placeholder="区分選択"
    :clearable="clearable"
    @update:model-value="(x) => $emit('update:modelValue', x)"
    :disabled="trafficCategories.length === 0"
  >
    <el-option
      v-for="category in trafficCategories.filter((x) => x.isActive)"
      :key="category.id"
      :label="category.name"
      :value="category.id"
    />
  </el-select>
</template>

<script setup lang="ts">
import { ElSelect, ElOption } from "element-plus";
import type { TrafficCategory } from "@/types";
import { computed, effect } from "vue";

const props = defineProps<{ trafficCategories: TrafficCategory[]; modelValue?: string; clearable?: boolean }>();
const emits = defineEmits<{ (event: "update:model-value", modelValue?: string): void }>();

const trafficCategories = computed(() => props.trafficCategories.filter((x) => x.isActive));
effect(() => {
  if (!!props.modelValue && trafficCategories.value.every((x) => x.id !== props.modelValue))
    emits("update:model-value");
});
</script>
