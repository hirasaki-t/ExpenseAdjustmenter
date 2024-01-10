<template>
  <el-select
    :model-value="modelValue"
    placeholder="業務名選択"
    :clearable="clearable"
    @update:model-value="(x) => $emit('update:modelValue', x)"
  >
    <el-option v-for="item in workNames" :key="item" :label="item" :value="item" />
  </el-select>
</template>

<script setup lang="ts">
import type { WorkName } from "@/types";
import { ElSelect, ElOption } from "element-plus";

defineProps<{ modelValue?: string; clearable?: boolean }>();
defineEmits<{ (event: "update:model-value", modelValue?: string): void }>();

const workNames: WorkName[] = ["社内業務", "営業", "現場業務"];
</script>
