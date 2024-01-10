<template>
  <div :class="$style.container">
    <el-select
      :model-value="modelValue?.getTime()"
      :disabled="selectableMonths.length === 0"
      :placeholder="placeholder"
      @update:model-value="(x) => $emit('update:model-value', !x ? undefined : new Date(x))"
    >
      <el-option
        v-for="month in selectableMonths"
        :value="month.getTime()"
        :label="formatDateToYearMonth(month) ?? ''"
        :key="month.getTime()"
      />
    </el-select>
  </div>
</template>

<script setup lang="ts">
import { formatDateToYearMonth } from "@/formatter";
import { effect } from "vue";

const props = defineProps<{ selectableMonths: Date[]; modelValue?: Date; placeholder?: string }>();
const emits = defineEmits<{ (event: "update:model-value", modelValue?: Date): void }>();

effect(() => {
  if (!!props.modelValue && props.selectableMonths.every((x) => x.getTime() !== props.modelValue?.getTime()))
    emits("update:model-value");
});
</script>

<style module>
.container {
  display: inline-block;
  width: 120px;
}
</style>
