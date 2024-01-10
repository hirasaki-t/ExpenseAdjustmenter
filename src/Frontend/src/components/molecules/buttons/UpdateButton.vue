<template>
  <el-button type="primary" :loading="loading" :disabled="disabled" @click="() => $emit('update')">更新</el-button>
</template>

<script setup lang="ts">
import { ElButton } from "element-plus";
defineEmits<{ (event: "update"): void }>();
defineProps<{ loading?: boolean; disabled?: boolean }>();
</script>
