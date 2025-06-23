<template>
  <div class="container d-flex justify-content-center align-items-center mt-5 position-relative">
    <div class="card shadow-sm p-4 rounded-4" style="width: 500px;">
      <h5 class="fw-bold text-primary mb-1">PAGO – SEMANAL (01-06-25 / 30-06-25)</h5>
 
      <div class="mb-3">
        <strong class="text-primary">BLITZ</strong>
      </div>

      <div class="d-flex justify-content-between mb-2">
        <strong class="text-primary">Andy Francisco Hernandez Rojas</strong>
        <strong class="text-primary">Tiempo Completo</strong>
      </div>

      <div class="mb-4">
        <strong class="text-primary">Junio</strong>
      </div>

      <ul class="list-group mb-3">
        <li class="list-group-item d-flex justify-content-between">
          <span>Salario Bruto</span>
          <span>₡ 1,000,000</span>
        </li>
        <li class="list-group-item d-flex justify-content-between">
          <span>SEM (Seguro Enfermedad/Maternidad)</span>
          <span>-₡55,000</span>
        </li>
        <li class="list-group-item d-flex justify-content-between">
          <span>IVM (Invalidez, Vejez y Muerte)</span>
          <span>-₡41,700</span>
        </li>
        <li class="list-group-item d-flex justify-content-between">
          <span>Aporte Trabajador Banco Popular</span>
          <span>-₡10,000</span>
        </li>
        <li class="list-group-item d-flex justify-content-between">
          <span>Impuesto de renta</span>
          <span>-₡7,800</span>
        </li>
        <li class="list-group-item d-flex justify-content-between fw-bold">
          <span>Total deducciones obligatorias</span>
          <span>-₡114,500</span>
        </li>
        <li class="list-group-item d-flex justify-content-between">
          <span>Pensión complementaria</span>
          <span>-₡120,000</span>
        </li>
        <li class="list-group-item d-flex justify-content-between">
          <span>Beneficio dental</span>
          <span>-₡40,000</span>
        </li>
        <li class="list-group-item d-flex justify-content-between fw-bold">
          <span>Total deducciones voluntarias</span>
          <span>-₡160,000</span>
        </li>
      </ul>

      <div class="d-flex justify-content-between fw-bold fs-5 mb-4">
        <span>Pago Neto</span>
        <span>₡780,500</span>
      </div>

      <div class="text-center">
        <button class="btn btn-outline-primary">Enviar por correo</button>
      </div>
    </div>
  </div>
</template>

<script setup>

</script>

<style scoped>
.card {
  border-radius: 1rem;
}
</style>
