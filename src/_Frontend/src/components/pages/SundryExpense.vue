<template>
    <menu-bar>
        <div style="display:flex; flex-direction: column; gap:10px; height: 100%">
            <div style="display:flex; gap:10px">
                <el-select style="width: 120px" :model-value="selectedMonth.getTime()" @update:model-value="updateSelectedMonth">
                    <el-option
                        v-for="date in selectableMonths.sort((x, y) => y.getTime() - x.getTime())"
                        :label="`${format(date, 'yyyy年MM月')}`"
                        :value="date.getTime()"
                    />
                </el-select>
                <application-button :loading="false" :disable="isApplicationable" :date="selectedMonth" />
            </div>
            <sundry-expense-list />
        </div>
    </menu-bar>
</template>

<script setup lang="ts">
import { ref, watch, computed } from "vue";
import MenuBar from "../MenuBar.vue";
import { ElSelect, ElOption } from "element-plus";
import ApplicationButton from "../organisms/traveringExpenses/ApplicationButton.vue";
import { useSundryExpenseStore } from "../../stores/sundryExpenseStore";
import SundryExpenseList from "../organisms/sundryExpenses/List.vue";
import { onBeforeRouteLeave } from "vue-router";
import { format } from "date-fns";

const sundryExpenseStore = useSundryExpenseStore();
const today = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate());
const selectedMonth = ref<Date>(new Date(today.getFullYear(), today.getMonth(), 1));
const selectableMonths = computed(() => sundryExpenseStore.selectableMonths);
const isApplicationable = computed(() => sundryExpenseStore.sundryExoenses.filter(x => x.status === '否認' || x.status === null).length < 1);

watch(selectedMonth, async (newSelectedDate, oldSelectedDate) => {
    if (!newSelectedDate) return;
    if (oldSelectedDate?.getFullYear() !== newSelectedDate.getFullYear() || oldSelectedDate?.getMonth() !== newSelectedDate.getMonth())
        sundryExpenseStore.reload(newSelectedDate.getFullYear(), newSelectedDate.getMonth() + 1);
});

function updateSelectedMonth(selectedMonthNumber: number) {
    selectedMonth.value = new Date(selectedMonthNumber);
}

onBeforeRouteLeave(() => {
    selectedMonth.value = today;
    return true;
});
</script>