<template>
  <el-select
    :model-value="modelValue"
    placeholder="経費種別選択"
    :clearable="clearable"
    @update:model-value="(x) => $emit('update:modelValue', x)"
    :disabled="expenseTypes.length === 0"
  >
    <el-option
      v-for="expenseType in expenseTypes"
      :key="expenseType.id"
      :label="expenseType.name"
      :value="expenseType.id"
    />
  </el-select>
</template>

<script setup lang="ts">
import { ElSelect, ElOption } from "element-plus";
import type { ExpenseType } from "@/types";
import { computed, effect } from "vue";

const props = defineProps<{ expenseTypes: ExpenseType[]; modelValue?: string; clearable?: boolean }>();
const emits = defineEmits<{ (event: "update:model-value", modelValue?: string): void }>();

const expenseTypes = computed(() => props.expenseTypes.filter((x) => x.isActive));
effect(() => {
  if (!!props.modelValue && expenseTypes.value.every((x) => x.id !== props.modelValue)) emits("update:model-value");
});
</script>
