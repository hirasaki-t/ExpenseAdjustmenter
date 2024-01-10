<template>
    <el-dialog :model-value="props.show" title="一括確認" append-to-body :width="400" @update:model-value="emit('complete')">
        <el-form ref="formRef" label-placement="left" label-width="110px">
            <div style="display:flex; justify-content:flex-end; gap:10px">
                <el-input v-model="comment" placeholder="任意コメント(ユーザーに通知されます)" />
                <span>
                    <approve-button :loading="loading" @click="approve" />
                </span>
                <span>
                    <reject-button :loading="loading" @click="reject" />
                </span>
            </div>
        </el-form>
    </el-dialog>
</template>

<script setup lang="ts">
import { ElForm, ElInput, ElDialog } from "element-plus";
import { ref } from "vue";
import { useApproveHistoryStore } from "../../../stores/approveHistoryStore";
import ApproveButton from "../../molecules/buttons/ApproveButton.vue";
import RejectButton from "../../molecules/buttons/RejectButton.vue";

const props = defineProps<{ show: boolean, selectedIds: string[] }>();
const emit = defineEmits<{ (event: "complete"): void }>();

const approveHistoryStore = useApproveHistoryStore();
const loading = ref(false);
const comment = ref("");

async function approve() {
    try{
        loading.value = true;
        await approveHistoryStore.approve(props.selectedIds, comment.value);
    } finally {
        loading.value = false;
    }
}

async function reject() {
    try{
        loading.value = true;
        await approveHistoryStore.reject(props.selectedIds, comment.value);
    } finally {
        loading.value = false;
    }
}

</script>