<template>
  <div class="container mt-5">
    <h2 class="mb-3">Exportar a Excel</h2>

    <form @submit.prevent="exportar">
      <div class="mb-2">
        <label>Nombre:</label>
        <input v-model="nombre" class="form-control" required />
      </div>

      <div class="mb-2">
        <label>Fecha de ingreso:</label>
        <input v-model="fecha" type="date" class="form-control" required />
      </div>

      <div class="mb-2">
        <label>Salario bruto (₡):</label>
        <input v-model.number="salario" type="number" class="form-control" required />
      </div>

      <div class="mb-2">
        <label>Aporte voluntario (₡):</label>
        <input v-model.number="aporte" type="number" step="0.01" class="form-control" required />
      </div>

      <div class="mb-2">
        <label>Edad:</label>
        <input v-model.number="edad" type="number" class="form-control" required />
      </div>

      <button type="submit" class="btn btn-primary mt-3">Generar Excel</button>
    </form>
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "ExportExcelForm",
  data() {
    return {
      nombre: "",
      fecha: "",
      salario: null,
      aporte: null,
      edad: null,
    };
  },
  methods: {
    async exportar() {
      const payload = {
        sheetName: "PruebaTipos",
        columns: ["Nombre", "FechaIngreso", "SalarioBruto", "AporteVoluntario", "Edad"],
        rows: [
          [
            this.nombre,
            this.fecha,
            this.salario,
            this.aporte,
            this.edad
          ]
        ]
      };

      try {
        const response = await axios.post("https://localhost:5000/api/report/excel", payload, {
          responseType: "blob", 
        });

        const blob = new Blob([response.data], {
          type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        });

        const url = window.URL.createObjectURL(blob);
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", "PruebaTipos.xlsx");
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      } catch (error) {
        console.error("Error exportando:", error);
        alert("Ocurrió un error al generar el Excel.");
      }
    },
  },
};
</script>

<style scoped>
.container {
  max-width: 500px;
}
</style>
