import Login from '../views/LoginForm.vue'
import { createRouter, createWebHistory } from 'vue-router'
import BenefitList from '@/components/BenefitList.vue' 

const routes =
[
    { path: '/login', name: 'LoginForm', component: Login },
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
