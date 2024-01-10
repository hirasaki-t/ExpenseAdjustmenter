<template>
  <Story title="organisms/traveringCategories/TraveringCategoryAddFrom">
    <Variant title="通常">
      <traffic-category-add-form />
    </Variant>
    <Variant title="ロード中">
      <traffic-category-add-form loading />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import TrafficCategoryAddForm from "./TrafficCategoryAddForm.vue";
</script>
