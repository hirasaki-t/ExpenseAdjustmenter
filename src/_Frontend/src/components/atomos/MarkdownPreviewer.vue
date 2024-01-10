<template><div ref="previewTarget" /></template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
import { marked } from "marked";
const props = defineProps<{ markdown: string }>();
const previewTarget = ref<InstanceType<typeof HTMLDivElement>>();
onMounted(() => {
    if (previewTarget.value === undefined) return;
    previewTarget.value.innerHTML = marked.parse(props.markdown);
});
</script>