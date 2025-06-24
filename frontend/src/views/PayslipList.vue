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

    <table class="table table-bordered">
      <thead class="table-light">
        <tr class="text-center align-middle">
          <th>Tipo de reporte</th>
          <th>Fecha de pago</th>
          <th>Monto total</th>
          <th>Detalle</th>
        </tr>
      </thead>
      <tbody class="text-center align-middle">
        <tr
          v-for="(payslip, index) in filteredPayslips"
          :key="index"
        >
          <td>Colilla de pago – {{ payslip.periodType }}</td>
          <td>{{ payslip.date }}</td>
          <td>{{ payslip.total }}</td>
          <td>
          <router-link
            :to="`/view-payslip/`"
            class="btn btn-outline-dark"
            title="Ver detalles"
          >
            <i class="fas fa-eye"></i>
          </router-link>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue';

const searchDate = ref('');

const payslips = ref([
  {
    id: 1,
    periodType: 'SEMANAL',
    date: '01-06-25',
    total: '₡780,500'
  },
  {
    id: 2,
    periodType: 'QUINCENAL',
    date: '15-06-25',
    total: '₡895,200'
  },
  {
    id: 3,
    periodType: 'MENSUAL',
    date: '30-06-25',
    total: '₡1,050,000'
  }
]);

const filteredPayslips = computed(() => {
  if (!searchDate.value.trim()) {
    return payslips.value;
  }

  return payslips.value.filter(p =>
    p.date.includes(searchDate.value.trim())
  );
});
</script>

<style scoped>

</style>
