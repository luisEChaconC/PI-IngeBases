import ViewCompanyInfo from '@/views/ViewCompanyInfo.vue'
import { createRouter, createWebHistory } from 'vue-router'

const routes = [
    { path: '/view-company-info', name: 'ViewCompanyInfo', component: ViewCompanyInfo }
]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router
