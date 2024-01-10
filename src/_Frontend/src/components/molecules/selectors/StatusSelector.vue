<template>
    <el-select
        :model-value="props.modelValue"
        placeholder="ステータス"
        :clearable="props.clearable"
        @update:model-value="$emit('update:modelValue', $event)"
    >
        <el-option v-for="item in types" :key="item" :label="item" :value="item" />
    </el-select>
</template>

<script setup lang="ts">
import { ElSelect, ElOption } from "element-plus";
const types = ["申請中", "承認", "否認"];
const props = defineProps<{ modelValue?: string, clearable?: boolean }>();
defineEmits<{ (event: "update:modelValue", modelValue?: string): void }>();
</script>