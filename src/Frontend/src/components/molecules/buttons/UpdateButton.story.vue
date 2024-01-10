<template>
  <Story title="molecules/buttons/UpdateButton" :layout="{ type: 'grid' }">
    <Variant title="通常">
      <update-button />
    </Variant>
    <Variant title="ロード中">
      <update-button loading />
    </Variant>
    <Variant title="無効">
      <update-button disabled />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import UpdateButton from "./UpdateButton.vue";
</script>
