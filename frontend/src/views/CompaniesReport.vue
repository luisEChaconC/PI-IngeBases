<template>
  <div class="p-4">
    <h1 class="text-2xl font-bold mb-4">Historial de pagos por empresa</h1>

    <!-- Filtros de fecha -->
    <div class="mb-4 bg-gray-300 p-6 rounded space-y-4">
      <div class="grid grid-cols-1 sm:grid-cols-3 gap-4 items-center">
        <!-- Fecha de inicio -->
        <div class="flex flex-col">
          <label for="startDate" class="mb-1 font-semibold">Fecha inicio:</label>
          <input
            type="date"
            id="startDate"
            v-model="startDate"
            class="border rounded px-3 py-2"
          />
        </div>

        <!-- Fecha de fin -->
        <div class="flex flex-col">
          <label for="endDate" class="mb-1 font-semibold">Fecha fin:</label>
          <input
            type="date"
            id="endDate"
            v-model="endDate"
            class="border rounded px-3 py-2"
          />
        </div>

        <!-- Botón de búsqueda -->
        <div class="flex items-end">
          <button
            @click="searchByDate"
            class="w-full bg-gray-500 text-black font-bold px-4 py-2 rounded hover:bg-gray-500 transition"
            :disabled="loading"
          >
            Buscar
          </button>
        </div>
      </div>
    </div>

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
import companyReportService from '@/services/companyReportService';

export default {
  name: "TablaEmpresas",
  data() {
    return {
      empresas: [],
      loading: false,
      error: null,
      startDate: '',
      endDate: ''
    };
  },
  mounted() {
    this.fetchAllReports();
  },
  methods: {
    translateFrecuenciaPago(value) {
      switch (value) {
        case "Monthly":
          return "Mensual";
        case "Biweekly":
          return "Quincenal";
        case "Weekly":
          return "Semanal";
        default:
          return value;
      }
    },
    async fetchAllReports() {
      this.loading = true;
      this.error = null;
      try {
        const data = await companyReportService.getAllCompanyReports();
    
        this.empresas = data.rows.map(row => ({
          nombre: row[data.columns.indexOf("Nombre")],
          frecuenciaPago: this.translateFrecuenciaPago(row[data.columns.indexOf("FrecuenciaPago")]),
          periodoPago: row[data.columns.indexOf("PeriodoPago")],
          fechaPago: row[data.columns.indexOf("FechaPago")],
          salarioBruto: row[data.columns.indexOf("SalarioBruto")] ?? 0,
          cargasSociales: row[data.columns.indexOf("CargasSociales")] ?? 0,
          deducciones: row[data.columns.indexOf("DeduccionesVoluntarias")] ?? 0,
          costoEmpleador: row[data.columns.indexOf("CostoEmpleador")] ?? 0,
        }));
      } catch (err) {
        this.error = err.message || 'Error al cargar los reportes';
      } finally {
        this.loading = false;
      }
    },
    async fetchReportsByDate(startDate, endDate) {
      this.loading = true;
      this.error = null;
      try {
        const data = await companyReportService.getCompanyReportsByDate(startDate, endDate);
    
        this.empresas = data.rows.map(row => ({
          nombre: row[data.columns.indexOf("Nombre")],
          frecuenciaPago: this.translateFrecuenciaPago(row[data.columns.indexOf("FrecuenciaPago")]),
          periodoPago: row[data.columns.indexOf("PeriodoPago")],
          fechaPago: row[data.columns.indexOf("FechaPago")],
          salarioBruto: row[data.columns.indexOf("SalarioBruto")] ?? 0,
          cargasSociales: row[data.columns.indexOf("CargasSociales")] ?? 0,
          deducciones: row[data.columns.indexOf("DeduccionesVoluntarias")] ?? 0,
          costoEmpleador: row[data.columns.indexOf("CostoEmpleador")] ?? 0,
        }));
      } catch (err) {
        this.error = err.message || 'Error al cargar los reportes';
      } finally {
        this.loading = false;
      }
    },
    searchByDate() {
      if (!this.startDate || !this.endDate) {
        this.error = "Por favor ingrese ambas fechas.";
        return;
      }
      this.fetchReportsByDate(this.startDate, this.endDate);
    }
  }
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
