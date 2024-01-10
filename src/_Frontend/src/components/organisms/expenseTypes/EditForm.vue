<template>
    <el-form ref="formRef" :model="formValue" label-placement="left" label-width="120px">
        <stack-container style="display: flex; flex-direction: column">
            <el-form-item label="経費種別名">
                <el-input v-model="formValue.name" style="width: 200px" />
            </el-form-item>
            <el-form-item label="説明">
                <el-input v-model="formValue.details" style="width: 300px" />
            </el-form-item>
            <el-form-item label="領収書">
                <el-checkbox v-model="formValue.isReceipt" />
            </el-form-item>
            <el-form-item label="有効">
                <el-checkbox v-model="formValue.isActive" />
            </el-form-item>
        </stack-container>
        <div style="display: flex; justify-content: flex-end">
            <update-button :loading="loading" @update="update" />
        </div>
    </el-form>
</template>

<script setup lang="ts">
import { ElForm, ElFormItem, FormInstance, ElInput, ElCheckbox } from "element-plus";
import { reactive, ref } from "vue";
import { useExpenseTypeStore } from "../../../stores/expenseTypeStore";
import StackContainer from "../../atomos/StackContainer.vue";
import UpdateButton from "../../molecules/buttons/UpdateButton.vue"

const loading = ref(false);
const props = defineProps<{ id: string }>();
const emit = defineEmits<{ (event: "complete"): void }>();

const expenseTypeStore = useExpenseTypeStore();
const expenseType = expenseTypeStore.dictionary[props.id];

const formRef = ref<FormInstance>();
const formValue = reactive({
    name: expenseType?.name,
    details: expenseType?.details,
    isActive: expenseType?.isActive,
    isReceipt: expenseType?.isReceipt,
});

const userNameInput = ref<string>();

async function update() {
    if (!formRef.value) return;
    formRef.value.validate(async (valid) => {
        if (valid) {
            loading.value = true;
            try {
                await expenseTypeStore.update(props.id, formValue!.name, formValue!.details, formValue!.isReceipt, formValue!.isActive);
                emit("complete");
            } finally {
                loading.value = false;
            }
        }
    });
}
</script>
