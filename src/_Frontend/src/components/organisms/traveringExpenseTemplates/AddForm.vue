<template>
    <el-form ref="formRef" :model="formValue" label-placement="left" label-width="120px">
        <stack-container style="display: flex; flex-direction: column">
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
        </stack-container>
        <div style="display: flex; justify-content:flex-end">
            <el-button type="primary" @click="add">
                追加
            </el-button>
        </div>
    </el-form>
</template>

<script setup lang="ts">
import { ElForm, ElFormItem, ElInput, ElCheckbox, ElButton } from "element-plus";
import { reactive, ref } from "vue";
import StackContainer from "../../atomos/StackContainer.vue";
import WorkNameSelector from "../../molecules/selectors/WorkNameSelector.vue";
import CategorySelector from "../../molecules/selectors/CategorySelector.vue";
import { TraveringExpenseTemplate } from "../../../datas/TraveringExpenseTemplate";

const loading = ref(false);
const formValue = reactive<{ 
    name?: string,
    workName?: string, 
    startSection: string | null, 
    endSection: string | null, 
    isGoBack: boolean, 
    categoryId?: string,
    amount: number}>({
        startSection: null,
        endSection: null,
        isGoBack: false,
        amount: 0,
});

function add() {
    try {
        loading.value = true;
        const result = localStorage.getItem("traveringExpenseTemplates");
        if (!result) return;
        const traveringExpenseTemplates = JSON.parse(result) as TraveringExpenseTemplate;
        localStorage.setItem("traveringExpenseTemplates", JSON.stringify(formValue));
    } finally {
        loading.value = false;
    }
}
</script>