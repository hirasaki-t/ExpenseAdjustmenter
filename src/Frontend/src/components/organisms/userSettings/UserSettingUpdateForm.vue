<template>
  <div>
    <user-setting-form v-model="formValue" ref="formRef" is-update />
    <div style="display: flex; justify-content: flex-end">
      <update-button :loading="loading" @update="update" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from "vue";
import UpdateButton from "../../molecules/buttons/UpdateButton.vue";
import type { User } from "@/types";
import UserSettingForm from "./UserSettingForm.vue";

const props = defineProps<{ user: User; loading?: boolean }>();
const emit = defineEmits<{ (event: "update", user: User): void }>();

const formRef = ref<InstanceType<typeof UserSettingForm>>();
const formValue = ref<User>({ ...props.user });

watch(
  () => props.user,
  (x) => (formValue.value = { ...x })
);

async function update() {
  if (!formRef.value) return;
  if (await formRef.value.validate()) {
    emit("update", { ...formValue.value, id: props.user.id });
  }
}
</script>
