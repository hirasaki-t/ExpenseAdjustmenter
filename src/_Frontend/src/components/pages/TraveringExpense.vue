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
            <travering-expenses-list :disable="applicable" @update:model-value="updateSelectedIds" />
        </div>
    </menu-bar>
</template>

<script setup lang="ts">
import { ref, watch, computed } from "vue";
import MenuBar from "../MenuBar.vue";
import TraveringExpensesList from "../organisms/traveringExpenses/List.vue";
import { ElSelect, ElOption } from "element-plus";
import ApplicationButton from "../organisms/traveringExpenses/ApplicationButton.vue";
import { useTraveringExpenseStore } from "../../stores/traveringExpenseStore";
import { onBeforeRouteLeave } from "vue-router";
import { format } from "date-fns";
import { useSystemSettingStore } from "../../stores/systemSettingStore";

const traveringExpenseStore = useTraveringExpenseStore();
const systemSettingStore = useSystemSettingStore();
const deadline = systemSettingStore.deadline;
const applicable = ref(checkApplicable());
const today = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate());
const selectedMonth = ref<Date>(new Date(today.getFullYear(), today.getMonth(), 1));
const selectedIds = ref<string[]>([]);
const selectableMonths = computed(() => traveringExpenseStore.selectableMonths);
const isApplicationable = computed(() => traveringExpenseStore.traveringExpenses.filter(x => x.status === '否認' || x.status === null).length < 1);

watch(selectedMonth, async (newSelectedDate, oldSelectedDate) => {
    if (!newSelectedDate) return;
    if (oldSelectedDate?.getFullYear() !== newSelectedDate.getFullYear() || oldSelectedDate?.getMonth() !== newSelectedDate.getMonth())
        traveringExpenseStore.reload(newSelectedDate.getFullYear(), newSelectedDate.getMonth() + 1);
});

function updateSelectedIds(ids: string[]){
    selectedIds.value = ids;
}

function updateSelectedMonth(selectedMonthNumber: number) {
    selectedMonth.value = new Date(selectedMonthNumber);
}

function checkApplicable() {
    const today = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate());
    if (deadline!.getFullYear() === today.getFullYear() && deadline!.getMonth() === today.getMonth() && deadline!.getDate() < today.getDate()) return false;
    return true;
}

onBeforeRouteLeave(() => {
    selectedMonth.value = today;
    return true;
});

</script>