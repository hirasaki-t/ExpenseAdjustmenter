<template>
  <el-popconfirm title="削除してよろしいですか？" width="250px" @confirm="() => $emit('delete')">
    <template #reference>
      <el-button type="danger" :loading="loading" :disabled="disabled">削除</el-button>
    </template>
  </el-popconfirm>
</template>

<script setup lang="ts">
import { ElButton, ElPopconfirm } from "element-plus";

defineEmits<{ (event: "delete"): void }>();
defineProps<{ loading?: boolean; disabled?: boolean }>();
</script>
