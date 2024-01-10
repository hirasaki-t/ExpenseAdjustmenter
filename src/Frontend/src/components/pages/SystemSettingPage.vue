<template>
    <menu-bar>
        <div style="display:flex; flex-direction: column; gap:10px; height: 100%">
            <deadline-form />
        </div>
    </menu-bar>
</template>

<script setup lang="ts">
import MenuBar from '../MenuBar.vue';
import DeadlineForm from "../organisms/systemSettings/DeadlineForm.vue"

</script>