<template>
  <div>
    <user-setting-form v-model="formValue" ref="formRef" />
    <div style="text-align: right">
      <add-button :loading="loading" @add="add" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import AddButton from "../../molecules/buttons/AddButton.vue";
import type { User } from "@/types";
import UserSettingForm from "./UserSettingForm.vue";

defineProps<{ loading?: boolean }>();
const emit = defineEmits<{ (event: "add", user: Omit<User, "id">): void }>();

const formRef = ref<InstanceType<typeof UserSettingForm>>();
const formValue = ref<Omit<User, "id">>({
  name: "",
  mail: "",
  isAdmin: false,
  isActive: true,
});

async function add() {
  if (!formRef.value) return;
  if (await formRef.value.validate()) {
    emit("add", formValue.value);
  }
}
</script>
