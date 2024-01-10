<template>
    <el-select
        :model-value="props.modelValue"
        placeholder="区分選択"
        :clearable="props.clearable"
        @update:model-value="$emit('update:modelValue', $event)"
    >
    <el-option v-for="category in categories" :key="category.id" :label="category.name" :value="category.id" />
    </el-select>
</template>

<script setup lang="ts">
import { ElSelect, ElOption } from "element-plus";
import { computed } from "vue";
import { useCategoryStore } from "../../../stores/categoryStore";
const categories = computed(() => useCategoryStore().orderedCategories);
const props = defineProps<{ modelValue?: string, clearable?: boolean }>();
defineEmits<{ (event: "update:modelValue", modelValue?: string): void }>();
</script>