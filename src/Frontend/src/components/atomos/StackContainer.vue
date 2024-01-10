<template>
  <div :style="style">
    <slot />
  </div>
</template>

<script setup lang="ts">
import { computed, type StyleValue } from "vue";

const props = defineProps<{
  gap?: number;
  horizontal?: boolean;
  justifyContent?: "center" | "flex-start" | "flex-end";
}>();

const style = computed<StyleValue>(() => ({
  display: "inline-flex",
  flexDirection: props.horizontal ?? false ? "column" : "row",
  gap: `${props.gap ?? 10}px`,
  justifyContent: props.justifyContent,
}));
</script>
