<template>
  <div class="container my-5">
    <div class="position-relative mb-4">
      <router-link
          to="/home-view"
          class="btn btn-outline-secondary"
          title="Volver al menú principal"
        >
          ← Volver
        </router-link>
    </div>
    <div class="card p-4 shadow">
      <div class="d-flex justify-content-between align-items-center mb-4">
        <h2 class="card-title mb-0">Beneficios</h2>
        <button class="btn btn-dark" @click="openModal">
          + Seleccionar beneficios
        </button>
      </div>

      <div class="mb-4">
        <label for="maxBenefits" class="form-label">Cantidad máxima de beneficios</label>
        <input type="number" class="form-control w-25" id="maxBenefits" v-model.number="maxBenefits" disabled />
      </div>

      <div
        v-for="(benefit, index) in selectedBenefits"
        :key="index"
        class="d-flex justify-content-between align-items-center border p-2 mb-2 rounded"
        :style="benefit.isDeleted ? 'background-color: #f0f0f0; color: #888;' : ''"
      >
        <span>
          {{ benefit.name }} 
          <span v-if="benefit.isDeleted" class="ms-2 fst-italic">
            - Se eliminará el siguiente periodo de pago
          </span>
        </span>
      </div>
      <div class="d-flex justify-content-end mt-3">
        <button class="btn btn-success" @click="assignBenefits" :disabled="selectedBenefits.length === 0">
          Guardar
        </button>
      </div>
    </div>

    <!-- Modal -->
    <div class="modal fade" id="benefitsModal" tabindex="-1" ref="benefitsModal" aria-labelledby="benefitsModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Seleccionar Beneficios</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
          </div>
          <div class="modal-body">
            <div v-if="availableBenefits.length === 0" class="text-muted">No hay beneficios disponibles.</div>
            <div v-else>
              <div v-for="(benefit, index) in availableBenefits" :key="index" class="form-check">
                <input
                  class="form-check-input"
                  type="checkbox"
                  :value="benefit"
                  v-model="tempSelection"
                  :id="'benefit-' + index"
                  @change="onBenefitToggle(benefit)"
                />
                <label class="form-check-label" :for="'benefit-' + index">
                  {{ benefit.name }}
                </label>
                  <div
                    v-if="benefit && benefit.type === 'API' && tempSelection.includes(benefit) && apiParametersByBenefitId[benefit.id]"
                    class="ms-4 mt-2"
                  >
                  <div
                    v-for="(paramObj, paramName) in apiParametersByBenefitId[benefit.id]"
                    :key="benefit.id + '-' + paramName"
                    class="mb-2"
                  >
                    <label class="form-label" :for="paramName">
                      {{ traducirParametro(paramName) }}
                    </label>
                      <input
                        class="form-control"
                        :type="obtenerTipoInput(paramObj.type)"
                        v-model="apiParametersByBenefitId[benefit.id][paramName].value"
                        :id="paramName"
                        :required="true"
                        :min="paramObj.type.toLowerCase() === 'int' ? 0 : null"
                        :step="paramObj.type.toLowerCase() === 'int' ? 1 : null"
                      />
                  </div>
                  </div>
              </div>

              <div v-if="exceedsLimit" class="alert alert-warning mt-3" role="alert">
                No puedes seleccionar más de {{ maxBenefits }} beneficios.
              </div>
            </div>
          </div>
          <div class="modal-footer">
            <button class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
            <button class="btn btn-primary" @click="addSelectedBenefits" :disabled="exceedsLimit || tempSelection.length === 0">
              Agregar Seleccionados
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, computed, onMounted } from 'vue';
import { Modal } from 'bootstrap';
import benefitService from '@/services/benefitService';
import employeeService from '@/services/employeeService';
import companyService from '@/services/companyService';
import currentUserService from '@/services/currentUserService';

