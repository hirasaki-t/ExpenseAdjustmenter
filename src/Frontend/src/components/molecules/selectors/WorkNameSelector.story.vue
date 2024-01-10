<template>
  <Story title="molecules/selectors/WorkNameSelector" :layout="{ type: 'grid' }">
    <Variant title="選択可能">
      <work-name-selector />
    </Variant>
    <Variant title="初期選択">
      <work-name-selector model-value="社内業務" />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import WorkNameSelector from "./WorkNameSelector.vue";
</script>
