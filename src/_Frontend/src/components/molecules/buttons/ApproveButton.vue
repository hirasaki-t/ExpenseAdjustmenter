<template>
    <el-button type="primary" :loading="props.loading" :disabled="props.disabled" @click="$emit('pending')">
        承認
    </el-button>
</template>

<script setup lang="ts">
import { ElButton } from "element-plus";

defineEmits<{ (event: "pending"): void }>();
const props = defineProps<{ loading: boolean; disabled?: boolean }>();
</script>
