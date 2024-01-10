import axios from "axios";
import { defineStore } from "pinia";
import { ref } from "vue";
import { WebAPIRequestor } from "../webAPIRequestor";
import type { GraphUser } from "@/types";

export const useAzureDirectoryStore = defineStore("azureDirectory", () => {
  const graphUser = ref<GraphUser>();

  async function initialize() {
    graphUser.value = await getMe();
  }

  return { graphUser, initialize };

  async function getMe() {
    return await WebAPIRequestor.ThrowIfRequestFailedAsync(
      async (config) => {
        try {
          return (await axios.get("https://graph.microsoft.com/v1.0/me", config)).data as GraphUser;
        } catch {
          return undefined;
        }
      },
      ["user.read"]
    );
  }
});
