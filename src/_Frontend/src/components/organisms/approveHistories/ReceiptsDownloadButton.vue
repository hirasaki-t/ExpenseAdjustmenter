<template>
    <el-button type="success" :disabled="props.disabled" @click="requestDownload"> 領収書一括ダウンロード </el-button>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { ElButton } from "element-plus";
import { WebAPIRequestor } from "../../../webAPIRequestor";
import axios from "axios";
import { useApproveHistoryStore } from "../../../stores/approveHistoryStore";

const props = defineProps<{ disabled: boolean }>();

const approveHistoryStore = useApproveHistoryStore();
const approveHistories = computed(() => approveHistoryStore.approveHistories);

async function requestDownload(){
    const expenseIds = approveHistories.value.map(x => x.expenseId);
    await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        const response = await axios.post("api/Receipts", { expenseIds }, {
            ...config,
            responseType: "blob",
            headers: {
                ...config.headers,
            }
        });
        const downloadUrl = window.URL.createObjectURL(response.data);
        try {
                const downloadButton = document.createElement("a");
                downloadButton.href = downloadUrl;
                downloadButton.setAttribute("download", "領収書.zip");
                document.body.appendChild(downloadButton);
                downloadButton.click();
                document.body.removeChild(downloadButton);
            } finally {
                window.URL.revokeObjectURL(downloadUrl);
            }
    });
}
</script>
