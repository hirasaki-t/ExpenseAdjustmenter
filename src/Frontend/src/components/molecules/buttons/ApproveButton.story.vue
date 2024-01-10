<template>
  <Story title="molecules/buttons/ApproveButton" :layout="{ type: 'grid' }">
    <Variant title="通常">
      <approve-button />
    </Variant>
    <Variant title="ロード中">
      <approve-button loading />
    </Variant>
    <Variant title="無効">
      <approve-button disabled />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import ApproveButton from "./ApproveButton.vue";
</script>
