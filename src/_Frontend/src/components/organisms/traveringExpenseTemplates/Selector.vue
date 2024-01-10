<template>
    <div style="display:flex; gap:10px">
        <el-select placeholder="テンプレート選択">
            <el-option value="自宅から事務所" />
            <el-option value="自宅から現場" />
            <el-option value="現場から事務所" />
        </el-select>
        <el-button :icon="Setting" @click="show = true" />
    </div>
    <el-dialog v-model="show" :width=900 title="テンプレート管理" >
        <travering-expense-template-list />
    </el-dialog>
</template>

<script setup lang="ts">
import { ElButton, ElDialog, ElSelect, ElOption } from "element-plus";
import { Setting } from '@element-plus/icons-vue'
import { ref } from "vue";
import TraveringExpenseTemplateList from "./List.vue"

const show = ref(false);
defineEmits<{ (event: "update:show", show: boolean): void }>();
</script>