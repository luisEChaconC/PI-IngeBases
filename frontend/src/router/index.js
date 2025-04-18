import CompanyRegistration from '@/views/CompanyRegistration.vue'
import { createRouter, createWebHistory } from 'vue-router'

const routes = [
    { path: '/company-registration', name: 'CompanyRegistration', component: CompanyRegistration }
]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router
