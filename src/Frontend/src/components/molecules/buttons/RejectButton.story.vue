<template>
  <Story title="molecules/buttons/RejectButton" :layout="{ type: 'grid' }">
    <Variant title="通常">
      <reject-button />
    </Variant>
    <Variant title="ロード中">
      <reject-button loading />
    </Variant>
    <Variant title="無効">
      <reject-button disabled />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import RejectButton from "./RejectButton.vue";
</script>
