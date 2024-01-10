<template>
    <el-button type="primary" :loading="props.loading" :disabled="props.disabled" @click="$emit('add')">
        追加
    </el-button>
</template>

<script setup lang="ts">
import { ElButton } from "element-plus";

defineEmits<{ (event: "add"): void }>();
const props = defineProps<{ loading: boolean; disabled?: boolean }>();
</script>
