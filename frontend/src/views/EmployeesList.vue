<template>
  <div class="container my-5">
    <div class="position-relative mb-4">
      <router-link
        to="/home-view"
        class="btn btn-outline-secondary"
        title="Volver al menú principal"
      >
        ← Volver
      </router-link>
    </div>
    <div class="card shadow">
      <div class="card-body">
        <div class="d-flex justify-content-between mx-2 mb-4">
          <h2 class="card-title">Empleados</h2>
          <a href="/add-employee" class="btn btn-dark">
            <i class="fas fa-user-plus me-2"/>
            Nuevo Empleado
          </a>
        </div>
        <div class="table-responsive" style="max-height: 600px overflow-y: auto">
          <table class="table table-striped table-bordered">
            <thead class="table-dark sticky-header">
              <tr>
                <th>Nombre completo</th>
                <th>Cédula</th>
                <th>Posición</th>
                <th>Acciones</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(employee, index) in employees" :key="index">
                <td>{{ employee.fullName }}</td>
                <td>{{ employee.legalId }}</td>
                <td>{{ employee.position }}</td>
                <td class="d-flex justify-content-center">
                  <a :href="'/view-employee-profile-employer/' + employee.idEmployee" class="btn btn-outline-dark btn-sm">
                    <i class="fas fa-eye"></i>
                  </a>
                </td>
              </tr>
              <tr v-if="employees.length === 0">
                <td colspan="4" class="text-center">No se encontraron empleados.</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import currentUserService from "@/services/currentUserService";

export default {
  name: 'EmployeesList',
  data() {
    return {
      employees: [] // Initialize as an empty array
    }
  },
  methods: {
    async fetchEmployees() {
      try {
        const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage()
        const companyId = currentUserInformation.companyId
        const response = await axios.get('https://localhost:5000/api/Employee/GetEmployeesByCompanyId', {
          params: { companyId: companyId }
        })
        // Translate positions to Spanish
        this.employees = response.data.map(employee => ({
          ...employee,
          position: this.translatePosition(employee.position)
        }))
      } catch (error) {
        console.error('Error fetching employees:', error)
        this.employees = [] // Reset employees in case of an error
      }
    },
    translatePosition(position) {
      switch (position) {
        case 'Supervisor':
          return 'Supervisor'
        case 'Payroll Manager':
          return 'Encargado de planilla'
        case 'Collaborator':
          return 'Colaborador'
        default:
          return 'Desconocido' // Default for unexpected values
      }
    }
  },
  mounted() {
    this.fetchEmployees() // Fetch employees when the component is mounted
  }
}
</script>

<style scoped>
.card-title {
  font-size: 1.75rem;
  font-weight: bold;
}

.sticky-header th {
  position: sticky;
  top: 0;
  z-index: 2;
  background-color: #343a40 /* Matches the table-dark background */;
  color: white;
}
</style>