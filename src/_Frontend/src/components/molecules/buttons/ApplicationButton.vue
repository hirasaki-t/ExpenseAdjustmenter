<template>
    <el-popconfirm title="選択した経費を申請してよろしいですか？" width="350px" @confirm="$emit('application')">
        <template #reference>
            <el-button type="success" :loading="props.loading" :disabled="props.disable"> 申請する </el-button>
        </template>
    </el-popconfirm>
</template>

<script setup lang="ts">
import { ElButton, ElPopconfirm } from "element-plus";

defineEmits<{ (event: "application"): void }>();
const props = defineProps<{ loading: boolean, disable?: boolean }>();
</script>
