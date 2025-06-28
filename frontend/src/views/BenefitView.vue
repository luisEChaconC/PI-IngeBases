<template>
  <router-link
    to="/benefits"
    class="btn btn-outline-secondary ms-5 mt-3"
    title="Volver a la lista de beneficios"
    style="z-index: 10;"
  >
    ← Volver
  </router-link>
  <div class="container d-flex flex-column justify-content-center align-items-center" >
    <div class="card shadow-sm p-4 rounded-4 my-5" style="width: 400px;">
      <h3 class="fw-bold mb-4">Beneficio</h3>

      <div class="mb-3">
        <label class="form-label">Nombre</label>
        <input v-model="benefit.name" type="text" class="form-control" :disabled="!fieldEditability.type" />
      </div>

      <div class="mb-3">
        <label class="form-label">Descripción</label>
        <textarea v-model="benefit.description" class="form-control" rows="2" :disabled="!fieldEditability.description"></textarea>
      </div>

      <div class="mb-3">
        <label class="form-label">Tipo</label>

        <select
          v-if="fieldEditability.type"
          v-model="benefit.type"
          class="form-select"
        >
          <option value="API">API</option>
          <option value="FixedAmount">Monto Fijo</option>
          <option value="FixedPercentage">Porcentaje Fijo</option>
        </select>
        
        <input
          v-else
          :value="translatedType"
          type="text"
          class="form-control"
          disabled
        />
      </div>

      <div v-if="benefit.type === 'API'" class="mb-3">
        <label class="form-label">Enlace API</label>
        <input v-model="benefit.linkAPI" type="text" class="form-control" :disabled="!fieldEditability.linkAPI" />
      </div>

      <div v-if="benefit.type === 'FixedPercentage'" class="mb-3">
        <label class="form-label">Porcentaje fijo (%)</label>
        <div class="input-group">
          <input v-model="benefit.fixedPercentage" type="number" class="form-control" :disabled="!fieldEditability.fixedPercentage" />
          <span class="input-group-text">%</span>
        </div>
      </div>

      <div v-if="benefit.type === 'FixedAmount'" class="mb-3">
        <label class="form-label">Monto fijo</label>
        <div class="input-group">
          <span class="input-group-text">₡</span>
          <input v-model="benefit.fixedAmount" type="number" class="form-control" :disabled="!fieldEditability.fixedAmount" />
        </div>
      </div>

      <div class="mb-3">
        <label class="form-label">Meses requeridos trabajados</label>
        <input
          v-model="benefit.requiredMonthsWorked"
          type="number"
          class="form-control"
          :disabled="!fieldEditability.requiredMonthsWorked"
        />
      </div>

      <div class="mb-3">
        <label class="form-label">Tipos de empleados elegibles</label>

        <div v-if="isEditable">
          <div
            v-for="(type, index) in allEmployeeTypes"
            :key="index"
            class="form-check"
          >
            <input
              class="form-check-input"
              type="checkbox"
              :id="`empType-${index}`"
              :value="type.name"
              v-model="benefit.eligibleEmployeeTypes"
              :disabled="!fieldEditability.employeeTypes"
            />
            <label class="form-check-label" :for="`empType-${index}`">
              {{ type.label }}
            </label>
          </div>
        </div>

        <ul v-else class="list-group">
          <li
            class="list-group-item"
            v-for="(type, index) in translatedEmployeeTypes"
            :key="index"
          >
            {{ type }}
          </li>
        </ul>
      </div>

      <div class="d-flex justify-content-center align-items-center mt-4">
        <div class="text-center me-2">
          <button v-if="!isEditable" class="btn btn-dark" @click="handleEditClick">
            Editar
          </button>

          <div v-else>
            <button class="btn btn-success me-2" @click="guardarCambios">
              Guardar Cambios
            </button>
            <button class="btn btn-outline-secondary" @click="cancelarEdicion">
              Cancelar
            </button>
          </div>

          <div v-if="showApiEditWarning" class="alert alert-warning mt-3" role="alert">
            El beneficio tiene parámetros y ya ha sido seleccionado
          </div>
        </div>

        <button v-if="!isEditable" class="btn btn-danger" @click="deleteBenefit">
          Eliminar
        </button>
      </div>
    </div>
  </div>
