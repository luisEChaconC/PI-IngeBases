<template>

  <div class="position-relative">
    <router-link
      to="/login"
      class="btn btn-outline-secondary"
      title="Volver al inicio de sesión"
    >
      ← Volver
    </router-link>
  </div>

  <div class="container mb-5">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h3 class="text-primary mb-0">Reporte de pagos por empleado - empleador</h3>
      <div class="d-flex align-items-center">
        <input
          type="text"
          class="form-control me-2"
          placeholder="Filtrar campos..."
          style="height: 60px;"
        >
        <button class="btn btn-outline-primary px-4" style="height: 60px;">
          Generar Excel
        </button>
      </div>
    </div>

    <div class="mb-3">
      <strong class="text-primary">Empresa:</strong> <span>{{ companyName }}</span><br>
      <strong class="text-primary">Empleador:</strong> <span>{{ employerName }}</span>
    </div>

    <table class="table table-bordered table-striped">
      <thead class="table-primary">
        <tr>
          <th>Nombre empleado</th>
          <th>Cédula</th>
          <th>Tipo de empleado</th>
          <th>Periodo de pago</th>
          <th>Fecha de pago</th>
          <th>Salario Bruto</th>
          <th>Cargas sociales empleador</th>
          <th>Deducciones voluntarias</th>
          <th>Costo empleador</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(row, idx) in employees" :key="idx">
          <td>{{ row.employeeName }}</td>
          <td>{{ row.legalId }}</td>
          <td>{{ translateEmployeeType(row.employeeType) }}</td>
          <td>{{ row.paymentPeriod }}</td>
          <td>{{ row.paymentDate }}</td>
          <td class="currency-cell">{{ formatCurrency(row.grossSalary) }}</td>
          <td class="currency-cell">{{ formatCurrency(row.employerSocialCharges) }}</td>
          <td class="currency-cell">{{ formatCurrency(row.voluntaryDeductions) }}</td>
          <td class="currency-cell">{{ formatCurrency(row.employerCost) }}</td>
        </tr>
      </tbody>
      <tfoot>
        <tr class="fw-bold">
          <td colspan="5" class="text-end text-primary">Totales:</td>
          <td class="currency-cell">{{ totalGrossSalary }}</td>
          <td class="currency-cell">{{ totalEmployerSocialCharges }}</td>
          <td class="currency-cell">{{ totalVoluntaryDeductions }}</td>
          <td class="currency-cell">{{ totalEmployerCost }}</td>
        </tr>
      </tfoot>
    </table>
  </div>
</template>

<script>
import EmployerReportsService from '@/services/employerReportsService';
import currentUserService from "@/services/currentUserService";

export default {
  name: "EmployerPayrollReportView",
  data() {
    return {
      companyName: '',
      employerName: '',
      employees: [],
    }
  },
  computed: {
    totalGrossSalary() {
      return this.formatCurrency(this.employees.reduce((sum, r) => sum + (r.grossSalary || 0), 0));
    },
    totalEmployerSocialCharges() {
      return this.formatCurrency(this.employees.reduce((sum, r) => sum + (r.employerSocialCharges || 0), 0));
    },
    totalVoluntaryDeductions() {
      return this.formatCurrency(this.employees.reduce((sum, r) => sum + (r.voluntaryDeductions || 0), 0));
    },
    totalEmployerCost() {
      return this.formatCurrency(this.employees.reduce((sum, r) => sum + (r.employerCost || 0), 0));
    }
  },
  methods: {
    formatCurrency(value) {
      let number = Number(value) || 0;
      // Format: ₡ xxx.xxx.xxx,xx (no break, always 2 decimals)
      return '₡ ' + number
        .toFixed(2) // always 2 decimals
        .replace('.', ',') // decimal with comma
        .replace(/\B(?=(\d{3})+(?!\d))/g, '.'); // thousands with dots
    },
    translateEmployeeType(type) {
      switch (type) {
        case 'Full-Time': return 'Tiempo completo';
        case 'Part-Time': return 'Medio tiempo';
        case 'Professional Services': return 'Servicios profesionales';
        case 'Hourly': return 'Por horas';
        default: return type;
      }
    },
    async fetchReport() {
      const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage()
      const companyId = currentUserInformation.companyId
      const employerId = currentUserInformation.idPerson;
      try {
        const data = await EmployerReportsService.getEmployeePayrollReport(employerId, companyId);
        this.companyName = data.companyName;
        this.employerName = data.employerName;
        this.employees = data.employees;
      } catch (error) {
        this.companyName = '';
        this.employerName = '';
        this.employees = [];
      }
    }
  },
  mounted() {
    this.fetchReport();
  }
}
</script>

<style scoped>
.table th, .table td {
  vertical-align: middle;
  text-align: center;
  font-size: 0.95rem;
}
.currency-cell {
  white-space: nowrap;
  font-size: 0.95rem;
}
.table-primary th {
  background-color: #0d6efd !important;
  color: #fff;
}
.fw-bold {
  font-weight: bold;
}
.text-primary {
  color: #0d6efd !important;
}
</style>