<template>
  <div class="position-relative">
    <router-link
      to="/home-view"
      class="btn btn-outline-secondary position-absolute top-0 start-0 mt-2 ms-3"
      title="Volver al menú principal"
    >
      ← Volver
    </router-link>

    <div class="container pt-5 my-5">
      <div class="card shadow">
        <div class="card-body">
          <h2 class="card-title mb-4">Empresas</h2>
          <div class="table-responsive" id="table-to-export" style="max-height: 600px; overflow-y: auto;">
            <table class="table table-striped table-bordered">
              <thead class="table-dark">
                <tr>
                  <th>Nombre</th>
                  <th>Dueño</th>
                  <th>Cédula Jurídica</th>
                  <th>Cantidad de empleados</th>
                  <th>Ver empresa</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(company, index) in companies.filter(c => !c.isDeleted)" :key="index">
                  <td>{{ company.name }}</td>
                  <td>{{ company.creationAuthor }}</td>
                  <td>{{ company.legalId }}</td>
                  <td>{{ company.employeeCount }}</td>
                  <td class="text-center">
                    <button class="btn btn-outline-dark btn-sm" title="Ver información de la empresa" @click="verEmpresa(company.id)">
                      <i class="fas fa-eye" style="color: #000000;"></i>
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <div class="mt-3">
            <ExportDropdown 
              element-id="table-to-export"
              filename="empresas.pdf"
              email-subject="Lista de Empresas"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
  
</template>

<script>
import companyService from '@/services/companyService';
import ExportDropdown from '@/components/ExportDropdown.vue';

export default {
  name: 'ViewCompaniesListPdf',
  components: {
    ExportDropdown,
  },
  data() {
    return {
      companies: [],
    };
  },
  methods: {
    async getCompanies() {
      try {
        this.companies = await companyService.getAllCompanies();
      } catch (error) {
        console.error("Error fetching companies:", error);
      }
    },
    verEmpresa(companyId) {
      localStorage.setItem("selectedCompanyId", companyId);
      this.$router.push('/view-company-info');
    },
  },
  mounted() {
    this.getCompanies();
  },
};
</script>


<style scoped>
.card-title {
  font-size: 1.75rem;
  font-weight: bold;
}
</style>
