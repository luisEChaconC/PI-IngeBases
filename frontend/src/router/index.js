import SelectChangeBenefit from '../views/SelectChangeBenefit.vue'
import { createRouter, createWebHistory } from 'vue-router'
import BenefitList from '@/components/BenefitList.vue' 
import Benefit from '@/components/BenefitView.vue'

const routes =
[
    {
        path: '/benefits',
        name: 'Benefits',
        component: BenefitList
    },

    {
        path: '/select-change-benefit',
        name: 'SelectChangeBenefit',
        component: SelectChangeBenefit
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