<template>
  <nav class="navbar navbar-expand-lg bg-white border-bottom px-3 py-2 fixed-top">
    <div class="container">
     <router-link
        to="/home-view"
        class="navbar-brand fw-bold fs-4"
      >
        Inicio
      </router-link>
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#mainNavbar">
        <span class="navbar-toggler-icon"></span>
      </button>

      <div class="collapse navbar-collapse" id="mainNavbar">
        <ul class="navbar-nav me-auto mb-2 mb-lg-0">
          <li v-if="showOption('viewProfile')" class="nav-item">
            <router-link to="/view-employee-profile" class="nav-link">Perfil</router-link>
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
            <router-link to="/view-company-info" class="nav-link">Empresa</router-link>
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
            <router-link
              v-else-if="employeeType === 'Collaborator' || employeeType === 'Supervisor' || employeeType === 'PayrollManager'"
              to="/select-change-benefit"
              class="nav-link"
            >
              Beneficios
            </router-link>
            <a v-else href="#" class="nav-link">Beneficios</a>
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
            <router-link class="nav-link" to="/view-companies-list">Gestión de Empresas</router-link>
          </li>
          <li v-if="showOption('viewReports')" class="nav-item">
            <a class="nav-link" href="#">Reportes</a>
          </li>
          <li v-if="showOption('viewPayrolls')" class="nav-item">
            <router-link to="/payrolls-list" class="nav-link">Planillas</router-link>
          </li>
        </ul>

        <!-- Botón de cerrar sesión -->
        <div class="d-flex">
          <button @click="logout" class="btn btn-dark">
            Cerrar Sesión
          </button>
        </div>
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
      employeeType: "",
    };
  },
  methods: {
    showOption(option) {
      const permissions = {
        Collaborator: [
          "viewProfile", 
          "viewPayments", 
          "viewWorkedHours", 
          "viewBenefits"
        ],
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
          "viewPayrolls",
        ],
      };
      return permissions[this.employeeType]?.includes(option);
    },
    logout() {

      currentUserService.removeCurrentUserInformationFromLocalStorage();

      this.$router.push('/login');
    }
  },
  created() {
    const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage();
    console.log(currentUserService.getCurrentUserInformationFromLocalStorage())
    if (currentUserInformation?.position) {
      this.employeeType = currentUserInformation.position.replace(/\s/g, '');
    }
    console.log("Rol del usuario:", this.employeeType);
  }
};
</script>

<style scoped>
.navbar-nav .nav-link {
  font-weight: 500;
  color: #333;
  transition: color 0.2s ease;
}

.navbar-nav .nav-link:hover {
  color: #0d6efd;
  text-decoration: underline;
}

.navbar-toggler {
  border-color: rgba(0, 0, 0, 0.1);
}

.navbar {
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}
</style>