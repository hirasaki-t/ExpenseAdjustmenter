<template>
  <Story title="molecules/selectors/SubmissionMethodSelector">
    <submission-method-selector v-model="modelValue" />
  </Story>
</template>

<script setup lang="ts">
import type { SubmissionMethod } from "@/types";
import SubmissionMethodSelector from "./SubmissionMethodSelector.vue";
import { ref } from "vue";

const modelValue = ref<SubmissionMethod>();
</script>
