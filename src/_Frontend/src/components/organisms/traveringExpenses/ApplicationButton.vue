<template>
    <application-button :loading="false" :disable="props.disable" @application="application" />
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useApproveHistoryStore } from "../../../stores/approveHistoryStore";
import ApplicationButton from "../../molecules/buttons/ApplicationButton.vue";
const props = defineProps<{ date: Date, disable?: boolean }>();
const approveHistoryStore = useApproveHistoryStore();
const loading = ref(false);

async function application() {
    loading.value = true;
    try {
        await approveHistoryStore.application(props.date);
    } finally {
        loading.value = false;
    }
}
</script>