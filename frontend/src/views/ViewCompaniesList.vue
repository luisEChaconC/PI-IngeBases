<template>
  <div class="container my-5">
    <div class="position-relative mb-4">
      <router-link
          to="/main-menu"
          class="btn btn-outline-secondary"
          title="Volver al menú principal"
        >
          ← Volver
        </router-link>
    </div>
    <div class="card shadow">
      <div class="card-body">
        <h2 class="card-title mb-4">Empresas</h2>
        <div class="table-responsive" style="max-height: 600px; overflow-y: auto;">
          <table class="table table-striped table-bordered">
            <thead class="table-dark">
              <tr>
                <th>Nombre</th>
                <th>Dueño</th>
                <th>Cédula Jurídica</th>
                <th>Cantidad de empleados</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(company, index) in companies" :key="index">
                <td>{{ company.name }}</td>
                <td>{{ company.creationAuthor }}</td> 
                <td>{{ company.legalId }}</td>

                <td>{{ company.employeeCount }}</td>

              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import axios from 'axios';
import { ref, onMounted } from 'vue';

export default {
  name: 'ViewCompaniesList',
  setup() {
    const companies = ref([]);

    const getCompanies = async () => {
  try {
    const response = await axios.get('https://localhost:5000/api/Company/GetCompanies')
    companies.value = response.data;
  } catch (error) {
    console.error("Error fetching companies:", error);
  }
};

    onMounted(() => {
      getCompanies();
    });

    return {
      companies,
    };
  },
};
</script>

<style scoped>
.card-title {
  font-size: 1.75rem;
  font-weight: bold;
}
</style>
