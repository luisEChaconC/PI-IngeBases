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
          <h2 class="card-title">Planillas</h2>
          <button class="btn btn-dark" @click="showModal = true">
            <i class="fas fa-plus me-2" />
            Nueva Planilla
          </button>
        </div>
        <GeneratePayrollModal
          :visible="showModal"
          @close="showModal = false"
        />
        <div class="table-responsive" style="max-height: 600px; overflow-y: auto">
          <table class="table table-striped table-bordered">
            <thead class="table-dark sticky-header">
              <tr>
                <th>Nombre completo del encargado</th>
                <th>Fecha inicio</th>
                <th>Fecha fin</th>
                <th>Monto deducido</th>
                <th>Salario bruto</th>
                <th>Salario neto</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(payroll) in payrolls" :key="payroll.id">
                <td>{{ payroll.payrollManagerFullName }}</td>
                <td>{{ formatDate(payroll.startDate) }}</td>
                <td>{{ formatDate(payroll.endDate) }}</td>
                <td>{{ formatCurrency(payroll.totalAmountDeducted) }}</td>
                <td>{{ formatCurrency(payroll.totalGrossSalary) }}</td>
                <td>{{ formatCurrency(payroll.totalGrossSalary - payroll.totalAmountDeducted) }}</td>
              </tr>
              <tr v-if="payrolls.length === 0">
                <td colspan="6" class="text-center">No se encontraron planillas.</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import currentUserService from "@/services/currentUserService";
import GeneratePayrollModal from "@/components/GeneratePayrollModal.vue";

export default {
  components: { GeneratePayrollModal },
  name: 'PayrollsList',
  data() {
    return {
      payrolls: [],
      showModal: false,
    }
  },
  async mounted() {
    try {
      const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage();
      const companyId = currentUserInformation?.companyId;
      if (!companyId) {
        this.payrolls = [];
        return;
      }
      const response = await axios.get(`https://localhost:5000/api/payroll/company/${companyId}/summary`);
      this.payrolls = response.data;
    } catch (error) {
      this.payrolls = [];
    }
  },
  methods: {
    formatDate(dateStr) {
      if (!dateStr) return '';
      const date = new Date(dateStr);
      return date.toLocaleDateString('es-CR');
    },
    formatCurrency(amount) {
      if (amount == null) return '';
      return '₡ ' + amount.toLocaleString('de-DE', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }
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
  z-index: 10;
}

.table-responsive {
  border-radius: 0.5rem;
}

.table {
  margin-bottom: 0;
}
</style>