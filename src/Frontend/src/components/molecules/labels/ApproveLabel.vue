<template>
    <span v-if="props.status ==='否認'">
        <el-popover trigger="click" :title="props.userId ? userDictionary[props.userId].name : ''" :width="200" :content="props.comment ?? undefined" >
            <template #reference>
                <el-link type="danger">
                    {{props.status}}
                </el-link>
            </template>
        </el-popover>
    </span>
    <span v-else-if="props.status === '承認'" style="color:limegreen;">
        {{props.status}}
    </span>
    <span v-else>
        {{props.status === null ? "未申請" : props.status}}
    </span>
</template>

<script setup lang="ts">
import { ElPopover, ElLink } from "element-plus";
import { useUserStore } from "../../../stores/userStore";
const props = defineProps<{ status: string | null, comment: string | null, userId:string | null }>();
const userStore = useUserStore();
const userDictionary = userStore.dictionary;
</script>