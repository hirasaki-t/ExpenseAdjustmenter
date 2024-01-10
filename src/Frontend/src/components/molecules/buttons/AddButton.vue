<template>
  <el-button type="primary" :loading="loading" :disabled="disabled" @click="() => $emit('add')">追加</el-button>
</template>

<script setup lang="ts">
import { ElButton } from "element-plus";

defineEmits<{ (event: "add"): void }>();
defineProps<{ loading?: boolean; disabled?: boolean }>();
</script>
