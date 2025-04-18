import Login from '../views/LoginForm.vue'
import { createRouter, createWebHistory } from 'vue-router'

const routes = [
    { path: '/login', name: 'LoginForm', component: Login }
]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

export default router
