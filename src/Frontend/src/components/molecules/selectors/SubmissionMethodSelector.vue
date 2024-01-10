<template>
  <el-radio-group :model-value="modelValue" @update:model-value="(x) => $emit('update:model-value', x)">
    <el-radio label="紙/持参">紙/持参</el-radio>
    <el-radio label="紙/郵送">紙/郵送</el-radio>
    <el-radio label="電子">電子</el-radio>
  </el-radio-group>
</template>

<script setup lang="ts">
import type { SubmissionMethod } from "@/types";
import { ElRadioGroup, ElRadio } from "element-plus";

defineProps<{ modelValue?: SubmissionMethod }>();
defineEmits<{ (event: "update:model-value", modelValue?: SubmissionMethod): void }>();
</script>
