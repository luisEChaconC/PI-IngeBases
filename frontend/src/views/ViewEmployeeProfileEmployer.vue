<template>
  <div class="d-flex justify-content-center align-items-center position-relative" style="min-height: calc(100vh - 200px);">
    <router-link to="/employees-list" class="btn btn-outline-secondary position-absolute top-0 start-0 m-3" title="Volver">
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
        <h2 class="card-title">Perfil de Empleado</h2>
        <div class="row">
          <div class="col-md-6">
            <div class="mb-3" v-for="field in filteredLeftFields" :key="field.key">
              <label :for="field.key" class="form-label">{{ field.label }}</label>
              <input
                :type="field.type"
                class="form-control"
                :id="field.key"
                v-model="employee[field.key]"
                :disabled="!editMode || (hasPayments && ['firstName', 'firstSurname', 'secondSurname', 'legalId'].includes(field.key))"
                :class="{ 'is-invalid': editMode && !validations[field.key] }"
                @input="onInputChange(field.key)"
              />
              <div v-if="editMode && !validations[field.key]" class="invalid-feedback">
                El formato no es correcto, por favor revisa nuevamente.
              </div>
            </div>
            <div class="mb-3">
              <label for="contractType" class="form-label">Tipo de Contrato</label>
              <select
                id="contractType"
                class="form-select"
                v-model="employee.contractType"
                :disabled="!editMode"
                :class="{ 'is-invalid': editMode && !validations.contractType }"
                @change="validateField('contractType')"
              >
                <option value="">Seleccione una opción</option>
                <option value="Part-Time">Media Jornada</option>
                <option value="Full-Time">Jornada Completa</option>
                <option value="Professional Services">Servicios Profesionales</option>
                <option value="Hourly">Por Hora</option>
              </select>
              <div v-if="editMode && !validations.contractType" class="invalid-feedback">
                Seleccione un tipo de contrato válido.
              </div>
            </div>
          </div>

          <div class="col-md-6">
            <div class="mb-3" v-for="field in rightFields" :key="field.key">
              <label :for="field.key" class="form-label">{{ field.label }}</label>

              <input
                v-if="field.key !== 'role'"
                :type="field.type"
                class="form-control"
                :id="field.key"
                v-model="employee[field.key]"
                :disabled="!editMode || (hasPayments && ['firstName', 'firstSurname', 'secondSurname', 'legalId'].includes(field.key))"
                :class="{ 'is-invalid': editMode && !validations[field.key] }"
                @input="onInputChange(field.key)"
              />
              <div v-if="editMode && !validations[field.key] && field.key !== 'role'" class="invalid-feedback">
                El formato no es correcto, por favor revisa nuevamente.
              </div>

              <div v-else-if="field.key === 'role'">
                <select
                  id="role"
                  class="form-select"
                  v-model="employee.role"
                  :disabled="!editMode"
                  :class="{ 'is-invalid': editMode && !employee.role }"
                  required
                  @change="validateField('role')"
                >
                  <option value="">Seleccione una opción</option>
                  <option value="Supervisor">Supervisor</option>
                  <option value="PayrollManager">Encargado Planilla</option>
                  <option value="Collaborator">Colaborador</option>
                </select>
                <div v-if="editMode && !employee.role" class="invalid-feedback">
                  Seleccione un rol válido.
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="mb-3">
          <label for="gender" class="form-label">Género</label>
          <select
            id="gender"
            class="form-select"
            v-model="employee.gender"
            :disabled="!editMode"
            :class="{ 'is-invalid': editMode && !validations.gender }"
            @change="validateField('gender')"
          >
            <option value="">Seleccione</option>
            <option value="M">M</option>
            <option value="F">F</option>
          </select>
          <div v-if="editMode && !validations.gender" class="invalid-feedback">
            El género debe ser 'M' o 'F'.
          </div>
        </div>

        <div class="d-flex justify-content-start mt-3 gap-2">
          <button type="button" class="btn btn-dark" @click="toggleEdit">
            {{ editMode ? 'Cancelar' : 'Editar' }}
          </button>
          <button v-if="editMode" type="button" class="btn btn-primary" @click="submitEdit" :disabled="!formValid">
            Guardar cambios
          </button>
         <button type="button" class="btn btn-danger" @click="deleteEmployee">Eliminar</button>
        </div>
      </div>
    </div>

    <div class="modal fade" id="deleteModal" tabindex="-1" ref="deleteModal">
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header bg-danger text-white">
            <h5 class="modal-title">Confirmar Eliminación</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
          </div>
          <div class="modal-body">¿Seguro que deseas eliminar este perfil?</div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
            <button type="button" class="btn btn-danger" @click="confirmDelete">Sí, eliminar</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>


