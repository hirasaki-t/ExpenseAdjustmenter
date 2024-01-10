<template>
    <el-table :data="tableDatas" border table-layout="auto">
        <el-table-column label="テンプレート名" prop="name" align="center" />
        <el-table-column label="業務名" prop="workName" align="center" />
        <el-table-column label="出発地" prop="startSection" align="center" />
        <el-table-column label="到着地" prop="endSection" align="center" />
        <el-table-column label="往復" prop="isGoBack" align="center">
            <template #default="scope">
                <check-label :checked="scope.row.isGoBack" />
            </template>
        </el-table-column>
        <el-table-column label="交通区分" prop="category" align="center" />
        <el-table-column label="金額" align="center">
            <template #default="scope">
                {{`${(scope.row.amount as number).toLocaleString()}円`}}
            </template>
        </el-table-column>
        <el-table-column width="80" align="center">
            <template #header>
                <add-button :loading="false" />
            </template>
            <template #default="scope">
                <div style="display:flex; gap:10px">
                    <spam>
                        <edit-button name="" />
                    </spam>
                    <spam>
                        <delete-button name="" />
                    </spam>
                </div>
            </template>
        </el-table-column>
    </el-table>
</template>

<script setup lang="ts">
import { ElButton, ElTable, ElTableColumn } from "element-plus";
import AddButton from "./AddButton.vue";
import EditButton from "./EditButton.vue";
import DeleteButton from "./DeleteButton.vue";

const tableDatas = [
    {
        name: "自宅から事務所",
        workName: "社内業務",
        startSection: "代官山",
        endSection: "初台",
        isGoBack: true,
        category: "電車",
        amount: 528
    },{
        name: "自宅から現場",
        workName: "現場業務",
        startSection: "代官山",
        endSection: "初台",
        isGoBack: true,
        category: "電車",
        amount: 431
    },
    {
        name: "現場から事務所",
        workName: "社内業務",
        startSection: "品川",
        endSection: "初台",
        isGoBack: true,
        category: "電車",
        amount: 324
    }
]
</script>