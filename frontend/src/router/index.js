import SelectChangeBenefit from '../views/SelectChangeBenefit.vue'
import { createRouter, createWebHistory } from 'vue-router'


const routes =
[

    {
        path: '/select-change-benefit',
        name: 'SelectChangeBenefit',
        component: SelectChangeBenefit
    },

   

]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router