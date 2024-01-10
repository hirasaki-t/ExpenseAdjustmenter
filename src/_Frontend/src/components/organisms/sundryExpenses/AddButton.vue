<template>
    <add-dialog-open-button v-model:show="show" resource-name="諸経費" :width="500">
        <add-form @complete="show = false" />
    </add-dialog-open-button>
</template>

<script setup lang="ts">
import { ref } from "vue";
import AddDialogOpenButton from "../../molecules/buttons/AddDialogOpenButton.vue";
import AddForm from "./AddForm.vue";

const show = ref(false);
</script>
