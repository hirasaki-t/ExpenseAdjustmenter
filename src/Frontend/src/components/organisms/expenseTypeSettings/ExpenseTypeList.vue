<template>
  <el-table :data="datas" border height="100%">
    <el-table-column label="経費種別名" property="name" align="center" width="250" />
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
        <add-button @add="() => $emit('add')" />
      </template>
      <template #default="scope">
        <edit-button @edit="$emit('update', scope.row.id)" />
      </template>
    </el-table-column>
  </el-table>
</template>

<script setup lang="ts">
import { ElTable, ElTableColumn } from "element-plus";
import CheckLabel from "../../molecules/labels/CheckLabel.vue";
import type { ExpenseType } from "@/types";
import AddButton from "@/components/molecules/buttons/AddButton.vue";
import EditButton from "@/components/molecules/buttons/EditButton.vue";

defineProps<{ datas: ExpenseType[] }>();
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
