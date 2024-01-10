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
                    <span v-if="formValue.isGoBack">
                        ⇆
                    </span>
                    <span v-else>
                        →
                    </span>
                    <el-input placeholder="到着地" v-model="formValue.endSection" style="width:100px" />
                    <el-checkbox v-model="formValue.isGoBack">往復</el-checkbox>
                </div>
            </el-form-item>
            <el-form-item label="交通区分" prop="categoryId">
                <category-selector v-model="formValue.categoryId" style="width:150px" />
            </el-form-item>
            <el-form-item label="領収書提出方法" prop="submissionMethod" v-if="categoryDictionary[formValue.categoryId].isReceipt">
                <submission-method-radio-button v-model="formValue.submissionMethod" />
            </el-form-item>
            <el-form-item label="領収書" prop="receipt" v-if="formValue.submissionMethod ==='電子'">
                <file-upload @update-file="(file) => formValue.receipt = file" />
            </el-form-item>
            <el-form-item label="金額" prop="amount">
                <div style="display: flex; flex-direction: column;">
                    <div>
                        <el-input v-model="formValue.amount" :controls="false" input-style="text-align:right" style="width:150px;"  />
                      円
                    </div>
                    <span v-if="formValue.isGoBack" style="color:tomato;">
                        往復分を追加する際は片道の金額を入力してください。
                    </span>
                </div>
            </el-form-item>
            <el-form-item label="摘要">
                <el-input style="width:250px" v-model="formValue.remarks" />
            </el-form-item>
        </stack-container>
        <div style="display: flex; justify-content:space-between">
            <template-selector :show="templateDialogShow" />
            <add-button :loading="loading" @add="add" />
        </div>
    </el-form>
</template>

<script setup lang="ts">
import { ElForm, ElFormItem, FormInstance, ElDatePicker, ElInput, ElCheckbox } from "element-plus";
import { reactive, ref, watch } from "vue";
import StackContainer from "../../atomos/StackContainer.vue";
import AddButton from "../../molecules/buttons/AddButton.vue";
import WorkNameSelector from "../../molecules/selectors/WorkNameSelector.vue";
import CategorySelector from "../../molecules/selectors/CategorySelector.vue";
import { traveringExpenseRules } from "../../../validators/traveringExpenseRules";
import { useCategoryStore } from "../../../stores/categoryStore";
import FileUpload from "../../molecules/FileUpload.vue";
import SubmissionMethodRadioButton from "../SubmissionMethodRadioButton.vue";
import { useTraveringExpenseStore } from "../../../stores/traveringExpenseStore";
import TemplateSelector from "../traveringExpenseTemplates/Selector.vue";

const loading = ref(false);
const templateDialogShow = ref(false);
const emit = defineEmits<{ (event: "complete"): void }>();

const traveringExpenseStore = useTraveringExpenseStore();
const categorySotre = useCategoryStore();
const categoryDictionary = categorySotre.dictionary;
const categories = categorySotre.orderedCategories;

const formRef = ref<FormInstance>();
const formValue = reactive<{ 
    date: Date, 
    workName: string, 
    startSection: string | null, 
    endSection: string | null, 
    isGoBack: boolean, 
    categoryId: string, 
    submissionMethod: string | null,
    receipt: File | null,
    amount: number, 
    remarks: string | null}>({
        date: new Date(),
        workName: "社内業務",
        startSection: null,
        endSection: null,
        isGoBack: false,
        categoryId: categories.find(x => x)?.id!,
        submissionMethod: null,
        receipt: null,
        amount: 0,
        remarks: null,
});

async function add() {
    if (!formRef.value) return;
    formRef.value.validate(async (valid) => {
        if (valid) {
            loading.value = true;
            try {
                await traveringExpenseStore.add(
                    formValue.date, 
                    formValue.workName, 
                    formValue.startSection, 
                    formValue.endSection, 
                    formValue.categoryId, 
                    formValue.submissionMethod, 
                    formValue.receipt, 
                    formValue.amount, 
                    formValue.remarks);

                    if (formValue.isGoBack) {
                        await traveringExpenseStore.add(
                            formValue.date, 
                            formValue.workName, 
                            formValue.endSection, 
                            formValue.startSection, 
                            formValue.categoryId, 
                            formValue.submissionMethod, 
                            formValue.receipt, 
                            formValue.amount, 
                            formValue.remarks);
                    }
                formValue.date = new Date();
                formValue.workName = "社内業務";
                formValue.startSection = null;
                formValue.endSection = null;
                formValue.isGoBack = false;
                formValue.categoryId = categories.find(x => x)?.id!;
                formValue.receipt = null;
                formValue.amount = 0;
                formValue.remarks = null;
                emit("complete");
            } finally {
                loading.value = false;
            }
        }
    });
}

watch(
    () => formValue.categoryId,
    (categoryId) => {
        if (categoryDictionary[categoryId].isReceipt) formValue.isGoBack = false;
    }
);

watch(
    () => formValue.categoryId,
    () => formValue.submissionMethod = null
);

</script>
