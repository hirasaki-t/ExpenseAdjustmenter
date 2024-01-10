import { createApp } from 'vue'
import App from './components/App.vue'
import Index from "./components/pages/Index.vue";
import TraveringExpense from "./components/pages/TraveringExpense.vue";
import SundryExpense from "./components/pages/SundryExpense.vue";
import ApproveHistory from "./components/pages/ApproveHistory.vue";
import UserSetting from "./components/pages/UserSetting.vue";
import CategorySetting from "./components/pages/CategorySetting.vue";
import ExpenseTypeSetting from "./components/pages/ExpenseTypeSetting.vue";
import SystemSetting from "./components/pages/SystemSetting.vue"
import { createRouter, createWebHistory } from 'vue-router';
import { createPinia } from 'pinia';
import "element-plus/dist/index.css";

const router = createRouter({
    history: createWebHistory(),
    routes: [
        { path: "/", component: Index },
        { path: "/TraveringExpense", component: TraveringExpense },
        { path: "/SundryExpense", component: SundryExpense },
        { path: "/ApproveHistory", component: ApproveHistory },
        { path: "/UserSetting", component: UserSetting },
        { path: "/CategorySetting", component: CategorySetting},
        { path: "/ExpenseTypeSetting", component: ExpenseTypeSetting},
        { path: "/SystemSetting", component: SystemSetting },
    ],
});

createApp(App).use(router).use(createPinia()).mount('#app')
