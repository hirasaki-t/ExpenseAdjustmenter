<template>
    <el-popover trigger="click" :width="props.width">
        <template #reference>
            <span>
                {{ props.title }}
                <el-button link :icon="Filter" />
            </span>
        </template>
        <slot />
    </el-popover>
</template>

<script setup lang="ts">
import { ElPopover, ElButton } from "element-plus";
import { Filter } from '@element-plus/icons-vue'

const props = defineProps<{ title: string, width: string }>();
defineEmits<{ (event: "update:modelValue", modelValue?: string): void }>();
</script>