<template>
    <el-form ref="formRef" :model="formValue" label-placement="left" label-width="120px">
        <stack-container style="display: flex; flex-direction: column">
            <el-form-item label="氏名">
                {{formValue.name}}
            </el-form-item>
            <el-form-item label="メールアドレス">
                {{formValue.mail}}
            </el-form-item>
            <el-form-item label="管理者">
                <el-checkbox v-model="formValue.isAdmin" />
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
import { useUserStore } from "../../../stores/userStore";
import StackContainer from "../../atomos/StackContainer.vue";
import UpdateButton from "../../molecules/buttons/UpdateButton.vue"

const loading = ref(false);
const props = defineProps<{ id: string }>();
const emit = defineEmits<{ (event: "complete"): void }>();

const userStore = useUserStore();
const user = userStore.dictionary[props.id];

const formRef = ref<FormInstance>();
const formValue = reactive({
    name: user?.name,
    mail: user?.mail,
    isAdmin: user?.isAdmin,
    isActive: user?.isActive
});

const userNameInput = ref<string>();

async function update() {
    if (!formRef.value) return;
    formRef.value.validate(async (valid) => {
        if (valid) {
            loading.value = true;
            try {
                await userStore.update(props.id, formValue!.isAdmin, formValue!.isActive);
                emit("complete");
            } finally {
                loading.value = false;
            }
        }
    });
}
</script>
