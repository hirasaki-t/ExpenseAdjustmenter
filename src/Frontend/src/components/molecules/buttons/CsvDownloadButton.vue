<template>
  <el-button type="success" :loading="loading" :disabled="disabled" @click="() => $emit('download')">
    CSVダウンロード
  </el-button>
</template>

<script setup lang="ts">
import { ElButton } from "element-plus";

defineEmits<{ (event: "download"): void }>();
defineProps<{ loading?: boolean; disabled?: boolean }>();
</script>
