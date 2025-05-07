<template>
  <nav class="navbar navbar-expand-lg navbar-light bg-light custom-navbar">
    <div class="container-fluid">
      <a class="navbar-brand" href="#">Inicio</a>
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarOptions">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarOptions">
        <ul class="navbar-nav me-auto mb-2 mb-lg-0">
          <!-- Show options depending on the role-->
          <li v-if="showOption('viewProfile')" class="nav-item">
            <a class="nav-link" href="#">Perfil</a>
          </li>
          <li v-if="showOption('viewWorkedHours')" class="nav-item">
            <a class="nav-link" href="#">Horas Laboradas</a>
          </li>
          <li v-if="showOption('viewGeneratePayroll')" class="nav-item">
            <a class="nav-link" href="#">Generar Planillas</a>
          </li>
          <li v-if="showOption('viewHours')" class="nav-item">
            <a class="nav-link" href="#">Horas</a>
          </li>
          <li v-if="showOption('viewPayments')" class="nav-item">
            <a class="nav-link" href="#">Pagos</a>
          </li>
          <li v-if="showOption('viewCompany')" class="nav-item">
            <a class="nav-link" href="#">Empresa</a>
          </li>
          <li v-if="showOption('viewEmployees')" class="nav-item">
            <router-link to="/employees-list" class="nav-link">Empleados</router-link>
          </li>

          <li v-if="showOption('viewBenefits')" class="nav-item">
            <router-link
              v-if="employeeType === 'Employer'"
              to="/benefits"
              class="nav-link"
            >
              Beneficios
            </router-link>
            <a
              v-else
              class="nav-link"
              href="#"
            >
              Beneficios
            </a>
          </li>

          <li v-if="showOption('viewRequestHourCorrection')" class="nav-item">
            <a class="nav-link" href="#">Corrección Horas</a>
          </li>
          <li v-if="showOption('viewReportsPayments')" class="nav-item">
            <a class="nav-link" href="#">Reportes y Pagos</a>
          </li>
          <li v-if="showOption('viewRoleAssignment')" class="nav-item">
            <a class="nav-link" href="#">Asignación de Roles</a>
          </li>
          <li v-if="showOption('viewCompanyManagement')" class="nav-item">
            <a class="nav-link" href="#">Gestión de Empresas</a>
          </li>
          <li v-if="showOption('viewReports')" class="nav-item">
            <a class="nav-link" href="#">Reportes</a>
          </li>
          <li v-if="showOption('viewPayrollHistory')" class="nav-item">
            <a class="nav-link" href="#">Historial de Planillas</a>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>

<script>
import currentUserService from "@/services/currentUserService";

export default {
  name: "MainMenu",
  data() {
    return {
      employeeType: "", // se obtiene dinámicamente
    };
  },
  methods: {
    showOption(option) {
      const permissions = {
        Employee: [
          "viewProfile", 
          "viewPayments", 
          "viewWorkedHours", 
          "viewBenefits"],

        Employer: [
          "viewProfile",
          "viewCompany",
          "viewEmployees",
          "viewBenefits",
          "viewReportsPayments",
          "viewRoleAssignment",
        ],

        Supervisor: [
          "viewProfile",
          "viewPayments",
          "viewWorkedHours",
          "viewBenefits",
          "viewHours",
          "viewRequestHourCorrection",
        ],

        SoftwareManager: [
          "viewProfile",
          "viewCompanyManagement",
          "viewReports",
        ],

        PayrollManager: [
          "viewProfile",
          "viewPayments",
          "viewWorkedHours",
          "viewBenefits",
          "viewGeneratePayroll",
          "viewPayrollHistory",
        ],
      };

      return permissions[this.employeeType]?.includes(option);
    },
  },

  created() {
    const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage();
    this.employeeType = currentUserInformation.position; // o currentUserInformation.role, dependiendo del nombre exacto
    console.log("Rol del usuario:", this.employeeType);
  }
};
</script>

<style scoped>
.container {
  height: 100vh;
}


.row {
  row-gap: 30px;
}

.navbar {
  margin-bottom: 20px;
}

.navbar-brand {
  font-size: 30px;
  margin-right: 30px;
}

.navbar-brand:hover {
  text-decoration: underline;
  transition: all 0.3s ease;
  padding: 8px 16px; 
}


.navbar-nav .nav-item {
  margin-right: 30px;
  font-size: 30px;
}

.navbar .nav-link:hover {
  text-decoration: underline;
  transition: all 0.3s ease;
  padding: 8px 16px; 
}

.custom-navbar {
  background-color: #d3d3d3c0 !important;
  border-bottom: 1px solid #e0e0e0;
}
</style>