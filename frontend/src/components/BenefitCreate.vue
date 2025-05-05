<template>
    <div class="container d-flex justify-content-center align-items-center mt-5">
        <router-link
            to="/benefits"
            class="btn btn-outline-secondary position-absolute top-0 start-0 m-3"
            title="Volver a la lista de beneficios"
        >  ← Volver
        </router-link>
      <div class="card shadow-sm p-4 rounded-4" style="width: 500px;">

        <h3 class="fw-bold mb-4">Nuevo Beneficio</h3>
  
        <div class="mb-3">
          <label class="form-label">Nombre</label>
          <input v-model="benefit.name" type="text" class="form-control" />
        </div>
  
        <div class="mb-3">
          <label class="form-label">Descripción</label>
          <textarea v-model="benefit.description" class="form-control" rows="2"></textarea>
        </div>
  
        <div class="mb-3">
          <label class="form-label">Tipo</label>
          <select v-model="benefit.type" class="form-select">
            <option value="API">API</option>
            <option value="FixedAmount">Monto Fijo</option>
            <option value="FixedPercentage">Porcentaje Fijo</option>
          </select>
        </div>
  
        <div v-if="benefit.type === 'API'" class="mb-3">
          <label class="form-label">Enlace API</label>
          <input v-model="benefit.linkAPI" type="text" class="form-control" />
        </div>
  
        <div v-if="benefit.type === 'FixedPercentage'" class="mb-3">
          <label class="form-label">Porcentaje fijo (%)</label>
          <div class="input-group">
            <input v-model="benefit.fixedPercentage" type="number" class="form-control" />
            <span class="input-group-text">%</span>
          </div>
        </div>
  
        <div v-if="benefit.type === 'FixedAmount'" class="mb-3">
          <label class="form-label">Monto fijo</label>
          <div class="input-group">
            <span class="input-group-text">₡</span>
            <input v-model="benefit.fixedAmount" type="number" class="form-control" />
          </div>
        </div>
  
        <div class="mb-3">
          <label class="form-label">Meses requeridos trabajados</label>
          <input v-model="benefit.requiredMonthsWorked" type="number" class="form-control" />
        </div>
  
        <div class="mb-3">
            <label class="form-label">Tipos de empleados elegibles</label>
            <div class="form-check" v-for="type in employeeTypeOptions" :key="type.value">
                <input
                class="form-check-input"
                type="checkbox"
                :value="type.value"
                v-model="benefit.eligibleEmployeeTypes"
                :id="`checkbox-${type.value}`"
                />
                <label class="form-check-label" :for="`checkbox-${type.value}`">
                {{ type.label }}
                </label>
            </div>
        </div>
  
        <div class="text-center mt-4">
          <button class="btn btn-success" @click="saveBenefit">Guardar beneficio</button>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue'
  import axios from 'axios'
  import { useRouter } from 'vue-router'
  
  const router = useRouter()
  
  const benefit = ref({
    name: '',
    description: '',
    type: 'API',
    linkAPI: '',
    fixedPercentage: null,
    fixedAmount: null,
    requiredMonthsWorked: 0,
    isActive: true,
    eligibleEmployeeTypes: []
  })
  
  const employeeTypesInput = ref('')


  const employeeTypeOptions = [
    { value: 'FullTime', label: 'Tiempo completo' },
    { value: 'PartTime', label: 'Medio tiempo' },
    { value: 'ByContract', label: 'Por contrato' },
    ]
    
  const saveBenefit = async () => {

    benefit.value.eligibleEmployeeTypes = employeeTypesInput.value
      .split(',')
      .map(t => t.trim())
      .filter(t => t !== '')
  
    try {
      await axios.post('https://localhost:5000/api/benefit', benefit.value)
      alert('Beneficio guardado exitosamente')
      router.push('/benefits')
    } catch (err) {
      console.error(err)
      alert('Error al guardar el beneficio')
    }
  }
  </script>
  