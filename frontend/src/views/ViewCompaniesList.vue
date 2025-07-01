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
          <div class="mt-3 text-end">
            <button class="btn btn-primary" @click="exportToPdf">Exportar a PDF</button>
          </div>
        </div>
      </div>
    </div>
  </div>
  
</template>

<script>
import companyService from '@/services/companyService';
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { generatePdfFromElement } from '@/utils/fileUtils';

export default {
  name: 'ViewCompaniesList',
  setup() {
    const companies = ref([]);
    const router = useRouter(); 

    const getCompanies = async () => {
      try {
        companies.value = await companyService.getAllCompanies();
      } catch (error) {
        console.error("Error fetching companies:", error);
      }
    };

    const verEmpresa = (companyId) => {
      localStorage.setItem("selectedCompanyId", companyId); 
      router.push('/view-company-info'); 
    };

    const exportToPdf = async () => {
      try {
        const tableElement = document.getElementById('table-to-export');
        if (!tableElement) {
          console.error("Table element not found!");
          return;
        }
        console.log("Table element found:", tableElement);
        const pdf = await generatePdfFromElement(tableElement, 'empresas.pdf');
        pdf.triggerUserDownload();
      } catch (error) {
        console.error("Error exporting to PDF:", error);
      }
    };

    onMounted(() => {
      getCompanies();
    });

    return {
      companies,
      verEmpresa,
      exportToPdf,
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
