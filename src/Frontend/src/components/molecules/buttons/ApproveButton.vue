<template>
  <el-button type="primary" :loading="loading" :disabled="disabled" @click="() => $emit('pending')">承認</el-button>
</template>

<script setup lang="ts">
import { ElButton } from "element-plus";

defineEmits<{ (event: "pending"): void }>();
defineProps<{ loading?: boolean; disabled?: boolean }>();
</script>
