import { createPinia } from "pinia";
import { defineSetupVue3 } from "@histoire/plugin-vue";
import ElementPlus from "element-plus";
import "element-plus/dist/index.css";

export const setupVue3 = defineSetupVue3(({ app }) => {
  const pinia = createPinia();
  app.use(pinia);
  app.use(ElementPlus, { size: "small" });
});
