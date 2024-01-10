<template>
  <Story title="organisms/userSettings/UserSettingEditForm">
    <Variant title="通常">
      <user-setting-update-form :user="{ ...modelValue }" @update="(x) => logEvent('update', x)" />
    </Variant>
    <Variant title="ロード中">
      <user-setting-update-form :user="{ ...modelValue }" loading />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import type { User } from "@/types";
import UserSettingUpdateForm from "./UserSettingUpdateForm.vue";
import { ref } from "vue";
import { logEvent } from "histoire/client";

const modelValue = ref<User>({
  id: "1",
  name: "櫻庭 史樹",
  mail: "sakuraba@google.com",
  isAdmin: true,
  isActive: true
});
</script>
