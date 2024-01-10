<template>
  <el-button :loading="loading" :disabled="disabled" @click="() => $emit('edit')">編集</el-button>
</template>

<script setup lang="ts">
import { ElButton } from "element-plus";

defineEmits<{ (event: "edit"): void }>();
defineProps<{ loading?: boolean; disabled?: boolean }>();
</script>
