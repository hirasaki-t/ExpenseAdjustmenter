<template>
    <el-select
        :model-value="props.modelValue"
        placeholder="経費種別選択"
        :clearable="props.clearable"
        @update:model-value="$emit('update:modelValue', $event)"
    >
        <el-option v-for="expenseType in expenseTypes" :key="expenseType.id" :label="expenseType.name" :value="expenseType.id" />
    </el-select>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { ElSelect, ElOption } from "element-plus";
import { useExpenseTypeStore } from "../../../stores/expenseTypeStore";
const expenseTypes = computed(() => useExpenseTypeStore().orderedExpenseTypes);
const props = defineProps<{ modelValue?: string, clearable?: boolean }>();
defineEmits<{ (event: "update:modelValue", modelValue?: string): void }>();
</script>