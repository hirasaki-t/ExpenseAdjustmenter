<template>
  <Story title="molecules/selectors/ApproveStatusSelector" :layout="{ type: 'grid' }">
    <Variant title="選択可能">
      <approve-status-selector />
    </Variant>
    <Variant title="初期選択">
      <approve-status-selector model-value="承認" />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import ApproveStatusSelector from "./ApproveStatusSelector.vue";
</script>
