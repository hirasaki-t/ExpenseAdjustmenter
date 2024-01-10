<template>
  <el-button type="danger" :loading="loading" :disabled="disabled" @click="() => $emit('reject')">否認</el-button>
</template>

<script setup lang="ts">
import { ElButton } from "element-plus";

defineEmits<{ (event: "reject"): void }>();
defineProps<{ loading?: boolean; disabled?: boolean }>();
</script>
