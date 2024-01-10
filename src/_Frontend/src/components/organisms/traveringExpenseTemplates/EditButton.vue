<template>
    <edit-dialog-open-button v-model:show="show" resource-name="" :width="500">
        <edit-form :name="props.name" @complete="show = false" />
    </edit-dialog-open-button>
</template>

<script setup lang="ts">
import { ref } from "vue";
import EditDialogOpenButton from "../../molecules/buttons/EditDialogOpenButton.vue";
import EditForm from "./EditForm.vue";

const props = defineProps<{ name: string }>();
const show = ref(false);
</script>
