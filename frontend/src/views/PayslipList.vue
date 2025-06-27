<template>
  <div class="container mt-5">
    <div class="position-relative mb-4">
      <router-link
        to="/home-view"
        class="btn btn-outline-secondary"
        title="Volver al menú principal"
      >
        ← Volver
      </router-link>
    </div>

    <h2 class="mb-4">Colillas de pago</h2>

    <div class="row align-items-center mb-4">
      <div class="col-auto">
        <label for="searchDate" class="form-label fw-bold">Buscar por fecha</label>
      </div>
      <div class="col-auto">
        <input
          id="searchDate"
          type="text"
          class="form-control"
          placeholder="Ej: 01-06-25"
          v-model="searchDate"
        />
      </div>
    </div>

    <!-- Si hay colillas, muestra la tabla -->
<table v-if="filteredPayslips.length > 0" class="table table-bordered">
  <thead class="table-light">
    <tr class="text-center align-middle">
      <th>Tipo de reporte</th>
      <th>Fecha de pago</th>
      <th>Monto total</th>
      <th>Detalle</th>
    </tr>
  </thead>
  <tbody class="text-center align-middle">
    <tr v-for="(payslip, index) in filteredPayslips" :key="index">
      <td>Colilla de pago – {{ payslip.periodType }}</td>
      <td>{{ payslip.dateRange }}</td>
      <td>{{ formatCurrency(payslip.netPay) }}</td>
      <td>
        <router-link
          :to="`/view-payslip/${getStartDateForRouting(payslip.dateRange)}`"
          class="btn btn-outline-dark"
          title="Ver detalles"
        >
          <i class="fas fa-eye"></i>
        </router-link>
      </td>
    </tr>
  </tbody>
</table>

<!-- Si NO hay colillas -->
<div v-else class="alert alert-warning text-center fs-4 fw-bold py-5">
  No hay registros de pago disponibles.
</div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import axios from 'axios'
import currentUserService from '@/services/currentUserService';


const employeeId = currentUserService.getCurrentUserInformationFromLocalStorage().idNaturalPerson;
const searchDate = ref('')
const payslips = ref([])


const fetchPayslips = async () => {
  try {
    const response = await axios.get(`https://localhost:5000/api/payslip/employee/${employeeId}`)
    payslips.value = response.data
    console.log(payslips.value)
  } catch (error) {
    console.error('Error al obtener colillas:', error)
  }
}


onMounted(fetchPayslips)

const filteredPayslips = computed(() => {
  if (!searchDate.value.trim()) return payslips.value
  return payslips.value.filter(p => p.dateRange.includes(searchDate.value.trim()))
})

const formatCurrency = (amount) => {
  return new Intl.NumberFormat('es-CR', {
    style: 'currency',
    currency: 'CRC',
    minimumFractionDigits: 0
  }).format(amount)
}

const getStartDateForRouting = (dateRange) => {
  const startRaw = dateRange.split(' / ')[0] // ej: "26-05-25"
  const [day, month, year2] = startRaw.split('-')
  const yearFull = parseInt(year2, 10) < 50 ? '20' + year2 : '19' + year2
  console.log(`${yearFull}-${month}-${day}`)
  return `${yearFull}-${month}-${day}` // formato YYYY-MM-DD
}
</script>

<style scoped>
</style>
