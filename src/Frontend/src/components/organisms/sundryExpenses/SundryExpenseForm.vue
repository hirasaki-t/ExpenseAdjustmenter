<template>
  <el-form
    ref="formRef"
    :model="formValue"
    :rules="sundryExpenseRules"
    label-placement="left"
    label-width="120px"
    style="width: 400px"
  >
    <el-form-item label="日付" prop="date">
      <date-picker v-model="formValue.date" :clearable="false" />
    </el-form-item>
    <el-form-item label="経費種別" prop="expenseTypeId">
      <div style="display: flex; flex-direction: column">
        <expense-type-selector :expense-types="expenseTypes" v-model="formValue.expenseTypeId" />
        <span v-if="formValue.expenseTypeId" style="color: tomato">
          {{ expenseTypes.find((x) => x.id === formValue.expenseTypeId)?.details }}
        </span>
      </div>
    </el-form-item>
    <el-form-item label="詳細" prop="details">
      <el-input v-model="formValue.details" />
    </el-form-item>
    <el-form-item label="人数" prop="participationNumber">
      <el-input v-model="formValue.participationNumber" input-style="text-align:right" style="width: 150px" />
      人
    </el-form-item>
    <el-form-item label="金額" prop="amount">
      <el-input v-model="formValue.amount" input-style="text-align:right" style="width: 150px" />
      円
    </el-form-item>
    <el-form-item
      label="領収書提出方法"
      prop="submissionMethod"
      v-if="expenseTypes.find((x) => x.id === formValue.expenseTypeId)?.isReceipt"
    >
      <submission-method-selector
        :model-value="formValue.submissionMethod ?? undefined"
        @update:model-value="(x) => (formValue.submissionMethod = x ?? null)"
      />
    </el-form-item>
    <el-form-item label="領収書" prop="receipt" v-if="formValue.submissionMethod === '電子'">
      <file-upload @update-file="(file) => (formValue.receipt = file)" />
    </el-form-item>
  </el-form>
</template>

<script setup lang="ts">
import type { SundryExpense } from "@/types";
import { reactive, ref, watch, watchEffect } from "vue";
import { sundryExpenseRules } from "../../../validators/sundryExpenseRules";
import type { FormInstance } from "element-plus";
import DatePicker from "@/components/molecules/pickers/DatePicker.vue";
import ExpenseTypeSelector from "@/components/molecules/selectors/ExpenseTypeSelector.vue";
import type { ExpenseType } from "@/types";
import FileUpload from "@/components/molecules/FileUpload.vue";
import SubmissionMethodSelector from "../../molecules/selectors/SubmissionMethodSelector.vue";

const props = defineProps<{ modelValue: Omit<SundryExpense, "id">; expenseTypes: ExpenseType[] }>();
const emits = defineEmits<{ (event: "update:model-value", modelValue: Omit<SundryExpense, "id">): void }>();

const formRef = ref<FormInstance>();
const formValue = reactive<Omit<SundryExpense, "id">>(props.modelValue);

watchEffect(() => emits("update:model-value", { ...formValue }));

watch(
  () => props.modelValue,
  (x) => {
    if (formValue.date.getTime() !== x.date.getTime()) formValue.date = x.date;
    if (formValue.expenseTypeId !== x.expenseTypeId) formValue.expenseTypeId = x.expenseTypeId;
    if (formValue.submissionMethod !== x.submissionMethod) formValue.submissionMethod = x.submissionMethod;
    if (formValue.details !== x.details) formValue.details = x.details;
    if (formValue.participationNumber !== x.participationNumber) formValue.participationNumber = x.participationNumber;
    if (formValue.receipt !== x.receipt) formValue.receipt = x.receipt;
    if (formValue.amount !== x.amount) formValue.amount = x.amount;
    if (formValue.status !== x.status) formValue.status = x.status;
  }
);

function validate() {
  if (!formRef.value) return undefined;
  return formRef.value.validate();
}

watch(
  () => formValue.expenseTypeId,
  () => {
    formValue.submissionMethod = null;
    formValue.receipt = null;
  }
);

defineExpose({ validate: validate });
</script>
