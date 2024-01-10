import { createApp } from "vue";
import { createPinia } from "pinia";
import ElementPlus from "element-plus";
import App from "./components/App.vue";
import router from "./router";
import "./global.css";
import "element-plus/dist/index.css";

const app = createApp(App);

app.use(createPinia());
app.use(ElementPlus, { size: "small" });
app.use(router);

app.mount("#app");
