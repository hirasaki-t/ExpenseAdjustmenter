<template>
    <el-form ref="formRef" :model="formValue" :rules="traveringExpenseRules" label-placement="left" label-width="120px">
        <stack-container style="display: flex; flex-direction: column">
            <el-form-item label="日付" prop="date">
                <el-date-picker type="date" placeholder="日付" format="YYYY年MM月DD日" v-model="formValue.date" style="width:150px" />
            </el-form-item>
            <el-form-item label="業務名" prop="workName">
                <work-name-selector v-model="formValue.workName" style="width:150px" />
            </el-form-item>
            <el-form-item label="区間">
                <div style="display:flex; gap:10px">
                    <el-input placeholder="出発地" v-model="formValue.startSection" style="width:100px" />
                    →
                    <el-input placeholder="到着地" v-model="formValue.endSection" style="width:100px" />
                </div>
            </el-form-item>
            <el-form-item label="交通区分" prop="categoryId">
                <category-selector v-model="formValue.categoryId" style="width:150px" />
            </el-form-item>
            <el-form-item label="領収書提出方法" prop="submissionMethod" v-if="categoryDictionary[formValue.categoryId].isReceipt">
                <submission-method-radio-button v-model="formValue.submissionMethod" />
            </el-form-item>
            <el-form-item label="領収書" prop="receipt" v-if="formValue.submissionMethod ==='電子'">
                <file-upload @update-file="(file) => formValue.receipt = file" :exist-file="!!formValue.receiptId" />
            </el-form-item>
            <el-form-item label="金額" prop="amount">
                <el-input input-style="text-align:right" v-model="formValue.amount" style="width:150px"  />
                  円
            </el-form-item>
            <el-form-item label="摘要">
                <el-input v-model="formValue.remarks" style="width:300px" />
            </el-form-item>
        </stack-container>
        <div style="display: flex; justify-content: flex-end">
            <update-button :loading="loading" @update="update" />
        </div>
    </el-form>
</template>

<script setup lang="ts">
import { ElForm, ElFormItem, FormInstance, ElDatePicker, ElInput } from "element-plus";
import { reactive, ref, watch } from "vue";
import StackContainer from "../../atomos/StackContainer.vue";
import UpdateButton from "../../molecules/buttons/UpdateButton.vue";
import WorkNameSelector from "../../molecules/selectors/WorkNameSelector.vue";
import CategorySelector from "../../molecules/selectors/CategorySelector.vue";
import { traveringExpenseRules } from "../../../validators/traveringExpenseRules";
import { useCategoryStore } from "../../../stores/categoryStore";
import { useTraveringExpenseStore } from "../../../stores/traveringExpenseStore";
import SubmissionMethodRadioButton from "../SubmissionMethodRadioButton.vue";
import FileUpload from "../../molecules/FileUpload.vue";

const loading = ref(false);
const emit = defineEmits<{ (event: "complete"): void }>();

const props = defineProps<{ id: string }>();
const traveringExpenseStore = useTraveringExpenseStore();
const traveringExpense = traveringExpenseStore.dictionary[props.id];

const categorySotre = useCategoryStore();
const categoryDictionary = categorySotre.dictionary;

const formRef = ref<FormInstance>();
const formValue = reactive({
    date: traveringExpense?.date,
    workName: traveringExpense?.workName,
    startSection: traveringExpense?.startSection,
    endSection: traveringExpense?.endSection,
    categoryId: traveringExpense?.categoryId,
    submissionMethod: traveringExpense?.submissionMethod,
    receiptId: traveringExpense?.receiptId,
    receipt: ref<File | null>(),
    amount: traveringExpense?.amount,
    remarks: traveringExpense?.remarks
});

async function update() {
    if (!formRef.value) return;
    formRef.value.validate(async (valid) => {
        if (valid) {
            loading.value = true;
            try {
                await traveringExpenseStore.update(
                    props.id,
                    formValue.date,
                    formValue.workName,
                    formValue.startSection,
                    formValue.endSection,
                    formValue.categoryId,
                    formValue.submissionMethod,
                    formValue.receipt ?? null,
                    formValue.amount,
                    formValue.remarks
                );
                emit("complete");
            } finally {
                loading.value = false;
            }
        }
    });
}

watch(
    () => formValue.categoryId,
    () => formValue.submissionMethod = null
);
</script>
