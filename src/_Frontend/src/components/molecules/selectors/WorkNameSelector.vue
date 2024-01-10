<template>
    <el-select
        :model-value="props.modelValue"
        placeholder="業務名選択"
        :clearable="props.clearable"
        @update:model-value="$emit('update:modelValue', $event)"
    >
        <el-option v-for="item in types" :key="item" :label="item" :value="item" />
    </el-select>
</template>

<script setup lang="ts">
import { ElSelect, ElOption } from "element-plus";
const types = ["社内業務", "営業", "現場業務"];
const props = defineProps<{ modelValue?: string, clearable?: boolean }>();
defineEmits<{ (event: "update:modelValue", modelValue?: string): void }>();
</script>