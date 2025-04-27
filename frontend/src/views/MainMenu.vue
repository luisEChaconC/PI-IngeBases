<template>
  <div class="container d-flex align-items-center justify-content-center vh-100">
    <div class="row g-2 justify-content-center">
      <!-- Profile card -->
      <div v-if="showOption('viewProfile')" :class="[getColumnSize(), {'softwareManagerCard': employeeType === 'softwareManager'}]">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h2 class="card-title">Perfil</h2>
            <button class="btn btn-dark align-self-start mt-auto" >Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Worked-hours card -->
      <div v-if="showOption('viewWorkedHours')" :class="getColumnSize()">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h2 class="card-title">Horas<br>Laboradas</h2>
            <button class="btn btn-dark align-self-start mt-auto" >Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Generate payroll card -->
      <div v-if="showOption('viewGeneratePayroll')" :class="getColumnSize()">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h2 class="card-title">Generar<br>Planillas</h2>
            <button class="btn btn-dark align-self-start mt-auto" >Ir al sitio</button>
          </div>
        </div>
      </div>

       <!-- Hours card -->
       <div v-if="showOption('viewHours')" :class="getColumnSize()">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h2 class="card-title">Horas</h2>
            <button class="btn btn-dark align-self-start mt-auto" >Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Payments card -->
      <div v-if="showOption('viewPayments')" :class="getColumnSize()">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h2 class="card-title">Pagos</h2>
            <button class="btn btn-dark align-self-start mt-auto" >Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Company card -->
      <div v-if="showOption('viewCompany')" :class="getColumnSize()">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h2 class="card-title">Empresa</h2>
            <button class="btn btn-dark align-self-start mt-auto" >Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Employees card -->
      <div v-if="showOption('viewEmployees')" :class="getColumnSize()">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h2 class="card-title">Empleados</h2>
            <button class="btn btn-dark align-self-start mt-auto" >Ir al sitio</button>
          </div>
        </div>
      </div>
      
      <!-- Benefits card -->
      <div v-if="showOption('viewBenefits')" :class="getColumnSize()">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h2 class="card-title">Beneficios</h2>
            <button class="btn btn-dark align-self-start mt-auto" >Ir al sitio</button>
          </div>
        </div>
      </div>

     

      <!-- Request-Hour-Correction card -->
      <div v-if="showOption('viewRequestHourCorrection')" :class="getColumnSize()">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h2 class="card-title mb-3">Solicitar<br>Corrección<br>en Horas</h2>
            <button class="btn btn-dark align-self-start mt-auto" >Ir al sitio</button>
          </div>
        </div>
      </div>

     

      <!-- Reports and payments card -->
      <div v-if="showOption('viewReportsPayments')" :class="getColumnSize()">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h2 class="card-title">Reportes y<br>Pagos</h2>
            <button class="btn btn-dark align-self-start mt-auto" >Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Role assignment card -->
      <div v-if="showOption('viewRoleAssignment')" :class="getColumnSize()">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h2 class="card-title">Asignación<br>de Roles</h2>
            <button class="btn btn-dark align-self-start mt-auto" >Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Company Management card -->
      <div v-if="showOption('viewCompanyManagement')" :class="[getColumnSize(), {'softwareManagerCard': employeeType === 'softwareManager'}]">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h2 class="card-title">Gestión de<br>empresas</h2>
            <button class="btn btn-dark align-self-start mt-auto" >Ir al sitio</button>
          </div>
        </div>
      </div>

      <!-- Reports card -->
      <div v-if="showOption('viewCompanyManagement')" :class="[getColumnSize(), {'softwareManagerCard': employeeType === 'softwareManager'}]">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h2 class="card-title">Reportes</h2>
            <button class="btn btn-dark align-self-start mt-auto" >Ir al sitio</button>
          </div>
        </div>
      </div>

    

      <!-- Payroll History card -->
      <div v-if="showOption('viewPayrollHistory')" :class="getColumnSize()">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h2 class="card-title">Historial de<br>Planillas</h2>
            <button class="btn btn-dark align-self-start mt-auto" >Ir al sitio</button>
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
      employeeType: "softwareManager", // change different roles
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

    getColumnSize() {
    // Define column size based on the type of employee
      const colSizes = {
        employee: "col-md-6", // 2x2 display
        employer: "col-md-4", // 2x3 display
        supervisor: "col-md-4", // 2x3 display
        softwareManager: "col-md-4", // 1x3 display
        payrollManager: "col-md-4", // 2x3 display
      };
    return colSizes[this.employeeType] || "col-md-4"; // Default size
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
  width: 85%;
}

.softwareManagerCard {
  width: 100%; /* % is used to adjust fill*/
  width: 400px;
  margin-left: 30px;
}

.card-title {
  font-weight: bold;
  color: #000000;
  text-align: left;
  flex-grow: 1;
  margin-bottom: 45px;
  margin-top: 10px;
  margin-left: 8px;
  min-height: 75px;
}

.card-body {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  height: 100%;
}
.card:hover {
  transform: scale(1.05);
  background-color: rgb(235, 234, 234);
  border-color: black;
}

.row {
  row-gap: 30px;
}

.btn-dark {
  color: white;
  padding-left:40px;
  padding-right:40px;
  margin-left: 8px;
  margin-bottom: 8px;
}
</style>