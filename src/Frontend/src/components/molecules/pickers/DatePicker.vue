<template>
  <el-date-picker
    type="date"
    format="YYYY年MM月DD日"
    :model-value="modelValue"
    style="width: 150px"
    :clearable="clearable"
    :placeholder="placeholder"
    @update:model-value="(x) => $emit('update:model-value', x)"
  />
</template>

<script setup lang="ts">
defineProps<{ modelValue?: Date; clearable?: boolean; placeholder?: string }>();
defineEmits<{ (event: "update:model-value", modelValue?: Date): void }>();
</script>