<script>
import { ref, onMounted, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import Swal from 'sweetalert2';
import { Modal } from 'bootstrap';
import employeeService from '@/services/employeeService';

export default {
  name: 'ViewEmployeeProfile',
  setup() {
    const employee = ref({});
    const validations = ref({});
    const loading = ref(true);
    const error = ref(null);
    const editMode = ref(false);
    const route = useRoute();
    const deleteModal = ref(null);
    let deleteInstance = null;
    const hasPayments = ref(false);


    const regex = {
      email: /^[\w.-]+@([\w-]+\.)+[\w-]{2,4}$/,
      phoneNumber: /^$|^\d{4}-?\d{4}$/,
      grossSalary: /^\d+(\.\d{1,2})?$/,
      legalId: /^((?!0)\d{9,12}|(?!0)\d{1,2}-\d{4}-\d{4}|(?!0)\d-\d{3}-\d{6})$/,
      workerId: /^[A-Z0-9#-]{3,20}$/,
      contractType: /^.+$/,
      firstName: /^[A-Za-zÁÉÍÓÚñáéíóú\s]{2,40}$/,
      firstSurname: /^[A-Za-zÁÉÍÓÚñáéíóú\s]{2,40}$/,
      secondSurname: /^[A-Za-zÁÉÍÓÚñáéíóú\s]{2,40}$/
    };

    const fieldErrors = ref({});

   const leftFields = ref([
  { key: 'firstName', label: 'Nombre', type: 'text' },
  { key: 'secondSurname', label: 'Segundo Apellido', type: 'text' },
  { key: 'workerId', label: 'ID Trabajador', type: 'text' },
  { key: 'contractType', label: 'Tipo de Contrato', type: 'text' },
  { key: 'email', label: 'Correo Electrónico', type: 'email' }
]);

const filteredLeftFields = computed(() =>
  leftFields.value.filter(field => field.key !== 'contractType')
);


    const rightFields = [
      { key: 'firstSurname', label: 'Primer Apellido', type: 'text' },
      { key: 'legalId', label: 'Cédula', type: 'text' },
      { key: 'role', label: 'Rol', type: 'text' },
      { key: 'grossSalary', label: 'Salario Bruto', type: 'number' },
      { key: 'phoneNumber', label: 'Teléfono', type: 'tel' }
    ];

    const capitalizeWords = (text) => {
      return text
        .toLowerCase()
        .replace(/\b\w/g, char => char.toUpperCase());
    };

    const onInputChange = (key) => {
      if (["firstName", "firstSurname", "secondSurname"].includes(key)) {
        employee.value[key] = capitalizeWords(employee.value[key]);
      }
      validateField(key);
    };

    const validateField = (key) => {
      const value = employee.value[key];
      if (regex[key]) {
        const valid = regex[key].test(value ?? '');
        validations.value[key] = valid;
        fieldErrors.value[key] = valid ? '' : `Invalid field: ${key}`;
      } else {
        const required = key !== 'phoneNumber';
        const valid = !required || (value !== null && value !== '');
        validations.value[key] = valid;
        fieldErrors.value[key] = valid ? '' : `This field is required.`;
      }
    };

    const formValid = computed(() =>
      Object.values(validations.value).every(Boolean)
    );

    const getEmployee = async () => {
      try {
        const id = route.params.id;
        const data = await employeeService.getEmployeeById(id);
        employee.value = {
          id: data.id,
          firstName: data.firstName,
          firstSurname: data.firstSurname,
          secondSurname: data.secondSurname,
          gender: data.gender,
          legalId: data.cedula,
          workerId: data.workerId,
          role: data.role,
          contractType: data.contractType,
          grossSalary: data.grossSalary,
          email: data.email,
          phoneNumber: data.phoneNumber
        };
        Object.keys(employee.value).forEach(validateField);
        hasPayments.value = await employeeService.checkEmployeeHasPayments(employee.value.id);
      } catch (err) {
        error.value = "Error loading employee data.";
      } finally {
        loading.value = false;
      }
    };

    const toggleEdit = () => {
      if (editMode.value) getEmployee();
      editMode.value = !editMode.value;
    };

    const submitEdit = async () => {
      Object.keys(employee.value).forEach(validateField);
      if (!formValid.value) {
        alert("There are errors in the form. Check highlighted fields.");
        return;
      }
      try {
        const payload = {
          Id: employee.value.id,
          FirstName: employee.value.firstName,
          FirstSurname: employee.value.firstSurname,
          SecondSurname: employee.value.secondSurname,
          Gender: employee.value.gender,
          LegalId: employee.value.legalId,
          WorkerId: employee.value.workerId,
          Role: employee.value.role,
          ContractType: employee.value.contractType,
          GrossSalary: employee.value.grossSalary,
          Email: employee.value.email,
          PhoneNumber: employee.value.phoneNumber
        };
        await employeeService.updateEmployeeAsEmployer(payload);
        editMode.value = false;
        await getEmployee();
      } catch (err) {
        const errorCode = err.response?.data?.error || "";
        let msg;
        switch (errorCode) {
          case "CEDULA_DUPLICADA":
            msg = "Duplicate legal ID.";
            validations.value.legalId = false;
            break;
          case "WORKERID_DUPLICADO":
            msg = "Worker ID already in use.";
            validations.value.workerId = false;
            break;
          default:
            msg = "Failed to save changes. Check your input.";
        }
        alert(msg);
        console.error(err);
      }
    };

    const router = useRouter();

const deleteEmployee = () => {
  const warningText = `
    ${hasPayments.value
      ? '<div style="color:red; font-weight:bold;">Este empleado tiene registros de planilla asignados.</div>'
      : ''}
    <div style="color:red; margin-top:6px;">Esta acción eliminará al empleado.</div>
    <div style="margin-top:12px; color:#495057; font-size:0.9rem;">
      <i>Información histórica del empleado permanecerá registrada.</i>
    </div>
  `;

  Swal.fire({
    title: '¿Estás seguro?',
    html: warningText,
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#d33',
    cancelButtonColor: '#6c757d',
    confirmButtonText: 'Sí, eliminar',
    cancelButtonText: 'Cancelar'
  }).then(async (result) => {
    if (result.isConfirmed) {
      try {
        await employeeService.deleteEmployee(employee.value.id);
        Swal.fire(
          'Eliminado',
          'El empleado ha sido eliminado exitosamente.',
          'success'
        );
        router.push('/employees-list');
      } catch (error) {
        Swal.fire(
          'Error',
          'Ocurrió un error al eliminar el empleado.',
          'error'
        );
        console.error("Error al eliminar empleado:", error.response?.data || error);
      }
    }
  });
};

    const openModal = () => {
      if (!deleteInstance) deleteInstance = new Modal(deleteModal.value);
      deleteInstance.show();
    };

    const confirmDelete = () => {
      deleteInstance.hide();
      alert("Employee deleted");
    };

    onMounted(() => {
      getEmployee();
    });

    return {
      employee,
      validations,
      fieldErrors,
      loading,
      error,
      editMode,
      toggleEdit,
      submitEdit,
      formValid,
      deleteModal,
      openModal,
      confirmDelete,
      leftFields,
      rightFields,
      validateField,
      hasPayments,
      deleteEmployee,
      onInputChange,
      filteredLeftFields
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
.is-invalid {
  border-color: red;
}
</style>
