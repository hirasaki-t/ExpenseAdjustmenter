import { createRouter, createWebHistory } from "vue-router";
import ProxyPage from "../components/pages/ProxyPage.vue";
import TraveringExpensePage from "../components/pages/TraveringExpensePage.vue";
import ExpenseTypeSettingPage from "../components/pages/ExpenseTypeSettingPage.vue"
import TrafficCategorySettingPage from "../components/pages/TrafficCategorySettingPage.vue"
import SystemSettingPage from "../components/pages/SystemSettingPage.vue";
import UserSettingPage from "../components/pages/UserSettingPage.vue";
import SundryExpensePage from "../components/pages/SundryExpensePage.vue"
import ApproveHistoryPage from "../components/pages/ApproveHistoryPage.vue"

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: "/", component: ProxyPage },
    { path: "/travering-expense", component: TraveringExpensePage },
    { path: "/sundry-expense", component: SundryExpensePage },
    { path: "/approve-history", component: ApproveHistoryPage },
    { path: "/traffic-category-setting", component: TrafficCategorySettingPage },
    { path: "/user-setting", component: UserSettingPage },
    { path: "/expense-type-setting", component: ExpenseTypeSettingPage },
    { path: "/system-setting", component: SystemSettingPage },
  ],
});

export default router;
