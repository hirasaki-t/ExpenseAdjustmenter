<template>
    <el-button type="primary" :loading="props.loading" @click="$emit('update')"> 更新 </el-button>
</template>

<script setup lang="ts">
import { ElButton } from "element-plus";
defineEmits<{ (event: "update"): void }>();
const props = defineProps<{ loading: boolean }>();
</script>