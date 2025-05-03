<template>
    <div class="container my-5">
      <div class="card p-4 shadow">
        <div class="d-flex justify-content-between align-items-center mb-4">
          <h2 class="card-title mb-0">Beneficios</h2>
          <button class="btn btn-dark" @click="openModal">
            + Seleccionar beneficios
          </button>
        </div>
  
        <!-- maximum space -->
        <div class="mb-4">
          <label for="maxBenefits" class="form-label">Cantidad máxima de beneficios</label>
          <input type="number" class="form-control w-25" id="maxBenefits" v-model.number="maxBenefits" disabled />
        </div>
  
        <div v-for="(benefit, index) in selectedBenefits" :key="index" class="d-flex justify-content-between align-items-center border p-2 mb-2 rounded">
          <span>{{ benefit }}</span>
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
                    {{ benefit }}
                  </label>
                </div>
  
                <!-- Warning if user try to add more than max -->
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
  import { Modal } from 'bootstrap';
  
  export default {
    name: 'EmployeeBenefits',
    data() {
      return {
        selectedBenefits: ['Seguro privado', 'Plan dental'],
        allBenefits: ['Seguro privado', 'Plan dental', 'Gimnasio', 'Transporte', 'Almuerzo subsidiado'],
        tempSelection: [],
        modalInstance: null,
        maxBenefits: 3,
      };
    },
    computed: {
      availableBenefits() {
        return this.allBenefits.filter(b => !this.selectedBenefits.includes(b));
      },
      exceedsLimit() {
        return (this.selectedBenefits.length + this.tempSelection.length) > this.maxBenefits;
      }
    },
    methods: {
      openModal() {
        this.tempSelection = [];
        if (!this.modalInstance) {
          this.modalInstance = new Modal(this.$refs.benefitsModal);
        }
        this.modalInstance.show();
      },
      addSelectedBenefits() {
        if (!this.exceedsLimit) {
          this.selectedBenefits.push(...this.tempSelection);
          this.modalInstance.hide();
        }
      },
      removeBenefit(index) {
        this.selectedBenefits.splice(index, 1);
      }
    }
  };
  </script>
  
  <style scoped>
  .card-title {
    font-weight: bold;
  }
  </style>
  