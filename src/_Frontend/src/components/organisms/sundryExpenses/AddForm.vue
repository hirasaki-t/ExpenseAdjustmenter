<template>
    <el-form ref="formRef" :model="formValue" :rules="sundryExpenseRules" label-placement="left" label-width="120px">
        <stack-container style="display: flex; flex-direction: column">
            <el-form-item label="日付" prop="date">
                <el-date-picker type="date" placeholder="日付" format="YYYY年MM月DD日" v-model="formValue.date" style="width:150px" />
            </el-form-item>
            <el-form-item label="経費種別" prop="expenseTypeId">
                <div style="display: flex; flex-direction: column;">
                    <expense-type-selector v-model="formValue.expenseTypeId" style="width:150px" />
                    <span v-if="!!formValue.expenseTypeId" style="color:tomato;">
                        {{expenseTypeDictionary[formValue.expenseTypeId]?.details}}
                    </span>
                </div>
            </el-form-item>
            <el-form-item label="詳細" prop="details">
                <el-input v-model="formValue.details" style="width:300px" />
            </el-form-item>
            <el-form-item label="人数" prop="participationNumber">
                <el-input v-model="formValue.participationNumber" input-style="text-align:right" style="width:150px" />
                  人
            </el-form-item>
            <el-form-item label="金額" prop="amount">
                <el-input v-model="formValue.amount" input-style="text-align:right" style="width:150px;" />
                  円
            </el-form-item>
            <el-form-item label="領収書提出方法" prop="submissionMethod" v-if="expenseTypeDictionary[formValue.expenseTypeId]?.isReceipt">
                <submission-method-radio-button v-model="formValue.submissionMethod" />
            </el-form-item>
            <el-form-item label="領収書" prop="receipt" v-if="formValue.submissionMethod ==='電子'" >
                <file-upload @update-file="(file) => formValue.receipt = file" />
            </el-form-item>
        </stack-container>
        <div style="display: flex; justify-content: flex-end">
            <add-button :loading="loading" @add="add" />
        </div>
    </el-form>
</template>

<script setup lang="ts">
import { ElForm, ElFormItem, FormInstance, ElInput, ElDatePicker } from "element-plus";
import { reactive, ref, watch } from "vue";
import { useSundryExpenseStore } from "../../../stores/sundryExpenseStore";
import StackContainer from "../../atomos/StackContainer.vue";
import AddButton from "../../molecules/buttons/AddButton.vue";
import ExpenseTypeSelector from "../../molecules/selectors/ExpenseTypeSelector.vue";
import { useExpenseTypeStore } from "../../../stores/expenseTypeStore";
import FileUpload from "../../molecules/FileUpload.vue";
import SubmissionMethodRadioButton from "../SubmissionMethodRadioButton.vue";
import { sundryExpenseRules } from "../../../validators/sundryExpenseRules";

const loading = ref(false);
const emit = defineEmits<{ (event: "complete"): void }>();

const formRef = ref<FormInstance>();
const formValue = reactive<{ 
    date: Date, 
    expenseTypeId: string, 
    submissionMethod: string | null,
    details: string | null, 
    participationNumber: number, 
    receipt: File | null, 
    amount: number, 
    status: string | null }>({
        date: new Date(),
        expenseTypeId: "",
        submissionMethod: null,
        details: null,
        participationNumber: 1,
        receipt: null,
        amount: 0,
        status: null,
});

const expenseTypeStore = useExpenseTypeStore();
const sundryExpenseStore = useSundryExpenseStore();

const expenseTypeDictionary = expenseTypeStore.dictionary;

async function add() {
    if (!formRef.value) return;
    formRef.value.validate(async (valid) => {
        if (valid) {
            loading.value = true;
            await sundryExpenseStore.add(
                formValue.date,
                formValue.expenseTypeId,
                formValue.details,
                formValue.participationNumber,
                formValue.amount,
                formValue.submissionMethod,
                formValue.receipt
            );
            formValue.date = new Date();
            formValue.expenseTypeId= "1";
            formValue.submissionMethod = null;
            formValue.details = null;
            formValue.participationNumber= 0,
            formValue.receipt = null;
            formValue.amount = 0;
            formValue.status = null;
            try {
                emit("complete");
            } finally {
                loading.value = false;
            }
        }
    });
}

watch(
    () => formValue.expenseTypeId,
    () => formValue.submissionMethod = null
);
</script>
