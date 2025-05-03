import MainMenu from '@/views/MainMenu.vue'
import { createRouter, createWebHistory } from 'vue-router'
import BenefitList from '@/components/BenefitList.vue' 

const routes = [
    {
        path: '/main-menu',
        name: 'MainMenu',
        component: MainMenu
    }
    {
        path: '/benefits',
        name: 'Benefits',
        component: BenefitList
    }
]


const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router
