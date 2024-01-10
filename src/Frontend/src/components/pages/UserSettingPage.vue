<template>
  <menu-bar>
    <user-setting-list
      :datas="userStore.orderedUsers"
      @add="showDialogs.add = true"
      @update="(id) => openEditForm(id)"
    />
    <el-dialog v-model="showDialogs.add" title="ユーザー追加" width="400">
      <user-setting-add-form :loading="loadings.add" @add="(user) => addUser(user)" />
    </el-dialog>
    <el-dialog v-model="showDialogs.update" title="ユーザー編集" width="400">
      <user-setting-update-form
        :user="selectedUser"
        :loading="loadings.update"
        @update="(user) => updateUser(user)"
        v-if="selectedUser"
      />
    </el-dialog>
  </menu-bar>
</template>

<script setup lang="ts">
import { useUserStore } from "@/stores/userStore";
import MenuBar from "../MenuBar.vue";
import UserSettingList from "../organisms/userSettings/UserSettingList.vue";
import type { User } from "@/types";
import { reactive, ref } from "vue";
import UserSettingAddForm from "../organisms/userSettings/UserSettingAddForm.vue";
import UserSettingUpdateForm from "../organisms/userSettings/UserSettingUpdateForm.vue";

const userStore = useUserStore();
const loadings = reactive({ add: false, update: false });
const showDialogs = reactive({ add: false, update: false });
const selectedUser = ref<User>();

function openEditForm(id: string) {
  selectedUser.value = userStore.dictionary[id];
  showDialogs.update = true;
}

async function addUser(user: Omit<User, "id">) {
  loadings.add = true;
  try {
    await userStore.add(user.mail, user.isAdmin);
  } finally {
    loadings.add = false;
    showDialogs.add = false;
  }
}

async function updateUser(user: User) {
  loadings.update = true;
  try {
    await userStore.update(user.id, user.isAdmin, user.isActive);
  } finally {
    loadings.update = false;
    showDialogs.update = false;
  }
}
</script>
