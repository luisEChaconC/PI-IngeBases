<template>
  <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
    <div class="container-fluid">
      <a class="navbar-brand" href="#">Menú</a>
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
            <a class="nav-link" href="#">Empleados</a>
          </li>
          <li v-if="showOption('viewBenefits')" class="nav-item">
            <a class="nav-link" href="#">Beneficios</a>
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
export default {
  name: "MainMenu",
  data() {
    return {
      employeeType: "supervisor", // change different roles
    };
  },
  methods: {
    showOption(option) {
      const permissions = {
        employee: [
          "viewProfile", 
          "viewPayments", 
          "viewWorkedHours", 
          "viewBenefits"],

        employer: [
          "viewProfile",
          "viewCompany",
          "viewEmployees",
          "viewBenefits",
          "viewReportsPayments",
          "viewRoleAssignment",
        ],

        supervisor: [
          "viewProfile",
          "viewPayments",
          "viewWorkedHours",
          "viewBenefits",
          "viewHours",
          "viewRequestHourCorrection",
        ],

        softwareManager: [
          "viewProfile",
          "viewCompanyManagement",
          "viewReports",
        ],

        payrollManager: [
          "viewProfile",
          "viewPayments",
          "viewWorkedHours",
          "viewBenefits",
          "viewGeneratePayroll",
          "viewPayrollHistory",
        ],

      };

      return permissions[this.employeeType].includes(option);
    },
  },
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
  font-size: 24px;
  font-weight: bold;
}

.navbar-nav .nav-item {
  margin-right: 20px;
}

</style>