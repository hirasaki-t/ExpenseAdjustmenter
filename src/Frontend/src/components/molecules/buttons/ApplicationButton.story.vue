<template>
  <Story title="molecules/buttons/ApplicationButton" :layout="{ type: 'grid' }">
    <Variant title="通常">
      <application-button />
    </Variant>
    <Variant title="ロード中">
      <application-button loading />
    </Variant>
    <Variant title="無効">
      <application-button disabled />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import ApplicationButton from "./ApplicationButton.vue";
</script>
