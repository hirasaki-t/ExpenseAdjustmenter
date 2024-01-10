<template>
  <Story title="molecules/buttons/CsvDownloadButton" :layout="{ type: 'grid' }">
    <Variant title="通常">
      <csv-download-button />
    </Variant>
    <Variant title="ロード中">
      <csv-download-button loading />
    </Variant>
    <Variant title="無効">
      <csv-download-button disabled />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import CsvDownloadButton from "./CsvDownloadButton.vue";
</script>
