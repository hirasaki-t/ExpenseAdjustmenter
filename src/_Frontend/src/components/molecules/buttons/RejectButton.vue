<template>
    <el-button type="danger" :loading="props.loading" :disabled="props.disabled" @click="$emit('reject')">
        否認
    </el-button>
</template>

<script setup lang="ts">
import { ElButton } from "element-plus";

defineEmits<{ (event: "reject"): void }>();
const props = defineProps<{ loading: boolean; disabled?: boolean }>();
</script>
