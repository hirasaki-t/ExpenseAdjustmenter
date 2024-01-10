<template>
  <Story title="molecules/pickers/DatePicker" :layout="{ type: 'grid' }">
    <Variant title="空">
      <date-picker />
    </Variant>
    <Variant title="プレースホルダ">
      <date-picker placeholder="プレースホルダ" />
    </Variant>
    <Variant title="初期値">
      <date-picker :model-value="new Date(2023, 1, 5)" />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import DatePicker from "./DatePicker.vue";
</script>
