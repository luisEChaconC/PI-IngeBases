<template>
  <div class="p-4">
    <h1 class="text-2xl font-bold mb-4">Historial de pagos por empresa</h1>

    <!-- Sección de filtros de fechas -->
    <div class="contenedor-fechas">
      <div class="filtro-fecha">
        <label for="startDate">📅 Fecha inicio:</label>
        <input type="date" id="startDate" v-model="startDate" />
      </div>

      <div class="filtro-fecha">
        <label for="endDate">📅 Fecha fin:</label>
        <input type="date" id="endDate" v-model="endDate" />
      </div>

      <div class="filtro-fecha" style="justify-content: end;">
        <button @click="searchByDate" class="bg-blue-600 text-black font-semibold px-4 py-2 rounded-lg hover:bg-blue-700 transition disabled:opacity-50 w-full" :disabled="loading">
          🔍 Buscar
        </button>
      </div>
    </div>

    <!-- Botón separado para exportar -->
    <div class="mb-4 flex justify-end">
      <button @click="exportToExcel" class="flex items-center gap-2 bg-green-600 text-black font-semibold px-4 py-2 rounded-lg hover:bg-green-700 transition disabled:opacity-50" :disabled="loading">
        📤 Exportar a Excel
      </button>
    </div>

    <!-- Tabla solo si hay datos -->
    <table v-if="empresas.length > 0" class="min-w-full border border-gray-300">
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

   <div v-if="!loading && empresas.length === 0" class="sin-planillas">
      <span class="emoji">📂</span>
      <h2>No hay planillas en el rango seleccionado</h2>
      <p>Intenta con un rango de fechas diferente o muestra todos los registros</p>
      <button @click="fetchAllReports" class="boton-ver-todos">🔄 Ver todos los registros</button>
    </div>
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
    },

    async exportToExcel() {
      this.loading = true;
      this.error = null;
      try {
   
        const columns = [
          "Nombre", "FrecuenciaPago", "PeriodoPago", "FechaPago",
          "SalarioBruto", "CargasSociales", "DeduccionesVoluntarias", "CostoEmpleador"
        ];
        const rows = this.empresas.map(e => [
          e.nombre,
          e.frecuenciaPago,
          e.periodoPago,
          e.fechaPago,
          e.salarioBruto,
          e.cargasSociales,
          e.deducciones,
          e.costoEmpleador
        ]);
        const payload = {
          sheetName: "HistorialPagosEmpresa",
          columns: columns,
          rows: rows
        };

        const response = await fetch('https://localhost:5000/api/report/excel', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(payload)
        });

        if (!response.ok) throw new Error('Error al exportar el Excel');

        const blob = await response.blob();
        // Descarga el archivo
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', `${payload.sheetName}.xlsx`);
        document.body.appendChild(link);
        link.click();
        link.remove();
        window.URL.revokeObjectURL(url);
      } catch (err) {
        this.error = err.message || 'Error al exportar el Excel';
      } finally {
        this.loading = false;
      }
    },
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

.sin-planillas {
  background-color: #fff9db; 
  color: #5c4400; 
  text-align: center;
  padding: 3rem 2rem;
  border-radius: 12px;
  font-size: 1.2rem;
  border: 1px solid #ffe8a1;
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.05);
  max-width: 600px;
  margin: 2rem auto;
  animation: fadeIn 0.6s ease-in-out;
}

.sin-planillas .emoji {
  font-size: 2.5rem;
  display: block;
  margin-bottom: 0.5rem;
}

.sin-planillas h2 {
  font-size: 1.8rem;
  margin-bottom: 0.5rem;
}

.sin-planillas p {
  font-size: 1rem;
  color: #7a6200;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.filtro-fecha {
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
  margin-bottom: 1rem;
  max-width: 250px;
}


.filtro-fecha label {
  font-weight: 600;
  color: #333;
  font-size: 0.95rem;
}


.filtro-fecha input[type="date"] {
  padding: 0.6rem 1rem;
  border: 1px solid #ccc;
  border-radius: 10px;
  font-size: 1rem;
  outline: none;
  transition: border-color 0.3s ease;
}

.filtro-fecha input[type="date"]:focus {
  border-color: #2563eb; 
  box-shadow: 0 0 0 2px rgba(37, 99, 235, 0.2);
}

.contenedor-fechas {
  display: flex;
  flex-wrap: wrap;
  justify-content: left;
  gap: 1.5rem;
  margin-bottom: 2rem;
}

@media (min-width: 640px) {
  .contenedor-fechas {
    grid-template-columns: repeat(3, 1fr);
  }
}


</style>
