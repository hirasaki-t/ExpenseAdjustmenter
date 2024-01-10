<template>
    <el-button type="primary" :disabled="props.disabled" @click="$emit('update:show', !props.show)">
        {{ `${props.resourceName}追加` }}
    </el-button>
    <el-dialog
        :model-value="props.show"
        :title="`${props.resourceName}追加`"
        append-to-body
        :width="props.width"
        @update:model-value="$emit('update:show', $event)"
    >
        <slot />
    </el-dialog>
</template>

<script setup lang="ts">
import { ElButton, ElDialog } from "element-plus";

const props = defineProps<{ resourceName: string; show: boolean; width: number; disabled?: boolean }>();
defineEmits<{ (event: "update:show", show: boolean): void }>();
</script>
