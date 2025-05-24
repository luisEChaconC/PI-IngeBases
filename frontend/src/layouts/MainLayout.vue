<template>
  <div class="layout">
    <MainMenu />
    <div class="content-area">
      <router-view />  
    </div>
  </div>
</template>

<script>
import MainMenu from '@/views/MainMenu.vue';

export default {
  components: { MainMenu },
};
</script>

<style scoped>
.layout {
  min-height: 100vh;
  background-color: #f8f9fa; 
}

.content-area {
  padding-top: 80px; 
  padding-left: 1rem;
  padding-right: 1rem;
  min-height: calc(100vh - 80px); 
  box-sizing: border-box;
  background-color: white; 
}

:deep(.navbar) {
  position: fixed !important;
  top: 0;
  left: 0;
  right: 0;
  z-index: 1030;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}
</style>