<template>
    <div class="container d-flex justify-content-center align-items-center mt-5">
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
          <input v-model="benefit.type" type="text" class="form-control" disabled />
        </div>
       
        <div v-if="benefit.type === 'API'" class="mb-3">
          <label class="form-label">Enlace API</label>
          <input v-model="benefit.linkAPI" type="text" class="form-control" disabled />
        </div>

        <div v-if="benefit.type === 'Percentage'" class="mb-3">
          <label class="form-label">Porcentaje fijo (%)</label>
          <input v-model="benefit.fixedPercentage" type="number" class="form-control" disabled />
        </div>

        <div v-if="benefit.type === 'Amount'" class="mb-3">
          <label class="form-label">Monto fijo</label>
          <input v-model="benefit.fixedAmount" type="number" class="form-control" disabled />
        </div>
  
        <div class="mb-3">
          <label class="form-label">Meses requeridos trabajados</label>
          <input v-model="benefit.requiredMonthsWorked" type="number" class="form-control" disabled />
        </div>
  
        <div class="mb-3">
          <label class="form-label">Tipos de empleados elegibles</label>
          <ul class="list-group">
            <li class="list-group-item" v-for="(type, index) in benefit.eligibleEmployeeTypes" :key="index">
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
  import { onMounted, ref } from 'vue'
  import { useRoute } from 'vue-router'
  import axios from 'axios'
  
  const route = useRoute()
  const benefitId = route.params.id
  const benefit = ref({})
  
  onMounted(async () => {
    try {
      const response = await axios.get(`https://localhost:5000/api/benefit/${benefitId}`)
      benefit.value = response.data
    } catch (error) {
      console.error('Error loading benefit:', error)
    }
  })
  </script>
  
  <style scoped>
  .card {
    border-radius: 1rem;
  }
  </style>
  