</template>


  
  <script setup>
  
  import { computed, onMounted, ref } from 'vue'
  import { useRoute } from 'vue-router'
  import benefitService from '@/services/benefitService'
  import Swal from 'sweetalert2'

  const route = useRoute()
  const benefitId = route.params.id
  const benefit = ref({})
  const isEditable = ref(false)

  const getCurrentUserInformationFromLocalStorage = () => {
      const userInformation = localStorage.getItem('currentUserInformation');
      return userInformation ? JSON.parse(userInformation) : null;
  };

  const showApiEditWarning = ref(false)

  const originalBenefit = ref({})

  const assigned = ref(false)

  const allEmployeeTypes = [
    { name: 'Full-Time', label: 'Tiempo completo' },
    { name: 'Part-Time', label: 'Medio tiempo' },
    { name: 'Professional Services', label: 'Por contrato' },
  ]

  const fieldEditability = computed(() => {
    const isApi = benefit.value.type === 'API'
    const isAssigned = assigned.value

    if (!isEditable.value) {
      return {
        name: false,
        description: false,
        type: false,
        linkAPI: false,
        fixedPercentage: false,
        fixedAmount: false,
        requiredMonthsWorked: false,
        employeeTypes: false,
      }
    }

    if (isApi && !isAssigned) {
      return {
        name: false,
        description: true,
        type: false,
        linkAPI: false,
        fixedPercentage: false,
        fixedAmount: false,
        requiredMonthsWorked: true,
        employeeTypes: true,
      }
    }

    // En edición completa
    return {
      name: true,
      description: true,
      type: true,
      linkAPI: true,
      fixedPercentage: true,
      fixedAmount: true,
      requiredMonthsWorked: !(isAssigned && !isApi),
      employeeTypes: true,
    }
  })

  const handleEditClick = async () => {
    try {
      const isApiType = benefit.value.type === 'API'
      assigned.value = await benefitService.benefitIsAssigned(benefit.value.id)

      if (isApiType && assigned.value) {
        showApiEditWarning.value = true
        setTimeout(() => {
          showApiEditWarning.value = false
        }, 4000)
        return
      }

      originalBenefit.value = JSON.parse(JSON.stringify(benefit.value))
      isEditable.value = true

    } catch (error) {
      console.error('Error al verificar si el beneficio está asignado:', error)
      alert('Hubo un error al verificar el estado del beneficio.')
    }
  }

  const deleteBenefit = () => {
    Swal.fire({
      title: '¿Está seguro?',
      text: "Esta acción eliminará el beneficio",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#6c757d',
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then(async (result) => {
      if (result.isConfirmed) {
        try {
          // Usa el servicio en vez de axios directamente
          const response = await benefitService.deleteBenefit(benefit.value.id);
          const { resultCode, resultMessage } = response;

          let mensaje = '';
          switch (resultCode) {
            case 'MarkedAsDeleted':
              mensaje = 'El beneficio ha sido marcado como eliminado. Su visibilidad permanecerá activa hasta el siguiente periodo de pago para fines históricos.';
              break;
            case 'DeletedWithAssignments':
              mensaje = 'Beneficio eliminado. Las asignaciones activas han sido removidas.';
              break;
            case 'Deleted':
              mensaje = 'Beneficio eliminado correctamente.';
              break;
            case 'Error':
              mensaje = 'Ocurrió un error al eliminar el beneficio: ' + (resultMessage || '');
              break;
            default:
              mensaje = resultMessage || 'Resultado desconocido.';
          }

          Swal.fire({
            icon: resultCode === 'Error' ? 'error' : 'success',
            title: mensaje,
          }).then(() => {
            if (resultCode !== 'Error') {
              window.location.href = '/benefits';
            }
          });

        } catch (error) {
          let mensaje = 'No se pudo conectar con el servidor. Intente de nuevo más tarde.';
          if (error.response) {
            if (error.response.status === 400) {
              mensaje = error.response.data?.resultMessage || 'Solicitud inválida. El ID del beneficio es requerido.';
            } else if (error.response.status === 500) {
              mensaje = error.response.data?.resultMessage || 'Error interno del servidor al eliminar el beneficio.';
            }
          }
          Swal.fire({
            icon: 'error',
            title: mensaje
          });
        }
      }
    });
  }

  const cancelarEdicion = () => {
    benefit.value = JSON.parse(JSON.stringify(originalBenefit.value))
    isEditable.value = false
  }

  const guardarCambios = async () => {
    try {
      const companyId = getCurrentUserInformationFromLocalStorage()?.companyId

      const updatedBenefit = {
        ...benefit.value,
        companyId, 
      }

      await benefitService.updateBenefit(benefit.value.id, updatedBenefit)
    
      isEditable.value = false
      originalBenefit.value = JSON.parse(JSON.stringify(benefit.value)) 
      alert('Beneficio actualizado correctamente.')
    } catch (error) {
      if (error.response?.status === 409) {
        alert('No se puede editar el beneficio porque ya fue asignado a empleados.')
      } else if (error.response?.status === 400) {
        alert('ID no válido.')
      } else {
        console.error('Error al guardar el beneficio:', error)
        alert('Ocurrió un error al guardar los cambios.')
      }
    }
  }

  
  onMounted(async () => {
    try {
      const companyId = getCurrentUserInformationFromLocalStorage()?.companyId
      const data = await benefitService.getBenefitById(benefitId, companyId)
    data.eligibleEmployeeTypes = data.eligibleEmployeeTypes || []
    benefit.value = data
    } catch (error) {
      console.error('Error loading benefit:', error)
    }
  })

  const translatedEmployeeTypes = computed(() => {
      const employeeTypeMap = {
          'Full-Time': 'Tiempo completo',
          'Part-Time': 'Medio tiempo',
          'Professional Services': 'Por contrato',
          'Hourly': 'Por Horas'
      }

      return benefit.value.eligibleEmployeeTypes?.map(type => employeeTypeMap[type] || type) || []
  })

  const translatedType = computed(() => {
    switch (benefit.value.type) {
      case 'API': return 'API'
      case 'FixedAmount': return 'Monto Fijo'
      case 'FixedPercentage': return 'Porcentaje Fijo'
      default: return benefit.value.type
    }
  })
  </script>
  
  <style scoped>
  .card {
    border-radius: 1rem;
  }
  </style>
  