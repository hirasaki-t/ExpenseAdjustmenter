<template>
    <add-dialog-open-button v-model:show="show" resource-name="旅費・交通費" :disabled="props.disable" :width="500">
        <add-form @complete="show = false" />
    </add-dialog-open-button>
</template>

<script setup lang="ts">
import { ref } from "vue";
import AddDialogOpenButton from "../../molecules/buttons/AddDialogOpenButton.vue";
import AddForm from "./AddForm.vue";

const show = ref(false);
const props = defineProps<{ disable: boolean }>();
</script>
