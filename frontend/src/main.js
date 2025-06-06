import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap'
import '@fortawesome/fontawesome-free/css/all.css';
import '@vuepic/vue-datepicker/dist/main.css'


createApp(App).use(router).mount('#app')
