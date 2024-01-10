<template>
  <Story title="organisms/traveringExpenses/TraveringExpenseList">
    <Variant title="空">
      <travering-expense-list :traffic-categories="trafficCategories" display-name="山田 太郎" :datas="[]" />
    </Variant>
    <Variant title="データあり">
      <travering-expense-list :traffic-categories="trafficCategories" display-name="山田 太郎" :datas="datas" />
    </Variant>
  </Story>
</template>

<script setup lang="ts">
import type { TrafficCategory, TraveringExpenseListData } from "@/types";
import TraveringExpenseList from "./TraveringExpenseList.vue";

const trafficCategories: TrafficCategory[] = [
  { id: "1", name: "電車", details: null, isReceipt: false, isActive: true },
  { id: "2", name: "タクシー", details: null, isReceipt: true, isActive: true },
];

const datas: TraveringExpenseListData[] = [
  {
    id: "1",
    date: new Date(2023, 1, 1),
    startSection: "東京",
    endSection: "上野",
    categoryId: "1",
    workName: "営業",
    remarks: null,
    amount: 3000,
    receiptId: null,
    submissionMethod: null,
    reviewerId: null,
    comment: null,
    status: null,
  },
  {
    id: "2",
    date: new Date(2023, 1, 2),
    startSection: null,
    endSection: null,
    categoryId: "2",
    workName: "現場業務",
    remarks: "摘要",
    amount: 15000,
    receiptId: "1",
    submissionMethod: "電子",
    reviewerId: "5",
    comment: "コメント",
    status: "承認",
  },
  {
    id: "3",
    date: new Date(2023, 1, 3),
    startSection: null,
    endSection: null,
    categoryId: "2",
    workName: "現場業務",
    remarks: "摘要",
    amount: 15000,
    receiptId: "1",
    submissionMethod: "電子",
    reviewerId: "5",
    comment: "コメント",
    status: "否認",
  },
];
</script>
