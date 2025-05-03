import SelectChangeBenefit from '../views/SelectChangeBenefit.vue'
import { createRouter, createWebHistory } from 'vue-router'
import BenefitList from '@/components/BenefitList.vue' 

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
    }

]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router