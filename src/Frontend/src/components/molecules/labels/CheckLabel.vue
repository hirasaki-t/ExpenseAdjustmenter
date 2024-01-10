<template>
    <span>
        {{ props.checked ? "✔︎" : "" }}
    </span>
</template>

<script setup lang="ts">
const props = defineProps<{ checked: boolean }>();
</script>