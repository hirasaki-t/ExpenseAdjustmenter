<template>
  <Story title="organisms/userSettings/UserSettingForm">
    <Variant title="追加">
      <user-setting-form :model-value="{...modelValue}" :loading="false" @update="(x) => logEvent('update', x)" />
    </Variant>
    <Variant title="更新">
      <user-setting-form :model-value="{...modelValue}" :loading="false" is-update @update="(x) => logEvent('update', x)" />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import type { User } from "@/types";
import UserSettingForm from "./UserSettingForm.vue";
import { ref } from "vue";
import { logEvent } from "histoire/client";

const modelValue = ref<Omit<User, "id">>({
  name: "櫻庭 史樹",
  mail: "sakuraba@google.com",
  isAdmin: true,
  isActive: true,
});
</script>
