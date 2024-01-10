<template>
  <el-form label-placement="left" label-width="120px">
    <el-form-item label="旅費・交通費">
      <div style="display: flex; gap: 10px">
        <el-button type="success" @click="emit('download-travering-expense-csv', isOnlyTraveringApproved)">
          ダウンロード
        </el-button>
        <el-checkbox v-model="isOnlyTraveringApproved">承認済のみ</el-checkbox>
      </div>
    </el-form-item>
    <el-form-item label="諸経費">
      <div style="display: flex; gap: 10px">
        <el-button type="success" @click="emit('download-sundry-expense-csv', isOnlySundryApproved)">
          ダウンロード
        </el-button>
        <el-checkbox v-model="isOnlySundryApproved">承認済のみ</el-checkbox>
      </div>
    </el-form-item>
  </el-form>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { ElForm, ElFormItem, ElButton, ElCheckbox } from "element-plus";

const emit = defineEmits<{
  (event: "download-travering-expense-csv", onlyApproved: boolean): void;
  (event: "download-sundry-expense-csv", onlyApproved: boolean): void;
}>();

const isOnlyTraveringApproved = ref(true);
const isOnlySundryApproved = ref(true);
</script>
