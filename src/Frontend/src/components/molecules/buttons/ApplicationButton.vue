<template>
  <el-popconfirm title="申請してよろしいですか？" width="350px" @confirm="() => $emit('application')">
    <template #reference>
      <el-button type="success" :loading="loading" :disabled="disabled">申請する</el-button>
    </template>
  </el-popconfirm>
</template>

<script setup lang="ts">
import { ElButton, ElPopconfirm } from "element-plus";

defineEmits<{ (event: "application"): void }>();
defineProps<{ loading?: boolean; disabled?: boolean }>();
</script>
