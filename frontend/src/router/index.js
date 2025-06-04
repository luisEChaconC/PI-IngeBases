import MainLayout from '@/layouts/MainLayout.vue'
import SelectChangeBenefit from '../views/SelectChangeBenefit.vue'
import AddEmployee from '@/views/AddEmployee.vue'
import ViewEmployeeProfile from '../views/ViewEmployeeProfile.vue'
import { createRouter, createWebHistory } from 'vue-router'
import CompanyRegistration from '@/views/CompanyRegistration.vue'
import EmployerRegistration from '@/views/EmployerRegistration.vue'
import EmployeesList from '@/views/EmployeesList.vue'
import Login from '@/views/LoginForm.vue'
import BenefitList from '@/views/BenefitList.vue'
import Benefit from '@/views/BenefitView.vue'
import BenefitCreate from '@/views/BenefitCreate.vue'
import ViewEmployeeProfileEmployer from '../views/ViewEmployeeProfileEmployer.vue'
import ViewCompanyInfo from '@/views/ViewCompanyInfo.vue'
import ViewCompaniesList from '../views/ViewCompaniesList.vue'
import HomeView from '../views/HomeView.vue'
import PayrollsList from '@/views/PayrollsList.vue'
import EmployeeTimesheet from '@/views/EmployeeTimesheet.vue'
import UnauthorizedView from '@/views/UnauthorizedView.vue'

import CurrentUserService from '@/services/currentUserService'

const routes = [
    { path: '/', redirect: '/login' },
    { path: '/login', name: 'LoginForm', component: Login },
    { path: '/company-registration', name: 'CompanyRegistration', component: CompanyRegistration },
    { path: '/employer-registration/:companyId', name: 'EmployerRegistration', component: EmployerRegistration, props: true },
    { path: '/employer-registration', redirect: '/login' },
    { path: '/unauthorized', name: 'UnauthorizedView', component: UnauthorizedView },
    {
        path: '/',
        component: MainLayout,  
        children: [
            { path: '/home-view', name: 'HomeView', component: HomeView, meta: {allowedPositions: ['Employer', 'Collaborator', 'Payroll Manager', 'Supervisor', 'SoftwareManager']} },
            { path: '/add-employee', name: 'AddEmployee', component: AddEmployee, meta: {allowedPositions: ['Employer']} },
            { path: '/view-employee-profile/:id', name: 'ViewEmployeeProfile', component: ViewEmployeeProfile, meta: {allowedPositions: ['Employer', 'Collaborator', 'Payroll Manager', 'Supervisor', 'SoftwareManager']} },
            { path: '/view-companies-list', name: 'ViewCompaniesList', component: ViewCompaniesList, meta: {allowedPositions: ['SoftwareManager']} },
            { path: '/employees-list', name: 'EmployeesList', component: EmployeesList, meta: {allowedPositions: ['Employer','Payroll Manager']} },
            { path: '/view-employee-profile-employer/:id', name: 'ViewEmployeeProfileEmployer', component: ViewEmployeeProfileEmployer, meta: {allowedPositions: ['Employer','Payroll Manager']} },
            { path: '/benefits', name: 'Benefits', component: BenefitList, meta: {allowedPositions: ['Employer','Payroll Manager']} },
            { path: '/benefit/:id', name: 'Benefit', component: Benefit, props: true, meta: {allowedPositions: ['Employer','Payroll Manager']} },
            { path: '/benefit/create', name: 'CreateBenefit', component: BenefitCreate, meta: {allowedPositions: ['Employer']} },
            { path: '/select-change-benefit', name: 'SelectChangeBenefit', component: SelectChangeBenefit, meta: {allowedPositions: ['Employer', 'Collaborator', 'Payroll Manager', 'Supervisor']} },
            { path: '/view-employee-profile', name: 'ViewEmployeeProfile', component: ViewEmployeeProfile, meta: {allowedPositions: ['Employer', 'Collaborator', 'Payroll Manager', 'Supervisor']} },
            { path: '/view-company-info', name: 'ViewCompanyInfo', component: ViewCompanyInfo, meta: {allowedPositions: ['Employer', 'SoftwareManager']} },
            { path: '/payrolls-list', name: 'PayrollsList', component: PayrollsList, meta: {allowedPositions: ['Payroll Manager']} },
            { path: '/employee-timesheet', name: 'EmployeeTimesheet', component: EmployeeTimesheet}
        ],
    },
]

const router = createRouter({
    history: createWebHistory(),
    routes,
})

router.beforeEach((to, from, next) => {
  const publicRoutes = ['LoginForm', 'CompanyRegistration', 'EmployerRegistration', 'UnauthorizedView']

  const user = CurrentUserService.getCurrentUserInformationFromLocalStorage()

  // If trying to access login and already logged in, redirect to home
  if (to.name === 'LoginForm' && user) {
    return next({ name: 'HomeView' })
  }

  // If route is public, allow access
  if (publicRoutes.includes(to.name)) {
    return next()
  }

  // If not logged in, redirect to unauthorized
  if (!user) {
    return next({ name: 'UnauthorizedView' })
  }

  // If route has meta.allowedPositions, check if user's position is allowed
  if (to.meta && to.meta.allowedPositions) {
    if (!to.meta.allowedPositions.includes(user.position)) {
      return next({ name: 'UnauthorizedView' })
    }
  }

  next()
})

export default router