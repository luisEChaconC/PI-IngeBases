<template>
  <div class="container d-flex align-items-center justify-content-center vh-100">
    <div class="row g-4 justify-content-center">
      <!-- Profile card -->
      <div v-if="showOption('viewProfile')" class="col-md-3">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h5 class="card-title">Perfil</h5>
            <button class="btn btn-dark mt-3" mt-auto>Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Payments card -->
      <div v-if="showOption('viewPayments')" class="col-md-3">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h5 class="card-title">Pagos</h5>
            <button class="btn btn-dark mt-3" mt-auto>Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Worked-hours card -->
      <div v-if="showOption('viewWorkedHours')" class="col-md-3">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h5 class="card-title">Horas Laboradas</h5>
            <button class="btn btn-dark mt-3" mt-auto>Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Benefits card -->
      <div v-if="showOption('viewBenefits')" class="col-md-3">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h5 class="card-title">Beneficios</h5>
            <button class="btn btn-dark mt-3" mt-auto>Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Hours card -->
      <div v-if="showOption('viewHours')" class="col-md-3">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h5 class="card-title">Horas</h5>
            <button class="btn btn-dark mt-3" mt-auto>Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Request-Hour-Correction card -->
      <div v-if="showOption('viewRequestHourCorrection')" class="col-md-3">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h5 class="card-title">Solicitar Corrección en Horas</h5>
            <button class="btn btn-dark mt-3" mt-auto>Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Company card -->
      <div v-if="showOption('viewCompany')" class="col-md-3">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h5 class="card-title">Empresa</h5>
            <button class="btn btn-dark mt-3" mt-auto>Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Employees card -->
      <div v-if="showOption('viewEmployees')" class="col-md-3">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h5 class="card-title">Empleados</h5>
            <button class="btn btn-dark mt-3" mt-auto>Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Reports and payments card -->
      <div v-if="showOption('viewReportsPayments')" class="col-md-3">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h5 class="card-title">Reportes y Pagos</h5>
            <button class="btn btn-dark mt-3" mt-auto>Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Role assignment card -->
      <div v-if="showOption('viewRoleAssignment')" class="col-md-3">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h5 class="card-title">Asignación de Roles</h5>
            <button class="btn btn-dark mt-3" mt-auto>Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Company Management card -->
      <div v-if="showOption('viewCompanyManagement')" class="col-md-3">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h5 class="card-title">Gestión de empresas</h5>
            <button class="btn btn-dark mt-3" mt-auto>Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Reports card -->
      <div v-if="showOption('viewReports')" class="col-md-3">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h5 class="card-title">Reportes</h5>
            <button class="btn btn-dark mt-3" mt-auto>Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Generate payroll card -->
      <div v-if="showOption('viewGeneratePayroll')" class="col-md-3">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h5 class="card-title">Generar Planillas</h5>
            <button class="btn btn-dark mt-3" mt-auto>Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Payroll History card -->
      <div v-if="showOption('viewPayrollHistory')" class="col-md-3">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h5 class="card-title">Historial de Planillas</h5>
            <button class="btn btn-dark mt-3" mt-auto>Ir al sitio</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "MainMenu",
  data() {
    return {
      employeeType: "payrollManager", // change different roles
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
.card {
  cursor: pointer;
  transition: transform 0.2s ease;
}
.card:hover {
  transform: scale(1.05);
  background-color: rgb(221, 221, 221);
  border-color: black;
}
</style>