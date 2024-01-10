<template>
    <menu-bar>
        <stack-container horizontal style="height:100%; width: 100%;">
            <div style="display:flex; justify-content: space-around;">
                <markdown-previewer :markdown="RequirementsDocument" />
                <markdown-previewer :markdown="GitHub" />
                <markdown-previewer :markdown="ReportPage" />
            </div>
            <markdown-previewer :markdown="history" />
        </stack-container>
    </menu-bar>
</template>

<script setup lang="ts">
import MenuBar from "../MenuBar.vue";
import StackContainer from "../atomos/StackContainer.vue";
import MarkdownPreviewer from "../atomos/MarkdownPreviewer.vue";

const RequirementsDocument = "";
const GitHub = "";
const ReportPage = "";

const history =
    "";

</script>