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

<div class="d-flex justify-content-center align-items-center">
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
          <li
            v-for="(item, index) in payslipData.items"
            :key="index"
            class="list-group-item d-flex justify-content-between"
            :class="item.bold ? 'fw-bold' : ''"
          >
            <span>{{ item.label }}</span>
            <span>{{ item.amount }}</span>
          </li>
        </ul>

        <div class="d-flex justify-content-between fw-bold fs-5 mb-4">
          <span>Pago Neto</span>
          <span>{{ payslipData.netPay }}</span>
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
const payslipData = {
  periodType: 'MENSUAL',
  dateRange: '01-06-25 / 30-06-25',
  companyName: 'BLITZ',
  employeeName: 'Andy Francisco Hernandez Rojas',
  contractType: 'Tiempo Completo',
  paymentMonth: 'Junio',
  items: [
    { label: 'Salario Bruto', amount: '₡ 1,000,000' },
    { label: 'SEM (Seguro Enfermedad/Maternidad)', amount: '-₡55,000' },
    { label: 'IVM (Invalidez, Vejez y Muerte)', amount: '-₡41,700' },
    { label: 'Aporte Trabajador Banco Popular', amount: '-₡10,000' },
    { label: 'Impuesto de renta', amount: '-₡7,800' },
    { label: 'Total deducciones obligatorias', amount: '-₡114,500', bold: true },
    { label: 'Pensión complementaria', amount: '-₡120,000' },
    { label: 'Beneficio dental', amount: '-₡40,000' },
    { label: 'Total deducciones voluntarias', amount: '-₡160,000', bold: true }
  ],
  netPay: '₡780,500'
}
</script>

<style scoped>
.card {
  border-radius: 1rem;
}
</style>
