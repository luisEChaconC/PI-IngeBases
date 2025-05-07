<template>
  <h3 class="fw-bold mb-4 text-center">Beneficio</h3>
    <div class="container d-flex justify-content-center align-items-center mt-5">
        <router-link
          to="/benefits"
          class="btn btn-outline-secondary position-absolute top-0 start-0 m-3"
          title="Volver a la lista de beneficios"
        >
          ← Volver
        </router-link>
      <div class="card shadow-sm p-4 rounded-4" style="width: 400px;">
        <h3 class="fw-bold mb-4">Beneficio</h3>
  
        <div class="mb-3">
          <label class="form-label">Nombre</label>
          <input v-model="benefit.name" type="text" class="form-control" disabled />
        </div>
  
        <div class="mb-3">
          <label class="form-label">Descripción</label>
          <textarea v-model="benefit.description" class="form-control" rows="2" disabled></textarea>
        </div>
  
        <div class="mb-3">
          <label class="form-label">Tipo</label>
          <input :value="translatedType" type="text" class="form-control" disabled />
        </div>
       
        <div v-if="benefit.type === 'API'" class="mb-3">
          <label class="form-label">Enlace API</label>
          <input v-model="benefit.linkAPI" type="text" class="form-control" disabled />
        </div>

        <div v-if="benefit.type === 'FixedPercentage'" class="mb-3">
          <label class="form-label">Porcentaje fijo (%)</label>
          <div class="input-group">
            <input :value="benefit.fixedPercentage" type="number" class="form-control" disabled />
            <span class="input-group-text">%</span>
          </div>
        </div>

        <div v-if="benefit.type === 'FixedAmount'" class="mb-3">
          <label class="form-label">Monto fijo</label>
          <div class="input-group">
            <span class="input-group-text">₡</span>
            <input :value="benefit.fixedAmount" type="number" class="form-control" disabled />
          </div>
        </div>
  
        <div class="mb-3">
          <label class="form-label">Meses requeridos trabajados</label>
          <input v-model="benefit.requiredMonthsWorked" type="number" class="form-control" disabled />
        </div>
  
        <div class="mb-3">
            <label class="form-label">Tipos de empleados elegibles</label>
            <ul class="list-group">
                <li class="list-group-item"
                    v-for="(type, index) in translatedEmployeeTypes"
                    :key="index">
                    {{ type }}
                </li>
            </ul>
        </div>
  
        <div class="text-center mt-4">
          <button class="btn btn-dark">Editar beneficio</button>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  
  import { computed, onMounted, ref } from 'vue'
  import { useRoute } from 'vue-router'
  import axios from 'axios'
  
  const route = useRoute()
  const benefitId = route.params.id
  const benefit = ref({})

  const getCurrentUserInformationFromLocalStorage = () => {
      const userInformation = localStorage.getItem('currentUserInformation');
      return userInformation ? JSON.parse(userInformation) : null;
  };
  
  onMounted(async () => {
  try {
    const companyId = getCurrentUserInformationFromLocalStorage()?.companyId
    const response = await axios.get(`https://localhost:5000/api/benefit/${benefitId}`, {
      params: { companyId }
    })
    benefit.value = response.data
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
  