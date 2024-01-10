<template>
  <el-form ref="formRef" :model="formValue" label-placement="left" label-width="120px">
    <el-form-item label="氏名">
      {{ formValue.name }}
    </el-form-item>
    <el-form-item label="メールアドレス">
      <el-input v-model="formValue.mail" style="width: 200px" v-if="!isUpdate" />
      <span v-else>
        {{ formValue.mail }}
      </span>
    </el-form-item>
    <el-form-item label="管理者">
      <el-checkbox v-model="formValue.isAdmin" />
    </el-form-item>
  </el-form>
</template>

<script setup lang="ts">
import { ElForm, ElFormItem, ElInput, ElCheckbox, type FormInstance } from "element-plus";
import { reactive, ref, watch, watchEffect } from "vue";
import type { User } from "@/types";

const props = defineProps<{
  modelValue: Omit<User, "id">;
  isUpdate?: boolean;
}>();
const emit = defineEmits<{ (event: "update", user: Omit<User, "id">): void }>();

const formRef = ref<FormInstance>();
const formValue = reactive<Omit<User, "id">>(props.modelValue);

function validate() {
  if (!formRef.value) return undefined;
  return formRef.value.validate();
}

watch(
  () => props.modelValue,
  (x) => {
    if (formValue.name !== x.name) formValue.name = x.name;
    if (formValue.mail !== x.mail) formValue.mail = x.mail;
    if (formValue.isAdmin !== x.isAdmin) formValue.isAdmin = x.isAdmin;
    if (formValue.isActive !== x.isActive) formValue.isActive = x.isActive;
  }
);

watchEffect(() => {
  emit("update", { ...formValue });
});

defineExpose({ validate: validate });
</script>
