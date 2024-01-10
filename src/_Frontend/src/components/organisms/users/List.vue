<template>
    <el-table :data="users" border height="100%">
        <el-table-column label="氏名" property="name" align="center" width="200" />
        <el-table-column label="メールアドレス" property="mail" align="center" />
        <el-table-column label="管理者" property="isAdmin" align="center" width="100">
            <template #default="scope">
                <check-label :checked="scope.row.isAdmin" />
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
import AddButton from "./AddButton.vue";
import { useUserStore } from "../../../stores/userStore";
import CheckLabel from "../../molecules/labels/CheckLabel.vue";
import EditButton from "./EditButton.vue";

const userStore = useUserStore();
const users = computed(() => userStore.orderedUsers);

</script>

<style>
td > div.cell, th > div.cell {
    white-space:nowrap !important;
}
</style>