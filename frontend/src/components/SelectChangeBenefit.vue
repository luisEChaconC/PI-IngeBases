<template>
    <div class="container my-5">
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
  
        <div v-for="(benefit, index) in selectedBenefits" :key="index" class="d-flex justify-content-between align-items-center border p-2 mb-2 rounded">
          <span>{{ benefit.name }}</span>
          <button class="btn btn-dark btn-sm" @click="removeBenefit(index)">Eliminar</button>
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
                  />
                  <label class="form-check-label" :for="'benefit-' + index">
                    {{ benefit.name }}
                  </label>
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
  import axios from 'axios';
  import { Modal } from 'bootstrap';
  
  export default {
    name: 'EmployeeBenefits',
    setup() {
      const selectedBenefits = ref([]);
      const allBenefits = ref([]);
      const tempSelection = ref([]);
      const maxBenefits = ref(2);
      const modalInstance = ref(null);
      const benefitsModal = ref(null);
  
      const getBenefits = async () => {
        try {
          const response = await axios.get("https://localhost:5000/api/benefit");
          allBenefits.value = response.data;
        } catch (error) {
          console.error('Error getting benefits:', error);
        }
      };
  
      const availableBenefits = computed(() =>
        allBenefits.value.filter(
          (b) => !selectedBenefits.value.some((sb) => sb.id === b.id)
        )
      );
  
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
          selectedBenefits.value.push(...tempSelection.value);
          modalInstance.value.hide();
        }
      };
  
      const removeBenefit = (index) => {
        selectedBenefits.value.splice(index, 1);
      };
  
      onMounted(() => {
        getBenefits();
      });
  
      return {
        selectedBenefits,
        allBenefits,
        tempSelection,
        maxBenefits,
        modalInstance,
        benefitsModal,
        availableBenefits,
        exceedsLimit,
        getBenefits,
        openModal,
        addSelectedBenefits,
        removeBenefit,
      };
    },
  };
  </script>
  
  <style scoped>
  .card-title {
    font-weight: bold;
  }
  </style>