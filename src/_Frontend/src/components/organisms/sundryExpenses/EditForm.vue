<template>
    <el-form ref="formRef" :model="formValue" :rules="sundryExpenseRules" label-placement="left" label-width="130px">
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
            <el-form-item label="領収書" prop="receipt" v-if="formValue.submissionMethod ==='電子'">
                <file-upload @update-file="(file) => formValue.receipt = file" :exist-file="!!formValue.receiptId" />
            </el-form-item>
        </stack-container>
        <div style="display: flex; justify-content: flex-end">
            <update-button :loading="loading" @update="update" />
        </div>
    </el-form>
</template>

<script setup lang="ts">
import { ElForm, ElFormItem, FormInstance, ElInput, ElDatePicker } from "element-plus";
import { reactive, ref, watch } from "vue";
import { useSundryExpenseStore } from "../../../stores/sundryExpenseStore";
import StackContainer from "../../atomos/StackContainer.vue";
import UpdateButton from "../../molecules/buttons/UpdateButton.vue";
import ExpenseTypeSelector from "../../molecules/selectors/ExpenseTypeSelector.vue";
import { useExpenseTypeStore } from "../../../stores/expenseTypeStore";
import FileUpload from "../../molecules/FileUpload.vue";
import SubmissionMethodRadioButton from "../SubmissionMethodRadioButton.vue";
import { sundryExpenseRules } from "../../../validators/sundryExpenseRules";

const loading = ref(false);
const emit = defineEmits<{ (event: "complete"): void }>();
const props = defineProps<{ id: string }>();

const expenseTypeStore = useExpenseTypeStore();
const sundryExpenseStore = useSundryExpenseStore();
const sundryExpense = sundryExpenseStore.dictionary[props.id];

const formRef = ref<FormInstance>();
const formValue = reactive({
    id: props.id,
    date: sundryExpense?.date,
    expenseTypeId: sundryExpense?.expenseTypeId,
    submissionMethod: sundryExpense?.submissionMethod,
    details: sundryExpense?.details,
    participationNumber: sundryExpense?.participationNumber,
    receiptId: sundryExpense?.receiptId,
    receipt: ref<File | null>(),
    amount: sundryExpense?.amount,
    status: sundryExpense?.status,
});

const expenseTypeDictionary = expenseTypeStore.dictionary;

async function update() {
    if (!formRef.value) return;
    formRef.value.validate(async (valid) => {
        if (valid) {
            loading.value = true;
            await sundryExpenseStore.update(
                props.id,
                formValue.date,
                formValue.expenseTypeId,
                formValue.details,
                formValue.participationNumber,
                formValue.amount,
                formValue.submissionMethod,
                formValue.receipt ?? null
            );
            formValue.date = new Date();
            formValue.expenseTypeId= "1";
            formValue.submissionMethod = null;
            formValue.details = null;
            formValue.participationNumber= 0,
            formValue.receiptId = null;
            formValue.amount = 0;
            formValue.status = null;
            formValue.receipt = undefined;
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
