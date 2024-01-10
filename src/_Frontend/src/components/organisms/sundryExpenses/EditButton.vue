<template>
    <edit-dialog-open-button v-model:show="show" :disabled="props.status === '承認' || props.status === '申請中'" resource-name="" :width="500">
        <edit-form :id="props.id" @complete="show = false" />
    </edit-dialog-open-button>
</template>

<script setup lang="ts">
import { ref } from "vue";
import EditDialogOpenButton from "../../molecules/buttons/EditDialogOpenButton.vue";
import EditForm from "./EditForm.vue";

const props = defineProps<{ id: string, status: string | null }>();
const show = ref(false);
</script>
