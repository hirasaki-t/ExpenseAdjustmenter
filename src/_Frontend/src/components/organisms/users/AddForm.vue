<template>
    <el-form ref="formRef" :model="formValue" label-placement="left" label-width="120px">
        <stack-container style="display: flex; flex-direction: column">
            <el-form-item label="氏名">
                {{formValue.name}}
            </el-form-item>
            <el-form-item label="メールアドレス">
                <el-input v-model="formValue.mail" style="width: 200px" />
            </el-form-item>
            <el-form-item label="管理者">
                <el-checkbox v-model="formValue.isAdmin" />
            </el-form-item>
        </stack-container>
        <div style="display: flex; justify-content: flex-end">
            <add-button :loading="loading" @add="add" />
        </div>
    </el-form>
</template>

<script setup lang="ts">
import axios from "axios";
import { ElForm, ElFormItem, FormInstance, ElInput, ElCheckbox } from "element-plus";
import { reactive, ref, watch } from "vue";
import { GraphUser } from "../../../datas/graphUser";
import { useUserStore } from "../../../stores/userStore";
import { WebAPIRequestor } from "../../../webAPIRequestor";
import StackContainer from "../../atomos/StackContainer.vue";
import AddButton from "../../molecules/buttons/AddButton.vue";

const loading = ref(false);
const emit = defineEmits<{ (event: "complete"): void }>();

const formRef = ref<FormInstance>();
const formValue = reactive<{ name?: string, mail?: string, isAdmin: boolean }>({
    isAdmin: false,
});

let timer: number | undefined;

const userStore = useUserStore();

async function add() {
    if (!formRef.value) return;
    formRef.value.validate(async (valid) => {
        if (valid) {
            loading.value = true;
            try {
                await userStore.add(formValue.mail!, formValue.isAdmin);
                formValue.name = undefined;
                formValue.mail = undefined;
                formValue.isAdmin = false;
                emit("complete");
            } finally {
                loading.value = false;
            }
        }
    });
}

watch(() => formValue.mail, () => {
    if (timer !== undefined) window.clearTimeout(timer);
    timer = window.setTimeout(searchDisplayName, 500);
});

async function searchDisplayName() {
    formValue.name = "";
    formValue.name = await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        try{
            return ((
                await axios.get(
                    `https://graph.microsoft.com/v1.0/users/${formValue.mail}`,
                    config
                )).data as GraphUser).displayName;
        }catch{
            return undefined;
        }
    }, ["user.read", "user.readbasic.all"]);
}
</script>
