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

       <!-- Request-Hours-Correction card -->
       <div v-if="showOption('viewRequestHoursCorrection')" class="col-md-3">
        <div class="card text-center h-100 shadow-sm">
          <div class="card-body d-flex flex-column justify-content-between">
            <h5 class="card-title">Solicitar Corrección en Horas</h5>
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
      employeeType: "employee", // change different roles
    };
  },
  methods: {
    showOption(option) {
      const permissions = {
        employee: ["viewProfile", "viewPayments", "viewWorkedHours", "viewBenefits"],
        supervisor: [
          "viewProfile",
          "viewPayments",
          "viewWorkedHours",
          "viewBenefits",
          "viewHours",
          "viewRequestHoursCorrection",
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
}
</style>