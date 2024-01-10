<template>
    <el-dialog
          :model-value="props.show"
          title="承認履歴"
          append-to-body
          :width=400
          @update:model-value="emit('complete')"
      >
        <Suspense>
            <template #default>
                <history-form :id="props.id" :disabled="props.disabled" :show="props.show" />
            </template>
            <template #fallback> 
                <loading />
            </template>
        </Suspense>
    </el-dialog>
  </template>
  
<script setup lang="ts">
import { ElDialog } from "element-plus";
import HistoryForm from "./HistoryForm.vue";
import Loading from "../../atomos/Loading.vue";

const props = defineProps<{ id: string, show: boolean; disabled?: boolean }>();
const emit = defineEmits<{ (event: "complete"): void }>();
</script>