<template>
    <div ref="elementRef" />
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
import { ElLoading } from "element-plus";
const elementRef = ref<InstanceType<typeof HTMLElement>>();
onMounted(() => {
    ElLoading.service({
        fullscreen: false,
        target: elementRef.value,
        text: 'Loading...'
    });
});
</script>