<template>
  <el-table :data="datas" border height="100%">
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
        <add-button @click="$emit('add')" />
      </template>
      <template #default="scope">
        <edit-button @click="$emit('update', scope.row.id)" />
      </template>
    </el-table-column>
  </el-table>
</template>

<script setup lang="ts">
import { ElTable, ElTableColumn } from "element-plus";
import CheckLabel from "../../molecules/labels/CheckLabel.vue";
import AddButton from "@/components/molecules/buttons/AddButton.vue";
import EditButton from "@/components/molecules/buttons/EditButton.vue";
import type { User } from "@/types";

defineProps<{ datas: User[] }>();
defineEmits<{
  (event: "add"): void;
  (event: "update", id: string): void;
}>();
</script>

<style>
td > div.cell,
th > div.cell {
  white-space: nowrap !important;
}
</style>
