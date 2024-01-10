<template>
  <menu-bar>
    <traffic-category-list
      :datas="trafficCategoryStore.orderedTrafficCategories"
      @add="showDialogs.add = true"
      @update="(id) => openUpdateForm(id)"
    />
    <el-dialog v-model="showDialogs.add" title="交通区分追加" width="400">
      <traffic-category-add-form
        :loading="loadings.add"
        @add="(trafficCategory) => addTrafficCategory(trafficCategory)"
      />
    </el-dialog>
    <el-dialog v-model="showDialogs.update" title="交通区分編集" width="400">
      <traffic-category-update-form
        :traffic-category="selectedTrafficCategory"
        :loading="loadings.update"
        @update="(trafficCategory) => updateTrafficCategory(trafficCategory)"
        v-if="selectedTrafficCategory"
      />
    </el-dialog>
  </menu-bar>
</template>

<script setup lang="ts">
import { useTrafficCategoryStore } from "@/stores/trafficCategoryStore";
import { reactive, ref } from "vue";
import type { TrafficCategory } from "@/types";
import MenuBar from "../MenuBar.vue";
import TrafficCategoryList from "../organisms/trafficCategorySettings/TrafficCategoryList.vue";
import TrafficCategoryAddForm from "../organisms/trafficCategorySettings/TrafficCategoryAddForm.vue";
import TrafficCategoryUpdateForm from "../organisms/trafficCategorySettings/TrafficCategoryUpdateForm.vue";

const trafficCategoryStore = useTrafficCategoryStore();
const loadings = reactive({ add: false, update: false });
const showDialogs = reactive({ add: false, update: false });
const selectedTrafficCategory = ref<TrafficCategory>();

function openUpdateForm(id: string) {
  selectedTrafficCategory.value = trafficCategoryStore.dictionary[id];
  showDialogs.update = true;
}

async function addTrafficCategory(trafficCategory: Omit<TrafficCategory, "id">) {
  loadings.add = true;
  try {
    await trafficCategoryStore.add(trafficCategory.name, trafficCategory.details, trafficCategory.isReceipt);
  } finally {
    loadings.add = false;
    showDialogs.add = false;
  }
}

async function updateTrafficCategory(trafficCategory: TrafficCategory) {
  loadings.update = true;
  try {
    await trafficCategoryStore.update(
      trafficCategory.id,
      trafficCategory.name,
      trafficCategory.details,
      trafficCategory.isReceipt,
      trafficCategory.isActive
    );
  } finally {
    loadings.update = false;
    showDialogs.update = false;
  }
}
</script>
