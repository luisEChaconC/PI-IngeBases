<template>
  <div class="report-container">
    <h1>EMPLEADOR (COSTO PLANILLA)</h1>

    <div v-if="loading" class="loading">Cargando...</div>
    <div v-else>
      <!-- Botón PDF -->
      <div class="mt-3 text-end">
          <ExportDropdown 
            element-id="employer-cost-report"
            filename="costo-empleador.pdf"
            email-subject="Costo Empleador"
          />
        </div>

      <div id="employer-cost-report">
        <p><strong class="blue">Nombre de la empresa:</strong> {{ company?.name || '-' }}</p>
        <p><strong class="blue">Nombre del empleador:</strong> {{ employer?.firstName || '-' }} {{ employer?.firstSurname || '' }}</p>
        <p><strong class="blue">Fecha de pago / período de pago:</strong>
          {{ selectedPayrollId ? formatDateRange(getSelectedPayroll?.startDate, getSelectedPayroll?.endDate) : '-' }}
        </p>

        <label for="payrollSelect" class="form-label mt-3">Seleccione una planilla:</label>
        <select v-model="selectedPayrollId" id="payrollSelect" class="form-select mb-4">
          <option v-for="payroll in payrolls" :key="payroll.id" :value="payroll.id">
            {{ formatDateRange(payroll.startDate, payroll.endDate) }}
          </option>
        </select>

        <table>
          <tbody>
            <tr class="bold">
              <td>Total salarios</td>
              <td>₡{{ format(report.privateInsurance) }}</td>
            </tr>

            <tr><td>SEM (Seguro Enfermedad/Maternidad)</td><td>₡{{ format(report.sem) }}</td></tr>
            <tr><td>IVM (Invalidez, Vejez y Muerte)</td><td>₡{{ format(report.ivm) }}</td></tr>
            <tr><td>Cuota Patronal Banco Popular (0.25%)</td><td>₡{{ format(report.bpoP_OtherInstitutions) }}</td></tr>
            <tr><td>Asignaciones Familiares (5.00%)</td><td>₡{{ format(report.familyAllocations) }}</td></tr>
            <tr><td>IMAS (0.50%)</td><td>₡{{ format(report.imas) }}</td></tr>
            <tr><td>INA (1.50%)</td><td>₡{{ format(report.ina) }}</td></tr>
            <tr><td>Aporte Banco Popular (0.25%)</td><td>₡{{ format(report.bpoP_LPT) }}</td></tr>
            <tr><td>FCL - Fondo Capitalización Laboral (1.50%)</td><td>₡{{ format(report.fcl) }}</td></tr>
            <tr><td>Fondo Pensiones Complementarias (2.00%)</td><td>₡{{ format(report.opc) }}</td></tr>
            <tr><td>INS (1.00%)</td><td>₡{{ format(report.ins) }}</td></tr>

            <tr class="highlight">
              <td><strong>Total pagos de ley</strong></td>
              <td><strong>₡{{ format(report.legalDeductionsTotal) }}</strong></td>
            </tr>

            <tr class="total">
              <td><strong>Costo total empleador</strong></td>
              <td><strong>₡{{ format(report.totalEmployerCost) }}</strong></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch, computed } from 'vue'
import employerCostService from '@/services/employerCostService'
import payrollService from '@/services/payrollService'
import currentUserService from '@/services/currentUserService'
import employerService from '@/services/employerService'
import companyService from '@/services/companyService'
import ExportDropdown from '@/components/ExportDropdown.vue'

const report = ref({})
const payrolls = ref([])
const selectedPayrollId = ref(null)
const loading = ref(true)
const employer = ref({})
const company = ref({})

function format(value) {
  return Number(value).toLocaleString('es-CR', {
    style: 'decimal',
    minimumFractionDigits: 2
  })
}

function formatDateRange(start, end) {
  if (!start || !end) return '-'
  const format = (dateStr) =>
    new Date(dateStr).toLocaleDateString('es-CR', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit'
    })
  return `${format(start)} / ${format(end)}`
}

const getSelectedPayroll = computed(() =>
  payrolls.value.find(p => p.id === selectedPayrollId.value)
)

onMounted(async () => {
  try {
    const { idNaturalPerson } = currentUserService.getCurrentUserInformationFromLocalStorage()
    employer.value = await employerService.getEmployerById(idNaturalPerson)
    company.value = await companyService.getCompanyById(employer.value.companyId)
    payrolls.value = await payrollService.getPayrollsByCompanyId(employer.value.companyId)
    if (payrolls.value.length > 0) {
      selectedPayrollId.value = payrolls.value[0].id
    }
  } catch (error) {
    console.error('Error cargando datos iniciales:', error)
  } finally {
    loading.value = false
  }
})

watch(selectedPayrollId, async (payrollId) => {
  if (!payrollId) return
  try {
    report.value = await employerCostService.getByPayrollId(payrollId)
  } catch (err) {
    console.error('Error cargando costo patronal:', err)
  }
})
</script>

<style scoped>
.report-container {
  max-width: 750px;
  margin: auto;
  font-family: 'Segoe UI', sans-serif;
  padding: 2rem;
}

.blue {
  color: #1976d2;
}

table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 1.5rem;
}

td {
  padding: 8px 12px;
  border-bottom: 1px solid #eee;
}

.bold td,
.bold {
  font-weight: bold;
}

.highlight {
  background-color: #f5f5f5;
}

.total {
  font-size: 1.2rem;
  font-weight: bold;
  border-top: 2px solid #000;
}

.loading {
  text-align: center;
  padding: 2rem;
}
</style>
