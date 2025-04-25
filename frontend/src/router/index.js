import MainMenu from '@/views/MainMenu.vue'
import { createRouter, createWebHistory } from 'vue-router'

const routes = [
    { path: '/main-menu', name: 'MainMenu', component: MainMenu }
]
const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router
