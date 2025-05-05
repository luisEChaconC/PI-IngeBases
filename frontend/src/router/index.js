import EmployeesList from '../views/EmployeesList.vue'
import { createRouter, createWebHistory } from 'vue-router'
import BenefitList from '@/components/BenefitList.vue' 
import Benefit from '@/components/BenefitView.vue'

const routes =
[
    {
        path: '/employees-list',
        name: 'EmployeesList',
        component: EmployeesList
    },
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
