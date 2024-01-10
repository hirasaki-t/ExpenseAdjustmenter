<template>
    <el-button type="success" :loading="props.loading" @click="$emit('download')">CSVダウンロード</el-button>
</template>

<script setup lang="ts">
import { ElButton } from "element-plus";

defineEmits<{ (event: "download"): void }>();
const props = defineProps<{ loading: boolean }>();
</script>
