<template>
    <el-form ref="formRef" :model="formValue" label-placement="left" label-width="120px">
        <stack-container style="display: flex; flex-direction: column">
            <el-form-item label="区分">
                <el-input v-model="formValue.name" style="width: 200px" />
            </el-form-item>
            <el-form-item label="説明">
                <el-input v-model="formValue.details" style="width: 300px" />
            </el-form-item>
            <el-form-item label="領収書">
                <el-checkbox v-model="formValue.isReceipt" />
            </el-form-item>
        </stack-container>
        <div style="display: flex; justify-content: flex-end">
            <add-button :loading="loading" @add="add" />
        </div>
    </el-form>
</template>

<script setup lang="ts">
import { ElForm, ElFormItem, FormInstance, ElInput, ElCheckbox } from "element-plus";
import { reactive, ref } from "vue";
import { useCategoryStore } from "../../../stores/categoryStore";
import StackContainer from "../../atomos/StackContainer.vue";
import AddButton from "../../molecules/buttons/AddButton.vue";

const loading = ref(false);
const emit = defineEmits<{ (event: "complete"): void }>();

const categoryStore = useCategoryStore();

const formRef = ref<FormInstance>();
const formValue = reactive<{ name: string, details: string | null, isReceipt: boolean}>({
    name: "",
    details: null,
    isReceipt: false,
});

async function add() {
    if (!formRef.value) return;
    formRef.value.validate(async (valid) => {
        if (valid) {
            loading.value = true;
            try {
                await categoryStore.add(formValue.name!, formValue.details!, formValue.isReceipt!);
                formValue.name = "";
                formValue.details = null,
                formValue.isReceipt = false;
                emit("complete");
            } finally {
                loading.value = false;
            }
        }
    });
}
</script>
