<template>
    <el-dialog
          :model-value="props.show"
          :title="`CSVダウンロード(${ props.date.getFullYear() }年${ props.date.getMonth() + 1 }月分)`"
          append-to-body
          :width=400
          @update:model-value="emit('complete')"
      >
      <csv-download-form :date="props.date" />
    </el-dialog>
  </template>
  
<script setup lang="ts">
import { ElDialog } from "element-plus";
import CsvDownloadForm from "./CsvDownloadForm.vue";

const props = defineProps<{ date: Date, show: boolean; disabled?: boolean }>();
const emit = defineEmits<{ (event: "complete"): void }>();
</script>