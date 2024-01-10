<template>
    <span v-if="!props.isReceipt">
        -
    </span>
    <span v-if="props.isReceipt">
        <el-link v-if="props.submissionMethod === '電子'" type="primary" @click="requestDownload(props.receiptId)">
            電子
        </el-link>
        <span v-if="props.submissionMethod !== '電子'">
            {{props.submissionMethod}}
        </span>
    </span>
</template>

<script setup lang="ts">
import axios from "axios";
import { ElLink } from "element-plus";
import { SubmissionMethod } from "../../types";
import { WebAPIRequestor } from "../../webAPIRequestor";

const props = defineProps<{ isReceipt: boolean, submissionMethod: SubmissionMethod | null, receiptId: string | null, fileName: string | null }>();

async function requestDownload(receiptId: string | null){
    if (receiptId === null) return;
    await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
        const response = await axios.get(`api/Receipts/${receiptId}`, {
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
                downloadButton.setAttribute("download", props.fileName ?? '領収書.pdf');
                document.body.appendChild(downloadButton);
                downloadButton.click();
                document.body.removeChild(downloadButton);
            } finally {
                window.URL.revokeObjectURL(downloadUrl);
            }
    });
}
</script>