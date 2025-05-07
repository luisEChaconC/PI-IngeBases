<template>
    <div class="d-flex justify-content-center align-items-center min-vh-100 my-5">
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
                <h2 class="card-title">Perfil de Empleado</h2>
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
                        <div class="mb-3">
                            <label for="workerId" class="form-label">ID Trabajador</label>
                            <input type="text" class="form-control" id="workerId" v-model="employee.workerId" disabled />
                        </div>
                        <div class="mb-3">
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
                        <div class="mb-3">
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
                <div class="d-flex justify-content-start mt-3 gap-2">
                    <button type="button" class="btn btn-dark">Editar</button>
                    <button type="button" class="btn btn-danger" @click="openModal">Eliminar</button>
                </div>
            </div>
        </div>

        <!-- Modal -->
        <div class="modal fade" id="deleteModal" tabindex="-1" aria-labelledby="deleteModalLabel" aria-hidden="true" ref="deleteModal">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-danger text-white">
                        <h5 class="modal-title" id="deleteModalLabel">Confirmar Eliminación</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                    </div>
                    <div class="modal-body">
                        ¿Seguro que deseas eliminar este perfil de la Empresa?
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">No</button>
                        <button type="button" class="btn btn-danger" @click="confirmDelete">Sí, eliminar</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import { Modal } from 'bootstrap';

export default {
  name: 'ViewEmployeeProfile',
  setup() {
    const employee = ref({});
    const loading = ref(true);
    const error = ref(null);
    const route = useRoute();
    const deleteModal = ref(null);
    let deleteModalInstance = null;

    const getEmployee = async () => {
      try {
        loading.value = true;
        error.value = null;

        const id = route.params.id;
        const response = await axios.get(`https://localhost:5000/api/EmployeeGetID/GetEmployeeById/${id}`);

        employee.value = {
          id: response.data.id || '',
          firstName: response.data.firstName || '',
          firstLastName: response.data.firstSurname || '',
          secondLastName: response.data.secondSurname || '',
          identityCard: response.data.cedula || '',
          workerId: response.data.workerId || '',
          role: response.data.isAdmin ? 'Admin' : 'Employee',
          contractType: response.data.contractType || '',
          grossSalary: response.data.grossSalary || 0,
          email: response.data.email || '',
          phone: response.data.phoneNumber || ''
        };
      } catch (err) {
        console.error("Error to get employee data:", err);
        error.value = "Errorto load information";
      } finally {
        loading.value = false;
      }
    };

    const openModal = () => {
      if (!deleteModalInstance) {
        deleteModalInstance = new Modal(deleteModal.value);
      }
      deleteModalInstance.show();
    };

    const confirmDelete = () => {
      deleteModalInstance.hide();
      alert('Perfil eliminado');
    };

    onMounted(() => {
      getEmployee();
    });

    return {
      employee,
      loading,
      error,
      deleteModal,
      openModal,
      confirmDelete
    };
  }
};
</script>

<style scoped>
    .card-title {
        margin-bottom: 1.5rem;
    }

    .gap-2 {
        gap: 0.5rem;
    }
</style>
