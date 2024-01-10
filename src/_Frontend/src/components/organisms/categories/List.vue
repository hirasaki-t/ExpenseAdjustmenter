<template>
    <el-table :data="categories" border height="100%">
        <el-table-column label="区分名" property="name" align="center" width="250" />
        <el-table-column label="説明" property="details" header-align="center" />
        <el-table-column label="領収書" property="isReceipt" align="center" width="100">
            <template #default="scope">
                <check-label :checked="scope.row.isReceipt" />
            </template>
        </el-table-column>
        <el-table-column label="有効" property="isActive" align="center" width="100">
            <template #default="scope">
                <check-label :checked="scope.row.isActive" />
            </template>
        </el-table-column>
        <el-table-column width="120" align="center">
            <template #header>
                <add-button />
            </template>
            <template #default="scope">
                <stack-container>
                    <edit-button :id="scope.row.id" :loading="false" />
                </stack-container>
            </template>
        </el-table-column>
        </el-table>
</template>

<script setup lang="ts">
import { ElTable, ElTableColumn } from "element-plus";
import { computed, ref } from "vue";
import StackContainer from "../../atomos/StackContainer.vue";
import CheckLabel from "../../molecules/labels/CheckLabel.vue";
import EditButton from "./EditButton.vue";
import AddButton from "./AddButton.vue";
import { useCategoryStore } from "../../../stores/categoryStore";

const categoryStore = useCategoryStore();
const categories = computed(() => categoryStore.orderedCategories);

</script>

<style>
td > div.cell, th > div.cell {
    white-space:nowrap !important;
}
</style>