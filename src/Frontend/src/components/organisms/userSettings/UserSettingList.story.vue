<template>
  <Story title="organisms/userSettings/UserSettingList">
    <Variant title="空">
      <user-setting-list :datas="[]" />
    </Variant>
    <Variant title="データあり">
      <user-setting-list :datas="datas" />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import type { User } from "@/types";
import UserSettingList from "./UserSettingList.vue";

const datas: User[] = [
  {
    id: "1",
    name: "岩田 史応",
    mail: "iwata@google.com",
    isAdmin: true,
    isActive: true,
  },
  {
    id: "2",
    name: "櫻庭 史樹",
    mail: "sakuraba@google.com",
    isAdmin: true,
    isActive: true,
  },
  {
    id: "3",
    name: "平崎 時生",
    mail: "hirasaki@google.com",
    isAdmin: false,
    isActive: true,
  },
];
</script>
