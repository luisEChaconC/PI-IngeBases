<template>
  <div class="d-flex justify-content-center align-items-center position-relative" style="min-height: calc(100vh - 200px);">
    <router-link
      to="/home-view"
      class="btn btn-outline-secondary position-absolute top-0 start-0 m-3"
      title="Volver"
    >
      ← Volver
    </router-link>
      <div v-if="loading" class="text-center">
        <div class="spinner-border" role="status">
          <span class="visually-hidden">Cargando...</span>
        </div>
        <p class="mt-2">Cargando información del empleado...</p>
      </div>
      <div v-else-if="error" class="alert alert-danger">
        {{ error }}
      </div>
      <div v-else class="card w-50">
        <div class="card-body">
          <h2 class="card-title">
            Perfil de {{ userPosition === 'Employer' ? 'Empleador' : 'Empleado' }}
          </h2>
          <div class="row">
            <div class="col-md-6">
              <div class="mb-3">
                <label for="firstName" class="form-label">Nombre</label>
                <input type="text" class="form-control" id="firstName" v-model="employee.firstName" disabled />
              </div>
              <div class="mb-3">
                <label for="secondLastName" class="form-label">Segundo apellido</label>
                <input type="text" class="form-control" id="secondLastName" v-model="employee.secondLastName" disabled />
              </div>
              <div v-if="userPosition !== 'Employer'" class="mb-3">
                <label for="workerId" class="form-label">ID Trabajador</label>
                <input type="text" class="form-control" id="workerId" v-model="employee.workerId" disabled />
              </div>
  
              <div v-if="userPosition !== 'Employer'" class="mb-3">
                <label for="contractType" class="form-label">Tipo de Contrato</label>
                <input type="text" class="form-control" id="contractType" v-model="employee.contractType" disabled />
              </div>
              <div class="mb-3">
                <label for="email" class="form-label">Email</label>
                <input type="email" class="form-control" id="email" v-model="employee.email" disabled />
              </div>
            </div>
            <div class="col-md-6">
              <div class="mb-3">
                <label for="firstLastName" class="form-label">Primer apellido</label>
                <input type="text" class="form-control" id="firstLastName" v-model="employee.firstLastName" disabled />
              </div>
              <div class="mb-3">
                <label for="identityCard" class="form-label">Cédula</label>
                <input type="text" class="form-control" id="identityCard" v-model="employee.identityCard" disabled />
              </div>
              <div class="mb-3">
                <label for="role" class="form-label">Rol</label>
                <input type="text" class="form-control" id="role" v-model="employee.role" disabled />
              </div>
              <div v-if="userPosition !== 'Employer'" class="mb-3">
                <label for="grossSalary" class="form-label">Salario Bruto</label>
                <div class="input-group">
                  <span class="input-group-text">₡</span>
                  <input type="number" class="form-control" id="grossSalary" v-model="employee.grossSalary" disabled />
                </div>
              </div>
              <div class="mb-3">
                <label for="phone" class="form-label">Número de Teléfono</label>
                <input type="tel" class="form-control" id="phone" v-model="employee.phone" disabled />
              </div>
            </div>
          </div>
          <div class="d-flex justify-content-start mt-3">
            <button type="button" class="btn btn-dark">Editar</button>
          </div>
        </div>
      </div>
  </div>
</template>

<script>
import axios from 'axios';
import { ref, onMounted } from 'vue';
//import { useRoute } from 'vue-router';
import currentUserService from "@/services/currentUserService";

export default {
  name: 'ViewEmployeeProfile',
  setup() {
    const employee = ref({});
    const loading = ref(true);
    const error = ref(null);
    const userPosition = ref('');

    const getEmployee = async () => {
      try {
        loading.value = true;
        error.value = null;

        const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage();
        userPosition.value = currentUserInformation.position;
        let response;

        if (currentUserInformation.position == 'Employer') {
          response = await axios.get(`https://localhost:5000/api/EmployerGetID/GetEmployerById/${currentUserInformation.idNaturalPerson}`);
        } else {
          response = await axios.get(`https://localhost:5000/api/EmployeeGetID/GetEmployeeById/${currentUserInformation.idNaturalPerson}`);
        }

        employee.value = {
          id: response.data.id || '',
          firstName: response.data.firstName || '',
          firstLastName: response.data.firstSurname || '',
          secondLastName: response.data.secondSurname || '',
          identityCard: response.data.cedula || '',
          workerId: response.data.workerId || '',
          role: currentUserInformation.position,
          contractType: response.data.contractType || '',
          grossSalary: response.data.grossSalary || 0,
          email: response.data.email || '',
          phone: response.data.phoneNumber || ''
        };

      } catch (err) {
        console.error("Error al obtener datos del perfil:", err);
        error.value = "Error al cargar la información del perfil. Por favor, intente de nuevo más tarde.";
      } finally {
        loading.value = false;
      }
    };

    onMounted(() => {
      getEmployee();
    });

    return {
      employee,
      loading,
      error,
      userPosition
    };
  }
};

</script>

<style scoped>
.card-title {
  margin-bottom: 1.5rem;
}
</style>
