<template>
    <el-dialog :model-value="props.show" title="" append-to-body :width="400" @update:model-value="emit('complete')">
        <el-form ref="formRef" :model="approveHistory" label-placement="left" label-width="110px">
            <stack-container style="display: flex; flex-direction: column">
                <el-form-item label="日付">
                    {{ `${format(approveHistory.date, 'yyyy年MM月dd日')}` }}
                </el-form-item>
                <el-form-item label="氏名">
                    {{ user.name }}
                </el-form-item>
                <el-form-item label="分類">
                    {{ approveHistory.type }}
                </el-form-item>
                <div v-if="approveHistory.type === '旅費・交通費'">
                    <el-form-item label="業務名">
                        {{ approveHistory.workName }}
                    </el-form-item>
                    <el-form-item label="区間">
                        <span v-if="approveHistory.startSection">
                            {{ approveHistory.startSection }} → {{ approveHistory.endSection }}
                        </span>
                    </el-form-item>
                    <el-form-item label="交通区分">
                        {{ approveHistory.categoryId ? categoryDictionary[approveHistory.categoryId].name : ''  }}
                    </el-form-item>
                    <el-form-item label="摘要">
                        {{ approveHistory.remarks }}
                    </el-form-item>
                </div>
                <div v-else-if="approveHistory.type === '諸経費'">
                    <el-form-item label="経費種別">
                        {{ approveHistory.expenseTypeId ? expenseTypeDictionary[approveHistory.expenseTypeId].name : '' }}
                    </el-form-item>
                    <el-form-item label="詳細">
                        {{ approveHistory.details }}
                    </el-form-item>
                    <el-form-item label="人数">
                        {{ approveHistory.participationNumber }}人
                    </el-form-item>
                </div>
                <el-form-item label="金額">
                    {{`${approveHistory.amount.toLocaleString()}円`}}
                </el-form-item>
            </stack-container>
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
import { format } from "date-fns";
import { ElForm, ElFormItem, ElInput, ElDialog, type FormInstance } from "element-plus";
import { ref, computed } from "vue";
import { useApproveHistoryStore } from "../../../stores/approveHistoryStore";
import { useTrafficCategoryStore } from "../../../stores/trafficCategoryStore";
import { useExpenseTypeStore } from "../../../stores/expenseTypeStore";
import { useUserStore } from "../../../stores/userStore";
import StackContainer from "../../atomos/StackContainer.vue";
import ApproveButton from "../../molecules/buttons/ApproveButton.vue"
import RejectButton from "../../molecules/buttons/RejectButton.vue";

const loading = ref(false);
const props = defineProps<{ id: string, show: boolean }>();
const emit = defineEmits<{ (event: "complete"): void }>();

const approveHistoryStore = useApproveHistoryStore();
const approveHistory = computed(() => approveHistoryStore.dictionary[props.id]);
const userStore = useUserStore();
const user = computed(() => userStore.dictionary[approveHistory.value.userId]);
const trafficCategoryStore = useTrafficCategoryStore();
const categoryDictionary = trafficCategoryStore.dictionary;
const expenseTypeStore = useExpenseTypeStore();
const expenseTypeDictionary = expenseTypeStore.dictionary;

const formRef = ref<FormInstance>();
const comment = ref("");

async function approve() {
    if (!formRef.value) return;
    formRef.value.validate(async (valid) => {
        if (valid) {
            loading.value = true;
            try {
                await approveHistoryStore.approve([approveHistory.value.expenseId], comment.value);
                comment.value = "";
                emit("complete");
            } finally {
                loading.value = false;
            }
        }
    });
}

async function reject() {
    if (!formRef.value) return;
    formRef.value.validate(async (valid) => {
        if (valid) {
            loading.value = true;
            try {
                await approveHistoryStore.reject([approveHistory.value.expenseId], comment.value);
                comment.value = "";
                emit("complete");
            } finally {
                loading.value = false;
            }
        }
    });
}
</script>
