<template>
  <div class="container d-flex justify-content-center align-items-center mt-5">
    <router-link
      to="/benefits"
      class="btn btn-outline-secondary position-absolute top-0 start-0 m-3"
      title="Volver a la lista de beneficios"
    >
      ← Volver
    </router-link>

    <div class="card shadow-sm p-4 rounded-4" style="width: 500px;">
      <h3 class="fw-bold mb-4">Nuevo Beneficio</h3>

      <div class="mb-3">
        <label class="form-label">Nombre</label>
        <input
          v-model="benefit.name"
          type="text"
          class="form-control"
          maxlength="35"
          @input="validateBenefit"
        />
        <div v-if="errors.name" class="text-danger small mt-1">{{ errors.name }}</div>
      </div>

      <div class="mb-3">
        <label class="form-label">Descripción</label>
        <textarea
          v-model="benefit.description"
          class="form-control"
          rows="2"
          maxlength="100"
          @input="validateBenefit"
        ></textarea>
        <div v-if="errors.description" class="text-danger small mt-1">{{ errors.description }}</div>
      </div>

      <div class="mb-3">
        <label class="form-label">Tipo</label>
        <select
          v-model="benefit.type"
          class="form-select"
          @change="validateBenefit"
        >
          <option value="API">API</option>
          <option value="FixedAmount">Monto Fijo</option>
          <option value="FixedPercentage">Porcentaje Fijo</option>
        </select>
      </div>

      <div v-if="benefit.type === 'API'" class="mb-3">
        <label class="form-label">Enlace API</label>
        <input
          v-model="benefit.linkAPI"
          type="text"
          class="form-control"
          maxlength="100"
          @input="validateBenefit"
        />
        <div v-if="errors.linkAPI" class="text-danger small mt-1">{{ errors.linkAPI }}</div>
      </div>

      <div v-if="benefit.type === 'FixedPercentage'" class="mb-3">
        <label class="form-label">Porcentaje fijo (%)</label>
        <div class="input-group">
          <input
            v-model.number="benefit.fixedPercentage"
            type="number"
            class="form-control"
            min="0"
            max="100"
            @input="validateBenefit"
          />
          <span class="input-group-text">%</span>
        </div>
        <div v-if="errors.fixedPercentage" class="text-danger small mt-1">{{ errors.fixedPercentage }}</div>
      </div>

      <div v-if="benefit.type === 'FixedAmount'" class="mb-3">
        <label class="form-label">Monto fijo</label>
        <div class="input-group">
          <span class="input-group-text">₡</span>
          <input
            v-model.number="benefit.fixedAmount"
            type="number"
            class="form-control"
            min="0"
            max="10000000"
            @input="validateBenefit"
          />
        </div>
        <div v-if="errors.fixedAmount" class="text-danger small mt-1">{{ errors.fixedAmount }}</div>
      </div>

      <div class="mb-3">
        <label class="form-label">Meses requeridos trabajados</label>
        <input
          v-model.number="benefit.requiredMonthsWorked"
          type="number"
          class="form-control"
          min="0"
          max="240"
          @input="validateBenefit"
        />
        <div v-if="errors.requiredMonthsWorked" class="text-danger small mt-1">{{ errors.requiredMonthsWorked }}</div>
      </div>

      <div class="mb-3">
        <label class="form-label">Tipos de empleados elegibles</label>
        <div
          class="form-check"
          v-for="type in employeeTypeOptions"
          :key="type.value"
        >
          <input
            class="form-check-input"
            type="checkbox"
            :value="type.value"
            v-model="benefit.eligibleEmployeeTypes"
          />
          <label class="form-check-label">{{ type.label }}</label>
        </div>
      </div>

      <div class="text-center mt-4">
        <button
          class="btn btn-success"
          @click="saveBenefit"
          :disabled="Object.keys(errors).length > 0"
        >
          Guardar beneficio
        </button>
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

const errors = ref({})

const employeeTypeOptions = [
  { value: 'Full-Time', label: 'Tiempo completo' },
  { value: 'Part-Time', label: 'Medio tiempo' },
  { value: 'Professional Services', label: 'Por contrato' },
  { value: 'Hourly', label: 'Por Horas' }
]

const validateBenefit = () => {
  errors.value = {}
  const b = benefit.value

  if (!b.name) {
    errors.value.name = 'Nombre es requerido.'
  } else if (b.name.length > 35) {
    errors.value.name = 'Máximo 35 caracteres.'
  }

  if (!b.description) {
    errors.value.description = 'Descripción es requerida.'
  } else if (b.description.length > 100) {
    errors.value.description = 'Máximo 100 caracteres.'
  }

  if (b.type === 'API') {
    if (!b.linkAPI) {
      errors.value.linkAPI = 'El enlace API es requerido.'
    } else if (b.linkAPI.length > 100) {
      errors.value.linkAPI = 'Máximo 100 caracteres.'
    }
  }

  if (b.type === 'FixedPercentage') {
    if (b.fixedPercentage == null) {
      errors.value.fixedPercentage = 'Porcentaje es requerido.'
    } else if (b.fixedPercentage < 0 || b.fixedPercentage > 100) {
      errors.value.fixedPercentage = 'Debe estar entre 0 y 100.'
    }
  }

  if (b.type === 'FixedAmount') {
    if (b.fixedAmount == null) {
      errors.value.fixedAmount = 'Monto es requerido.'
    } else if (b.fixedAmount < 0 || b.fixedAmount > 10000000) {
      errors.value.fixedAmount = 'Debe estar entre 0 y 10,000,000.'
    }
  }

  if (b.requiredMonthsWorked == null || b.requiredMonthsWorked < 0 || b.requiredMonthsWorked > 240) {
    errors.value.requiredMonthsWorked = 'Meses entre 0 y 240.'
  }
}

const saveBenefit = async () => {
  validateBenefit()
  if (Object.keys(errors.value).length > 0) {
    return
  }
  
  const userInfo = localStorage.getItem('currentUserInformation')
  console.log(localStorage.getItem('currentUserInformation'))
  const companyId = userInfo ? JSON.parse(userInfo).companyId : null

  if (!companyId) {
    alert('No se encontró la empresa del usuario actual.')
    return
  }

  benefit.value.companyId = companyId

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
