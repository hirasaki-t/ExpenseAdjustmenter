<template>
    <delete-button :loading="false" @delete="remove" />
</template>

<script setup lang="ts">
import { ref } from "vue";
import DeleteButton from "../../molecules/buttons/DeleteButton.vue";
const props = defineProps<{ name: string }>();
const loading = ref(false);
async function remove() {
    loading.value = true;
    try {
        //
    } finally {
        loading.value = false;
    }
}
</script>