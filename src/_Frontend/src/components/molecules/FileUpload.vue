<template>
    <div style="display:flex; flex-direction: column;">
        <span style="display:flex; gap:10px;">
            <el-button type="success" @click="() => inputRef?.click()">ファイルを選択</el-button>
            <span style="color:darkgray;">
                {{ selectedFileName === "" ? props.existFile ? "取込済み" : "" : selectedFileName }}
            </span>
        </span>
    </div>
    <input type="file" ref="inputRef" style="display:none" accept=".pdf" @change="updateSelectedFile" />
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { ElButton } from 'element-plus'

const props = defineProps<{ existFile?: boolean }>();
const emit = defineEmits<{ (event: "update-file", newFile: File | null) : void }>();
const inputRef = ref<HTMLInputElement>();
const selectedFileName = ref("");

function updateSelectedFile(){
    const file = !inputRef.value?.files ? null : inputRef.value.files[0];
    selectedFileName.value = file?.name ?? "";
    emit("update-file", file);
}
</script>