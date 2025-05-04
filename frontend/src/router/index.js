import AddEmployee from '../views/AddEmployee.vue'
import { createRouter, createWebHistory } from 'vue-router'
import BenefitList from '@/components/BenefitList.vue' 

const routes =
[
    {
        path: '/add-employee',
        name: 'AddEmployee',
        component: AddEmployee
    },
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
