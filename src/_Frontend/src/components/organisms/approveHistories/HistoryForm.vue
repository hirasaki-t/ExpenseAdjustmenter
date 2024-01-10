<template>
    <el-timeline>
        <el-timeline-item 
            v-for="(data, index) in datas"
            :timestamp="`${format(data.date, 'yyyy/MM/dd HH:mm')}`"
            :key="index">
                <div style="display:flex; flex-direction:column; gap:5px;">
                    <span style="display:flex; gap:10px">
                        <span :style="getColor(data.status)">
                            {{ data.status }}
                        </span>
                        <span>
                            {{ `[${data.userId ? userDictionary[data.userId]?.name : null}]` }}
                        </span>
                    </span>
                    <span v-if="data.comment" style="font-size:12px">
                        {{ data.comment }}
                    </span>
                </div>
            </el-timeline-item>
    </el-timeline>
</template>

<script setup lang="ts">
import axios from "axios";
import { format } from "date-fns";
import { ElTimeline, ElTimelineItem } from "element-plus";
import { ref, watch } from "vue";
import { ApproveHistory } from "../../../datas/approveHistory";
import { useUserStore } from "../../../stores/userStore";
import { WebAPIRequestor } from "../../../webAPIRequestor";

const props = defineProps<{ id: string, show: boolean; disabled?: boolean }>();
const emit = defineEmits<{ (event: "complete"): void }>();
const userStore = useUserStore();
const userDictionary = userStore.dictionary;

const datas = ref<ApproveHistory[]>([]);

async function requestGets(expenseId: string){
  if(expenseId === "") return;
  return await WebAPIRequestor.ThrowIfRequestFailedAsync(async (config) => {
      return (await axios.get<ApproveHistory[]>(`api/ApproveHistories/${expenseId}`, config)).data;
  });
}

function getColor(status: string | null){
  if(status === '承認') return "color:lightgreen";
  if(status === '否認') return "color:tomato";
}

datas.value = await requestGets(props.id) ?? [];
</script>