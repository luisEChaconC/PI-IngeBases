<template>
  <div class="p-4">
    <h1 class="text-2xl font-bold mb-4">Historial de pagos por empresa</h1>
    <table class="min-w-full border border-gray-300">
      <thead class="bg-gray-200">
        <tr>
          <th class="border px-4 py-2">Nombre de la empresa</th>
          <th class="border px-4 py-2">Frecuencia de pago</th>
          <th class="border px-4 py-2">Periodo de pago</th>
          <th class="border px-4 py-2">Fecha de pago</th>
          <th class="border px-4 py-2">Salario Bruto</th>
          <th class="border px-4 py-2">Cargas sociales empleador</th>
          <th class="border px-4 py-2">Deducciones voluntarias</th>
          <th class="border px-4 py-2">Costo empleador</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(empresa, index) in empresas" :key="index" class="text-center">
          <td class="border px-4 py-2">{{ empresa.nombre }}</td>
          <td class="border px-4 py-2">{{ empresa.frecuenciaPago }}</td>
          <td class="border px-4 py-2">{{ empresa.periodoPago }}</td>
          <td class="border px-4 py-2">{{ empresa.fechaPago }}</td>
          <td class="border px-4 py-2">₡{{ empresa.salarioBruto.toLocaleString() }}</td>
          <td class="border px-4 py-2">₡{{ empresa.cargasSociales.toLocaleString() }}</td>
          <td class="border px-4 py-2">₡{{ empresa.deducciones.toLocaleString() }}</td>
          <td class="border px-4 py-2">₡{{ empresa.costoEmpleador.toLocaleString() }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  name: "TablaEmpresas",
  data() {
    return {
      empresas: [
        {
          nombre: "Aron Corporation",
          frecuenciaPago: "Quincenal",
          periodoPago: "1 al 15",
          fechaPago: "15/06/2025",
          salarioBruto: 500000,
          cargasSociales: 135000,
          deducciones: 20000,
          costoEmpleador: 635000,
        },
        {
          nombre: "Italy S.A.",
          frecuenciaPago: "Mensual",
          periodoPago: "1 al 30",
          fechaPago: "30/06/2025",
          salarioBruto: 750000,
          cargasSociales: 202500,
          deducciones: 35000,
          costoEmpleador: 952500,
        },
        {
          nombre: "Games S.A.",
          frecuenciaPago: "Semanal",
          periodoPago: "17 al 23",
          fechaPago: "23/06/2025",
          salarioBruto: 250000,
          cargasSociales: 67500,
          deducciones: 10000,
          costoEmpleador: 317500,
        },
      ],
    };
  },
};
</script>

<style scoped>
table {
  border-collapse: collapse;
  width: 100%;
}
th,
td {
  text-align: left;
}

th {
  background-color: #f3f4f6;
}
</style>
