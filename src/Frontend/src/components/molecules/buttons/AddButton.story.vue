<template>
  <Story title="molecules/buttons/AddButton" :layout="{ type: 'grid' }">
    <Variant title="通常">
      <add-button />
    </Variant>
    <Variant title="ロード中">
      <add-button loading />
    </Variant>
    <Variant title="無効">
      <add-button disabled />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import AddButton from "./AddButton.vue";
</script>
