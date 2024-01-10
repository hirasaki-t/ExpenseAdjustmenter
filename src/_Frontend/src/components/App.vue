<template>
  <el-config-provider :locale="ja" size="small">
      <suspense>
          <template #default>
              <content-root />
          </template>
          <template #fallback> 
            <div :className="$style.loadingContainer">
                <loading />
            </div>
          </template>
      </suspense>
  </el-config-provider>
</template>

<script setup lang="ts">
import ContentRoot from "./ContentRoot.vue";
import ja from "element-plus/es/locale/lang/ja";
import { ElConfigProvider } from "element-plus";
import Loading from "./atomos/Loading.vue";
</script>

<style>
* {
  margin: 0;
}
</style>

<style module>
.loadingContainer {
    display: flex;
    width: 100%;
    height: 100%;
    justify-content: center;
    align-items: center;
}
</style>