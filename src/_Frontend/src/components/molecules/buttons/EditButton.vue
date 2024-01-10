<template>
    <el-button :loading="props.loading" :disabled="props.disabled" @click="$emit('edit')">
        編集
    </el-button>
</template>

<script setup lang="ts">
import { ElButton } from "element-plus";

defineEmits<{ (event: "edit"): void }>();
const props = defineProps<{ loading: boolean; disabled?: boolean }>();
</script>
