<template>
  <router-view />
</template>

<script setup lang="ts">
import { RouterView } from "vue-router";
import { ElNotification } from "element-plus";
import { Authorizer } from "../authorizer";
import { onErrorCaptured } from "vue";
import { useTraveringExpenseStore } from "../stores/traveringExpenseStore";
import { useUserStore } from "../stores/userStore";
import { useExpenseTypeStore } from "../stores/expenseTypeStore";
import { useSundryExpenseStore } from "../stores/sundryExpenseStore";
import { useTrafficCategoryStore } from "../stores/trafficCategoryStore";
import { useAzureDirectoryStore } from "../stores/azureDirectoryStore";
import { useApproveHistoryStore } from "../stores/approveHistoryStore";
import { useSystemSettingStore } from "../stores/systemSettingStore";

onErrorCaptured((x) => {
  openNotification(x.message);
  return true;
});

function openNotification(message: string) {
  ElNotification.error({
    title: "エラー",
    message: message,
    showClose: false,
    duration: 3000,
  });
}

const traveringExpenseStore = useTraveringExpenseStore();
const sundryExpenseStore = useSundryExpenseStore();
const userStore = useUserStore();
const expenseTypeStore = useExpenseTypeStore();
const trafficCtegoryStore = useTrafficCategoryStore();
const approveHistoryStore = useApproveHistoryStore();
const azureDirectoryStore = useAzureDirectoryStore();
const systemSettingStore = useSystemSettingStore();

const signIn = await Authorizer.SignInAsync();
if (signIn) {
  await Promise.all([
    azureDirectoryStore.initialize(),
    traveringExpenseStore.initialize(),
    sundryExpenseStore.initialize(),
    userStore.initialize(),
    expenseTypeStore.initialize(),
    approveHistoryStore.initialize(),
    trafficCtegoryStore.initialize(),
    systemSettingStore.initialize(),
  ]);
}
</script>
