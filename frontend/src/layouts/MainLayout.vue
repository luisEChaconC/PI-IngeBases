<template>
  <div>
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

<style>
.content-area {
  padding: 20px;
 
}
</style>