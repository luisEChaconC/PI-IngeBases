import AddEmployee from '../views/AddEmployee.vue'
import { createRouter, createWebHistory } from 'vue-router'
import BenefitList from '@/components/BenefitList.vue' 
import Benefit from '@/components/BenefitView.vue'

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
