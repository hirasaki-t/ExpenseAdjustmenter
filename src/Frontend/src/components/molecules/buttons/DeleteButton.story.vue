<template>
  <Story title="molecules/buttons/DeleteButton" :layout="{ type: 'grid' }">
    <Variant title="通常">
      <delete-button />
    </Variant>
    <Variant title="ロード中">
      <delete-button loading />
    </Variant>
    <Variant title="無効">
      <delete-button disabled />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import DeleteButton from "./DeleteButton.vue";
</script>
