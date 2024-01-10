<template>
  <el-select
    :model-value="modelValue"
    placeholder="承認ステータス"
    :clearable="clearable"
    @update:model-value="(x) => $emit('update:modelValue', x)"
  >
    <el-option v-for="status in approveStatuses" :key="status" :label="status" :value="status" />
  </el-select>
</template>

<script setup lang="ts">
import type { ApproveStatus } from "@/types";
import { ElSelect, ElOption } from "element-plus";

defineProps<{ modelValue?: ApproveStatus; clearable?: boolean }>();
defineEmits<{ (event: "update:model-value", modelValue?: ApproveStatus): void }>();

const approveStatuses: ApproveStatus[] = ["申請中", "承認", "否認"];
</script>