export default {
  name: 'EmployeeBenefits',
  setup() {
    const selectedBenefits = ref([]);
    const allBenefits = ref([]);
    const tempSelection = ref([]);
    const maxBenefits = ref(3);
    const modalInstance = ref(null);
    const benefitsModal = ref(null);
    const allApis = ref([]);
    const apiParametersByBenefitId = ref({});
    const availableBenefits = computed(() =>
      allBenefits.value.filter(
        (b) => !selectedBenefits.value.some((sb) => sb.id === b.id)
      )
    );

    const getAllApis = async () => {
      try {
        allApis.value = await benefitService.getAPIs();
      } catch (error) {
        console.error("Error obteniendo APIs:", error);
      }
    };

    const tipoParametroPorNombre = {
      "Association Name": "string",
      "Date of birth": "date",
      "Dependents": "int"
    };

    const traducirParametro = (param) => {
      const traducciones = {
        "Association Name": 'Nombre de la asociación',
        "Date of birth": 'Fecha de Nacimiento',
        "Dependents": 'Número de dependientes',
      };
      return traducciones[param] || param;
    };

    const onBenefitToggle = async (benefit) => {
      if (benefit.type !== 'API') return;

      if (apiParametersByBenefitId.value[benefit.id]) return;

      const matchedApi = allApis.value.find(api => api.name === benefit.name);

      if (!matchedApi) {
        console.warn(`No se encontró una API con nombre ${benefit.name}`);
        return;
      }

      try {
        const userDefinedParams = await benefitService.getAPIParameters(matchedApi.id);
        const filteredParams = userDefinedParams.filter(p => p.type === 'UserDefined');
        
        apiParametersByBenefitId.value[benefit.id] = {};
        filteredParams.forEach(param => {
          apiParametersByBenefitId.value[benefit.id][param.name] = {
            id: param.id,
            value: '',
            type: tipoParametroPorNombre[param.name] || 'string' 
          };
        });

        benefit.apiId = matchedApi.id;

        apiParametersByBenefitId.value = { ...apiParametersByBenefitId.value };
      } catch (error) {
        console.error(`Error cargando parámetros para API ID ${matchedApi.id}:`, error);
      }
    };

    const getBenefits = async () => {
      try {
        const userInfo = currentUserService.getCurrentUserInformationFromLocalStorage();
        const companyId = userInfo?.companyId;
        const employeeId = userInfo?.idNaturalPerson;

        if (!companyId) {
          console.error('No se encontró companyId en localStorage.');
          return;
        }

        if (!employeeId) {
          console.error('No se encontró employeeId en localStorage.');
          return;
        }

        const employee = await employeeService.getEmployeeById(employeeId);
        const contractType = employee?.contractType;

        if (!contractType) {
          console.error('No se encontró contractType en localStorage.');
          return;
        }

        const benefits = await benefitService.getBenefitsByCompanyId(companyId);
        allBenefits.value = benefits.filter(benefit =>
          benefit.eligibleEmployeeTypes &&
          benefit.eligibleEmployeeTypes.some(type =>
            type.trim().toLowerCase() === contractType.trim().toLowerCase()
          )
        );

        console.log('Beneficios filtrados:', allBenefits.value);
      } catch (error) {
        console.error('Error getting benefits:', error);
      }
    };

    const exceedsLimit = computed(() =>
      selectedBenefits.value.length + tempSelection.value.length > maxBenefits.value
    );

    const openModal = () => {
      tempSelection.value = [];
      if (!modalInstance.value) {
        modalInstance.value = new Modal(benefitsModal.value);
      }
      modalInstance.value.show();
    };

    const addSelectedBenefits = () => {
      if (!exceedsLimit.value) {
        tempSelection.value.forEach(b => {
          const copy = { ...b };

          if (copy.type === 'API' && apiParametersByBenefitId.value[copy.id]) {
            const parameters = apiParametersByBenefitId.value[copy.id];
            copy.parameterValues = {};
            Object.entries(parameters).forEach(([paramName, paramObj]) => {
              copy.parameterValues[paramName] = paramObj.value;
            });
          }

          selectedBenefits.value.push(copy);
        });

        modalInstance.value.hide();
      }
    };

    const getCompanyMaxBenefits = async () => {
      try {
        const companyId = currentUserService.getCurrentUserInformationFromLocalStorage()?.companyId;
        if (!companyId) {
          console.error("No se encontró companyId en localStorage.");
          return;
        }

        const company = await companyService.getCompanyById(companyId);
        if (company?.maxBenefitsPerEmployee !== undefined) {
          maxBenefits.value = company.maxBenefitsPerEmployee;
        }
      } catch (error) {
        console.error("Error obteniendo maxBenefitsPerEmployee:", error);
      }
    };

    const assignBenefits = async () => {
      try {
        const employeeId = currentUserService.getCurrentUserInformationFromLocalStorage()?.idNaturalPerson;
        const benefitIds = selectedBenefits.value.map(b => b.id);

        if (!employeeId || benefitIds.length === 0) {
          console.warn('Faltan datos para asignar beneficios.');
          return;
        }

        await benefitService.assignBenefitsToEmployee(employeeId, benefitIds);
        await saveParameterValues();
        alert('Beneficios asignados exitosamente.');
      } catch (error) {
        console.error('Error asignando beneficios:', error);
      }
    };

    const getAssignedBenefits = async () => {
      try {
        const employeeId = currentUserService.getCurrentUserInformationFromLocalStorage()?.idNaturalPerson;
        if (!employeeId) {
          console.error('No se encontró el ID del empleado en localStorage.');
          return;
        }

        selectedBenefits.value = await benefitService.getAssignedBenefits(employeeId);
      } catch (error) {
        console.error('Error recuperando los beneficios asignados:', error);
      }
    };

    const saveParameterValues = async () => {
      for (const benefit of selectedBenefits.value) {
        if (benefit.type !== 'API') {
          continue;
        }

        const employeeId = currentUserService.getCurrentUserInformationFromLocalStorage()?.idNaturalPerson;
        if (!employeeId) {
          return;
        }

        if (!benefit.apiId) {
          continue;
        }

        let parameters = [];
        try {
          parameters = await benefitService.getAPIParameters(benefit.apiId);
        } catch (error) {
          continue;
        }

        for (const [paramName, inputValue] of Object.entries(benefit.parameterValues || {})) {
          console.log('Procesando parámetro:', paramName, 'con valor:', inputValue);

          const param = parameters.find(p => p.name === paramName);

          if (!param) {
            continue;
          }

          if (param.type === 'systemDefined') {
            continue;
          }

          let type;
          switch (param.name) {
            case 'Date of birth':
              type = 'date';
              break;
            case 'Dependents':
              type = 'int';
              break;
            case 'Association name':
              type = 'string';
              break;
            default:
              continue;
          }

          const payload = {
            id: crypto.randomUUID(),
            parameterId: param.id,
            employeeId,
            valueType: type,
            stringValue: type === 'string' ? inputValue : null,
            intValue: type === 'int' ? parseInt(inputValue) : null,
            dateValue: type === 'date' ? new Date(inputValue).toISOString() : null
          };

          console.log('Payload preparado:', payload);

          try {
            await benefitService.saveParameterValues(payload);
            console.log("✔ Parámetro guardado correctamente");
          } catch (error) {
            console.error('❌ Error guardando valor de parámetro:', error, payload);
          }
        }
      }
    };


    const obtenerTipoInput = (type) => {
      if (!type || typeof type !== 'string') return 'text';
      switch (type.toLowerCase()) {
        case 'int':
        case 'number':
          return 'number';
        case 'date':
          return 'date';
        case 'email':
          return 'email';
        case 'boolean':
          return 'checkbox';
        default:
          return 'text';
      }
    };

    onMounted(() => {
      getBenefits();
      getAssignedBenefits();
      getCompanyMaxBenefits();
      getAllApis();
    });

    return {
      selectedBenefits,
      allBenefits,
      tempSelection,
      maxBenefits,
      modalInstance,
      benefitsModal,
      allApis,
      apiParametersByBenefitId, 
      availableBenefits,
      traducirParametro,
      onBenefitToggle,
      exceedsLimit,
      openModal,
      addSelectedBenefits,
      assignBenefits,
      getCompanyMaxBenefits,
      getBenefits,
      getAssignedBenefits,
      saveParameterValues,
      obtenerTipoInput,
    };
  },
};
</script>

<style scoped>
.card-title {
  font-weight: bold;
}
</style>
