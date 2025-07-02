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
                <label for="gender" class="form-label">Género</label>
                <input type="text" class="form-control" id="gender" :value="genderLabel" disabled />
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
import employeeService from '@/services/employeeService';
import employerService from '@/services/employerService';
import { ref, onMounted } from 'vue';
import { computed } from 'vue';
import currentUserService from "@/services/currentUserService";

export default {
  name: 'ViewEmployeeProfile',
  setup() {
    
    const employee = ref({});
    const loading = ref(true);
    const error = ref(null);
    const userPosition = ref('');

    const genderLabel = computed(() => {
      if (employee.value.gender === 'M') return 'Masculino';
      if (employee.value.gender === 'F') return 'Femenino';
      return 'Otro';
    });

    const getEmployee = async () => {
      try {
        loading.value = true;
        error.value = null;

        const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage();
        userPosition.value = currentUserInformation.position;
        let data;

        if (currentUserInformation.position == 'Employer') {
          data = await employerService.getEmployerById(currentUserInformation.idNaturalPerson);
        } else {
          data = await employeeService.getEmployeeById(currentUserInformation.idNaturalPerson);
        }

        employee.value = {
          id: data.id || '',
          firstName: data.firstName || '',
          firstLastName: data.firstSurname || '',
          secondLastName: data.secondSurname || '',
          identityCard: data.cedula || '',
          workerId: data.workerId || '',
          role: currentUserInformation.position,
          contractType: data.contractType || '',
          grossSalary: data.grossSalary || 0,
          email: data.email || '',
          phone: data.phoneNumber || '',
          gender: data.gender || ''
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
      userPosition,
      genderLabel
    };
  }
};

</script>

<style scoped>
.card-title {
  margin-bottom: 1.5rem;
}
</style>
