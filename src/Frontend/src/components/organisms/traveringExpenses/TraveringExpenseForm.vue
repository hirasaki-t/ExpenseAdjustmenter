<template>
  <el-form
    ref="formRef"
    :model="formValue"
    :rules="traveringExpenseRules"
    label-placement="left"
    label-width="120px"
    style="width: 450px"
  >
    <el-form-item label="日付" prop="date">
      <date-picker v-model="formValue.date" :clearable="false" />
    </el-form-item>
    <el-form-item label="業務名" prop="workName">
      <work-name-selector v-model="formValue.workName" style="width: 150px" />
    </el-form-item>
    <el-form-item label="区間">
      <div style="display: flex; gap: 10px">
        <el-input placeholder="出発地" v-model="formValue.startSection" style="width: 100px" />
        <span v-if="formValue.isGoBack"> ⇆ </span>
        <span v-else> → </span>
        <el-input placeholder="到着地" v-model="formValue.endSection" style="width: 100px" />
        <el-checkbox v-if="formValue.isGoBack !== undefined" v-model="formValue.isGoBack" :disabled="needReceipt">
          往復
        </el-checkbox>
      </div>
    </el-form-item>
    <el-form-item label="交通区分" prop="categoryId">
      <traffic-category-selector
        v-model="formValue.categoryId"
        style="width: 150px"
        :traffic-categories="trafficCategories"
      />
    </el-form-item>
    <el-form-item label="領収書提出方法" prop="submissionMethod" v-if="needReceipt">
      <submission-method-selector
        :model-value="formValue.submissionMethod ?? undefined"
        @update:model-value="(x) => (formValue.submissionMethod = x ?? null)"
      />
    </el-form-item>
    <el-form-item label="領収書" prop="receipt" v-if="formValue.submissionMethod === '電子'">
      <file-upload @update-file="(file) => (formValue.receipt = file)" />
    </el-form-item>
    <el-form-item label="金額" prop="amount">
      <div style="display: flex; flex-direction: column">
        <div>
          <el-input v-model="formValue.amount" :controls="false" input-style="text-align:right" style="width: 150px" />
          円
        </div>
        <span v-if="formValue.isGoBack" style="color: tomato">
          往復分を追加する際は片道の金額を入力してください。
        </span>
      </div>
    </el-form-item>
    <el-form-item label="摘要">
      <el-input style="width: 250px" v-model="formValue.remarks" />
    </el-form-item>
  </el-form>
</template>

<script setup lang="ts">
import { ElForm, ElFormItem, type FormInstance, ElInput, ElCheckbox } from "element-plus";
import { reactive, ref, watch, watchEffect } from "vue";
import WorkNameSelector from "../../molecules/selectors/WorkNameSelector.vue";
import TrafficCategorySelector from "../../molecules/selectors/TrafficCategorySelector.vue";
import { traveringExpenseRules } from "../../../validators/traveringExpenseRules";
import FileUpload from "../../molecules/FileUpload.vue";
import SubmissionMethodSelector from "../../molecules/selectors/SubmissionMethodSelector.vue";
import type { TrafficCategory, TraveringExpense } from "@/types";
import { computed } from "vue";
import DatePicker from "@/components/molecules/pickers/DatePicker.vue";

const props = defineProps<{
  loading?: boolean;
  trafficCategories: TrafficCategory[];
  modelValue: Omit<TraveringExpense, "id"> & { isGoBack?: boolean };
}>();
const emits = defineEmits<{
  (event: "update:model-value", modelValue: Omit<TraveringExpense, "id"> & { isGoBack?: boolean }): void;
}>();

const formRef = ref<FormInstance>();
const formValue = reactive<Omit<TraveringExpense, "id"> & { isGoBack?: boolean }>(props.modelValue);

const needReceipt = computed(
  () => props.trafficCategories.find((x) => x.id === formValue.categoryId)?.isReceipt ?? false
);

watchEffect(() => {
  emits("update:model-value", { ...formValue });
});

watch(
  () => props.modelValue,
  (x) => {
    if (formValue.date.getTime() !== x.date.getTime()) formValue.date = x.date;
    if (formValue.workName !== x.workName) formValue.workName = x.workName;
    if (formValue.startSection !== x.startSection) formValue.startSection = x.startSection;
    if (formValue.endSection !== x.endSection) formValue.endSection = x.endSection;
    if (formValue.isGoBack !== x.isGoBack) formValue.isGoBack = x.isGoBack;
    if (formValue.categoryId !== x.categoryId) formValue.categoryId = x.categoryId;
    if (formValue.submissionMethod !== x.submissionMethod) formValue.submissionMethod = x.submissionMethod;
    if (formValue.receipt !== x.receipt) formValue.receipt = x.receipt;
    if (formValue.amount !== x.amount) formValue.amount = x.amount;
    if (formValue.remarks !== x.remarks) formValue.remarks = x.remarks;
  }
);

watch(
  () => formValue.categoryId,
  () => (formValue.submissionMethod = null)
);

function validate() {
  if (!formRef.value) return undefined;
  return formRef.value.validate();
}

defineExpose({ validate: validate });
</script>
