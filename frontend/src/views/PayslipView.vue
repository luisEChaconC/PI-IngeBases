<template>
  <div class="container mt-5">
    <div class="mb-4">
      <router-link
        to="/view-payslip-list"
        class="btn btn-outline-secondary"
        title="Volver al menú principal"
      >
        ← Volver
      </router-link>
    </div>

  <div v-if="payslipData" class="d-flex justify-content-center align-items-center">
    <div class="card shadow-sm p-4 rounded-4" style="width: 500px;">
      <h5 class="fw-bold text-primary mb-1">
        PAGO – {{ payslipData.periodType }} ({{ payslipData.dateRange }})
      </h5>

      <div class="mb-3">
        <strong class="text-primary">{{ payslipData.companyName }}</strong>
      </div>

      <div class="d-flex justify-content-between mb-2">
        <strong class="text-primary">{{ payslipData.employeeName }}</strong>
        <strong class="text-primary">{{ payslipData.contractType }}</strong>
      </div>

      <div class="mb-4">
        <strong class="text-primary">{{ payslipData.paymentMonth }}</strong>
      </div>

      <ul class="list-group mb-3">
        <!-- Salario Bruto -->
        <li
          class="list-group-item d-flex justify-content-between fw-bold"
        >
          <span>Salario Bruto</span>
          <span>{{ formatCurrency(payslipData.grossSalary) }}</span>
        </li>

        <!-- Ítems de deducciones -->
        <li
          v-for="(item, index) in payslipData.items"
          :key="index"
          class="list-group-item d-flex justify-content-between"
          :class="item.bold ? 'fw-bold' : ''"
        >
          <span>{{ item.label }}</span>
          <span>{{ formatCurrency(item.amount) }}</span>
        </li>
      </ul>

      <!-- Pago Neto -->
      <div class="d-flex justify-content-between fw-bold fs-5 mb-4">
        <span>Pago Neto</span>
        <span>{{ formatCurrency(payslipData.netPay) }}</span>
      </div>

      <div class="text-center">
        <button class="btn btn-outline-primary me-2">Enviar por correo</button>
        <button class="btn btn-outline-primary">Descargar PDF</button>
      </div>
    </div>
  </div>
  </div>
</template>

<script setup>
import {ref, onMounted} from 'vue'
import { useRoute } from 'vue-router'
import currentUserService from '@/services/currentUserService';
import payslipService from '@/services/payslipService';

const route = useRoute()
const payslipDate = route.params.date
console.log(payslipDate)

const payslipData = ref(null)

const employeeId = currentUserService.getCurrentUserInformationFromLocalStorage().idNaturalPerson;

const fetchPayslip = async () => {
  try {
    const data = await payslipService.getPayslipByEmployeeIdAndDate(employeeId, payslipDate);
    payslipData.value = data; 
  } catch (error) {
    console.error(error.message);
  }
};

const formatCurrency = (amount) => {
  if (typeof amount !== 'number') return '₡0';
  return new Intl.NumberFormat('es-CR', {
    style: 'currency',
    currency: 'CRC',
    minimumFractionDigits: 0
  }).format(amount)
}


onMounted(fetchPayslip)
</script>

<style scoped>
.card {
  border-radius: 1rem;
}
</style>
