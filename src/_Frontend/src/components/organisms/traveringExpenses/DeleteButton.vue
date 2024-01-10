<template>
    <delete-button :loading="false" :disable="props.status === '承認' || props.status === '申請中'" @delete="remove" />
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useTraveringExpenseStore } from "../../../stores/traveringExpenseStore";
import DeleteButton from "../../molecules/buttons/DeleteButton.vue";
const props = defineProps<{ id: string, status: string | null }>();
const loading = ref(false);
const traveringExpenseStore = useTraveringExpenseStore();
async function remove() {
    loading.value = true;
    try {
        await traveringExpenseStore.remove(props.id);
    } finally {
        loading.value = false;
    }
}
</script>