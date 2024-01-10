<template>
  <Story title="organisms/userSettings/UserSettingAddForm">
    <Variant title="通常">
      <user-setting-add-form :loading="false" @add="(x) => logEvent('add', x)" />
    </Variant>
    <Variant title="ロード中">
      <user-setting-add-form loading />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import UserSettingAddForm from "./UserSettingAddForm.vue";
import { logEvent } from "histoire/client";
</script>
