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
      <h1 class="mb-4">Beneficios</h1>

      <div class="row align-items-center mb-4">
          <div class="col-auto">
              <label for="maxBenefits" class="form-label fw-bold">Cantidad maxima de beneficios</label>
          </div>
          <div class="col-auto">
              <select id="maxBenefits" v-model="maxBenefits" class="form-select">
                  <option v-for="n in 5" :key="n" :value="n">{{ n }}</option>
              </select>
          </div>
          <div class="col text-end">
              <router-link to="/benefit/create" class="btn btn-dark">
                  + Nuevo beneficio
              </router-link>
          </div>
      </div>

      <table class="table table-bordered">
          <thead class="table-light">
              <tr class="text-center align-middle">
                  <th>Nombre</th>
                  <th>Estado</th>
                  <th>Tipo</th>
                  <th>Ver</th>
              </tr>
          </thead>
          <tbody class="text-center align-middle">
              <tr v-for="(benefit, index) in benefits" :key="index">
                  <td>{{ benefit.name }}</td>
                  <td>{{ benefit.isActive ? 'Activo' : 'Inactivo' }}</td>
                  <td>{{ translateType(benefit.type) }}</td>
                  <td class="text-center align-middle">
                      <router-link
                          :to="`/benefit/${benefit.id}`"
                          class="btn btn-dark"
                      >
                          +
                      </router-link>
                  </td>
              </tr>
          </tbody>
      </table>
  </div>
</template>

<script setup>
import axios from "axios";
import { ref, onMounted } from 'vue';
import currentUserService from "@/services/currentUserService";


const maxBenefits = ref(3); 

const getCompanyMaxBenefits = async () => {
  try {
    const companyId = currentUserService.getCurrentUserInformationFromLocalStorage()?.companyId;
    console.log("Company ID obtenido:", companyId);

    const response = await axios.get(`https://localhost:5000/api/company/GetCompanyById/${companyId}`);

    if (response.data?.maxBenefitsPerEmployee !== undefined) {
      maxBenefits.value = response.data.maxBenefitsPerEmployee;
    }
  } catch (error) {
    console.error("Error obteniendo maxBenefitsPerEmployee:", error);
  }
};

const benefits = ref([]);

const translateType = (type) => {
    switch (type) {
        case 'API': return 'API';
        case 'FixedAmount': return 'Monto fijo';
        case 'FixedPercentage': return 'Porcentaje fijo';
        default: return 'Desconocido';
    }
};

const getCurrentUserInformationFromLocalStorage = () => {
    const userInformation = localStorage.getItem('currentUserInformation');
    return userInformation ? JSON.parse(userInformation) : null;
};

const getBenefits = async () => {
    try {
        console.log(getCurrentUserInformationFromLocalStorage())
        const companyId = getCurrentUserInformationFromLocalStorage()?.companyId;
        

        if (!companyId) {
            console.error("No se encontró el ID de la empresa en el localStorage.");
            return;
        }

        const response = await axios.get(`https://localhost:5000/api/benefit?companyId=${companyId}`);
        benefits.value = response.data;
    } catch (error) {
        console.error("No se pudieron obtener los beneficios:", error);
    }
};

onMounted(() => {
    getBenefits();
    getCompanyMaxBenefits();
});
</script>

<style scoped>

</style>