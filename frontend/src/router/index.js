import Login from '../views/LoginForm.vue'
import { createRouter, createWebHistory } from 'vue-router'
import BenefitList from '@/components/BenefitList.vue' 
import Benefit from '@/components/BenefitView.vue'

const routes =
[
    { path: '/login', name: 'LoginForm', component: Login },
    {
        path: '/benefits',
        name: 'Benefits',
        component: BenefitList
    },

    {
        path: '/benefit/:id',
        name: 'Benefit',
        component: Benefit,
        props: true
    }

]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router
