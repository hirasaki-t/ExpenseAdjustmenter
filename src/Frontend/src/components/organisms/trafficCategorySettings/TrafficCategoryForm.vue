<template>
  <el-form ref="formRef" :model="formValue" label-placement="left" label-width="100px">
    <el-form-item label="区分">
      <el-input v-model="formValue.name" style="width: 200px" />
    </el-form-item>
    <el-form-item label="説明">
      <el-input v-model="formValue.details" style="width: 200px" />
    </el-form-item>
    <el-form-item label="領収書">
      <el-checkbox v-model="formValue.isReceipt" />
    </el-form-item>
  </el-form>
</template>

<script setup lang="ts">
import type { TrafficCategory } from "@/types";
import { ElForm, ElFormItem, ElInput, ElCheckbox, type FormInstance } from "element-plus";
import { reactive, ref, watch, watchEffect } from "vue";

const props = defineProps<{ modelValue: Omit<TrafficCategory, "id"> }>();
const emit = defineEmits<{ (event: "update:model-value", user: Omit<TrafficCategory, "id">): void }>();

const formRef = ref<FormInstance>();
const formValue = reactive<Omit<TrafficCategory, "id">>(props.modelValue);

function validate() {
  if (!formRef.value) return undefined;
  return formRef.value.validate();
}

watch(
  () => props.modelValue,
  (x) => {
    if (formValue.name !== x.name) formValue.name = x.name;
    if (formValue.details !== x.details) formValue.details = x.details;
    if (formValue.isReceipt !== x.isReceipt) formValue.isReceipt = x.isReceipt;
    if (formValue.isActive !== x.isActive) formValue.isActive = x.isActive;
  }
);

watchEffect(() => {
  emit("update:model-value", { ...formValue });
});

defineExpose({ validate: validate });
</script>
