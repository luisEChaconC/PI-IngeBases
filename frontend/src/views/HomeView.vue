<template>
  <div class="home">
    <div v-if="role === 'Employer'" class="iframe-wrapper">
      <iframe
        :src="iframeUrl"
        frameborder="0"
        class="metabase-iframe"
      ></iframe>
    </div>
    <div v-else class="no-dashboard">
      <p>Lo siento, todavía no está el dashboard.</p>
    </div>
  </div>
</template>


<script setup>
import {computed } from 'vue'
import currentUserService from "@/services/currentUserService";

const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage();
const companyId = currentUserInformation?.companyId;
const role = currentUserInformation.position;
console.log(role)

const iframeUrl = computed(() =>
  `http://localhost:3000/public/dashboard/7c408b10-a984-48d2-a354-654d12146274?company_id=${companyId}`
)
</script>

<style scoped>
.iframe-wrapper {
  position: relative;
  height: 800px;
  overflow: hidden;
  border-radius: 8px;
  border: 1px solid #ccc;
}

.metabase-iframe {
  position: absolute;
  top: -145px; 
  left: 0;
  width: 100%;
  height: 935px; 
}
</style>

