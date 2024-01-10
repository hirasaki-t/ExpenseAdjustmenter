<template>
  <el-container :class="$style.page">
    <el-header :class="$style.header">
      <div style="display: flex; align-items: center; gap: 10px">
        <el-button
          link
          size="large"
          :icon="showMenu ? Expand : Fold"
          @click="showMenu = showMenu ? false : true"
          style="color: black"
        />
        <div @click="router.push('/')" style="cursor: pointer; color: black">経費精算システム</div>
      </div>
      <div style="display: flex; align-items: center; color: black; gap: 10px">
        {{ `${userInfo?.displayName}[${userInfo?.mail}]` }}
        <el-switch v-model="darkMode" inline-prompt :active-icon="Moon" :inactive-icon="Sunny" />
      </div>
    </el-header>
    <el-container style="height: 100%; padding-top: 60px; box-sizing: border-box">
      <el-aside style="height: 100%" :width="showMenu ? 'auto' : '15%'">
        <el-scrollbar style="height: 100%" view-style="height:100%">
          <el-menu
            router
            :defaultOpeneds="['1', '3']"
            :default-active="router.currentRoute.value.path.slice(1)"
            :collapse="showMenu"
            style="height: 100%"
          >
            <el-sub-menu index="1">
              <template #title>
                <el-icon><money /></el-icon>
                <span>経費精算</span>
              </template>
              <el-menu-item index="travering-expense">旅費・交通費</el-menu-item>
              <el-menu-item index="sundry-expense">諸経費</el-menu-item>
            </el-sub-menu>
            <el-menu-item v-if="isAdmin" index="approve-history">
              <template #title>
                <el-icon><management /></el-icon>
                <span>申請管理</span>
              </template>
            </el-menu-item>
            <el-sub-menu v-if="isAdmin" index="3">
              <template #title>
                <el-icon><tools /></el-icon>
                <span>マスタ管理</span>
              </template>
              <el-menu-item index="user-setting">ユーザー</el-menu-item>
              <el-menu-item index="traffic-category-setting">交通区分</el-menu-item>
              <el-menu-item index="expense-type-setting">経費種別</el-menu-item>
              <el-menu-item index="system-setting">システム設定</el-menu-item>
            </el-sub-menu>
          </el-menu>
        </el-scrollbar>
      </el-aside>
      <el-main style="height: 100%; widht: 100%">
        <el-scrollbar view-style="height:100%">
          <slot />
        </el-scrollbar>
      </el-main>
    </el-container>
  </el-container>
</template>

<script setup lang="ts">
import { useRouter } from "vue-router";
import {
  ElContainer,
  ElHeader,
  ElAside,
  ElMenu,
  ElSubMenu,
  ElIcon,
  ElMenuItem,
  ElMain,
  ElScrollbar,
  ElButton,
  ElSwitch,
} from "element-plus";
import { Management, Money, Tools, Expand, Fold } from "@element-plus/icons-vue";
import { useAzureDirectoryStore } from "../stores/azureDirectoryStore";
import { ref } from "vue";
import { useDark } from "@vueuse/core";
import { Moon, Sunny } from "@element-plus/icons-vue";
import "element-plus/theme-chalk/dark/css-vars.css";
import { Authorizer } from "../authorizer";

const router = useRouter();
const azureDirectoryStore = useAzureDirectoryStore();
const userInfo = azureDirectoryStore.graphUser;
const showMenu = ref(false);
const darkMode = useDark();
const isAdmin = ref<boolean>(Authorizer.GetIsAdmin());
</script>

<style module>
.page {
  height: 100%;
}
.header {
  position: fixed;
  display: flex;
  align-items: center;
  width: 100%;
  z-index: 1000;
  justify-content: space-between;
  background-color: silver;
}
</style>
