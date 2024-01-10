<template>
  <Story title="molecules/buttons/EditButton" :layout="{ type: 'grid' }">
    <Variant title="通常">
      <edit-button />
    </Variant>
    <Variant title="ロード中">
      <edit-button loading />
    </Variant>
    <Variant title="無効">
      <edit-button disabled />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import EditButton from "./EditButton.vue";
</script>
