<template>
    <div style="display:flex; flex-direction:column; gap:10px">
        <el-card shadow="never">
            <template #header>
                <div style="display:flex; justify-content:space-between">
                    締日の設定
                    <span>
                        <el-button type="primary" disabled>
                            保存
                        </el-button>
                    </span>
                </div>
            </template>
            <div style="display:flex; align-items: center; gap:10px">
                <el-input-number v-model="date" />
                日
            </div>
        </el-card>
    </div>
</template>

<script setup lang="ts">
import { ElCard, ElInputNumber, ElButton } from "element-plus";
import { ref } from "vue";

const date = ref(2);
</script>