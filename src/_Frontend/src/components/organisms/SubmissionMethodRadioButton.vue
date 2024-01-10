<template>
    <el-radio-group v-model="selectedValue" @click="$emit('update', selectedValue)">
        <el-radio label="紙/持参">紙/持参</el-radio>
        <el-radio label="紙/郵送">紙/郵送</el-radio>
        <el-radio label="電子">電子</el-radio>
    </el-radio-group>
</template>

<script setup lang="ts">
import { ElRadioGroup, ElRadio } from "element-plus";
import { ref } from "vue";
const selectedValue = ref<string>();
defineEmits<{ (event: "update"): string }>();
</script>
