<template>
    <!-- Layout for out of period days -->
    <div v-if="isOutOfPeriod" class="row border-bottom day-row out-of-period-row g-4">
        <div class="col-md-2 pe-4">
            <div class="d-flex align-items-center">
                <span class="day-name-out-of-period me-2">{{ day }}</span>
                <small class="text-muted">{{ formattedDate }}</small>
            </div>
        </div>
        <div class="col-md-2 px-3"></div>
        <div class="col-md-6 px-4"></div>
        <div class="col-md-2 ps-2 text-center">
            <div class="d-flex justify-content-center">
                <span class="badge bg-secondary opacity-75 me-2">Fuera de período</span>
            </div>
        </div>
    </div>

    <!-- Normal layout for days in the current period -->
    <div v-else class="row py-3 border-bottom day-row g-4" 
         :class="{ 
           'bg-light': status === 'Aceptado' && !isCurrentDay,
           'current-day': isCurrentDay
         }">
        <div class="col-md-2 pe-4">
            <div class="day-column-wrapper">
                <!-- Day name container -->
                <div class="day-name-section">
                    <span 
                      class="day-name-container" 
                      :class="{ 
                        'fw-bold text-primary': isCurrentDay,
                        'transitioning': isTransitioning
                      }"
                      @mouseenter="handleHoverEnter"
                      @mouseleave="handleHoverLeave">
                        {{ displayText }}
                    </span>
                </div>
                
                <!-- Current day indicators - positioned independently -->
                <div v-if="isCurrentDay" class="current-day-indicators">
                    <span class="badge bg-primary current-day-badge">HOY</span>
                </div>
            </div>
        </div>
        <div class="col-md-2 text-center px-3">
            <input 
                type="number" 
                class="form-control text-center" 
                v-model.number="workHours"
                :disabled="status === 'Aceptado' || !backendId"
                :placeholder="!backendId ? 'N/A' : '0'"
                min="0"
                max="24"
                @blur="autoSaveDay"
                @focus="capturePreChangeValue('hours')"
                @input="markAsChanged"
                @change="onFieldChange"
            >
        </div>
        <div class="col-md-6 px-4">
            <textarea 
                class="form-control" 
                v-model="workDescription" 
                :disabled="status === 'Aceptado' || !backendId" 
                :placeholder="!backendId ? 'Este día no existe en el timesheet' : 'Descripción del trabajo (requerida)...'"
                :class="{ 'is-invalid': saveStatus === 'error' && (!workDescription || workDescription.trim() === '') }"
                rows="3"
                @blur="autoSaveDay"
                @focus="capturePreChangeValue('description')"
                @input="markAsChanged"
            ></textarea>
            
            <!-- Error message for empty description -->
            <div v-if="saveStatus === 'error' && (!workDescription || workDescription.trim() === '')" class="invalid-feedback">
                La descripción es requerida
            </div>
        </div>
        <div class="col-md-2 ps-2 text-center">
            <div class="d-flex justify-content-center">
                <!-- Status visual with badge -->
                <span class="badge me-2" :class="statusBadgeClass">
                    <i v-if="saveStatus === 'error'" class="bi bi-exclamation-triangle me-1"></i>
                    <i v-if="!backendId" class="bi bi-dash-circle me-1"></i>
                    {{ displayStatus }}
                </span>
                
                <!-- Manual button only if there are unsaved changes and backendId exists -->
                <button v-if="hasUnsavedChanges && !isSaving && backendId" 
                        class="btn btn-sm btn-outline-primary btn-extra-small"
                        @click="saveDay"
                        title="Guardar cambios">
                    <i class="bi bi-floppy"></i>
                </button>
            </div>
            
            <!-- Unsaved changes message -->
            <small v-if="hasUnsavedChanges && !isSaving && backendId" class="text-muted d-block mt-1">
                Cambios sin guardar
            </small>
            
            <!-- Message for days without backendId -->
            <small v-else-if="!backendId" class="text-muted d-block mt-1">
                No disponible
            </small>
        </div>
    </div>
</template>

<script>
export default {
  name: 'DayRow',
  props: {
    day: {
      type: String,
      required: true
    },
    date: {
      type: Date,
      required: true
    },
    hours: {
      type: Number,
      default: 0
    },
    description: {
      type: String,
      default: ''
    },
    status: {
      type: String,
      default: 'Sin revisión'
    },
    isCurrentDay: {
      type: Boolean,
      default: false
    },
    isOutOfPeriod: {
      type: Boolean,
      default: false
    },
    backendId: {
      type: String,
      default: null
    }
  },
  data() {
    return {
      workHours: this.hours,
      workDescription: this.description,
      isSaving: false,
      hasUnsavedChanges: false,
      saveStatus: null, // 'saved', 'error', null
      // Store original values independently
      savedHours: this.hours,
      savedDescription: this.description,
      // Values to capture state before watchers
      preChangeHours: this.hours,
      preChangeDescription: this.description,
      changeTimeout: null,
      // State for hover with delay and transition
      isHovering: false,
      hoverTimeout: null,
      isTransitioning: false
    }
  },
  computed: {
    statusBadgeClass() {
      if (this.isSaving) return 'bg-info';
      if (this.saveStatus === 'saved') return 'bg-success';
      if (this.saveStatus === 'error') return 'bg-danger';
      if (this.hasUnsavedChanges) return 'bg-warning';
      
      switch (this.status) {
        case 'Aceptado': return 'bg-success';
        case 'Sin revisión': return 'bg-secondary';
        default: return 'bg-secondary';
      }
    },
    displayStatus() {
      if (this.isSaving) return 'Guardando...';
      if (this.saveStatus === 'saved') return 'Guardado';
      if (this.saveStatus === 'error') {
        // Check if it's a validation error
        if (!this.workDescription || this.workDescription.trim() === '') {
          return 'Descripción requerida';
        }
        return 'Error al guardar';
      }
      if (!this.backendId) return 'No disponible';
      if (this.hasUnsavedChanges) return 'Modificado';
      return this.status;
    },
    formattedDate() {
      return this.date.toLocaleDateString('es-ES', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
      });
    },
    displayText() {
      return this.isHovering ? this.formattedDate : this.day;
    }
  },
  watch: {
    hours(newVal) {
      // Solo actualizar cuando no hay cambios pendientes (carga inicial o después de guardar)
      if (!this.hasUnsavedChanges) {
        this.workHours = newVal;
      }
    },
    description(newVal) {
      // Solo actualizar cuando no hay cambios pendientes (carga inicial o después de guardar)
      if (!this.hasUnsavedChanges) {
        this.workDescription = newVal;
      }
    },
    workHours(newVal) {
      this.$emit('update:hours', newVal);
    },
    workDescription(newVal) {
      this.$emit('update:description', newVal);
    }
  },
  methods: {
    markAsChanged() {
      // Check if there are actual changes compared to saved original values
      const hoursChanged = Number(this.workHours) !== Number(this.savedHours);
      const descriptionChanged = String(this.workDescription || '') !== String(this.savedDescription || '');
      
      this.hasUnsavedChanges = hoursChanged || descriptionChanged;
      
      if (this.hasUnsavedChanges) {
        this.saveStatus = null;
      }
    },
    
    async autoSaveDay() {
      if (!this.hasUnsavedChanges || this.status === 'Aceptado' || !this.backendId) return;
      
      setTimeout(() => {
        this.saveDay();
      }, 300);
    },
    
    async saveDay() {
      if (this.isSaving || this.status === 'Aceptado') return;
      
      // Validate that description is not empty (backend requires it)
      if (!this.workDescription || this.workDescription.trim() === '') {
        this.saveStatus = 'error';
        console.warn('Cannot save day without description');
        // Show error message briefly and then reset
        setTimeout(() => {
          if (this.saveStatus === 'error') {
            this.saveStatus = null;
          }
        }, 3000);
        return;
      }
      
      this.isSaving = true;
      this.saveStatus = null;
      
      try {
        this.$emit('save-day', {
          day: this.day,
          hours: this.workHours,
          description: this.workDescription,
          backendId: this.backendId
        });
        
        // Wait for parent to handle the save
        // The parent will throw an error if save fails
        await new Promise((resolve) => {
          setTimeout(resolve, 100);
        });
        
        this.updateSavedValues();
        this.hasUnsavedChanges = false;
        this.saveStatus = 'saved';
        
        // Clear 'saved' state after 2 seconds
        setTimeout(() => {
          if (this.saveStatus === 'saved') {
            this.saveStatus = null;
          }
        }, 2000);
        
      } catch (error) {
        console.error('Error saving day:', error);
        this.saveStatus = 'error';
      } finally {
        this.isSaving = false;
      }
    },
    
    capturePreChangeValue(field) {
      if (field === 'hours') {
        this.preChangeHours = this.workHours;
      } else if (field === 'description') {
        this.preChangeDescription = this.workDescription;
      }
    },
    updateSavedValues() {
      this.savedHours = Number(this.workHours);
      this.savedDescription = String(this.workDescription || '');
    },
    onFieldChange() {
      this.markAsChanged();
      
      if (this.hasUnsavedChanges) {
        clearTimeout(this.changeTimeout);
        this.changeTimeout = setTimeout(() => {
          this.autoSaveDay();
        }, 1000);
      }
    },
    
    handleHoverEnter() {
      clearTimeout(this.hoverTimeout);
      
      if (!this.isHovering) {
        this.isTransitioning = true;
        setTimeout(() => {
          this.isHovering = true;
          setTimeout(() => {
            this.isTransitioning = false;
          }, 150);
        }, 100);
      }
    },
    
    handleHoverLeave() {
      // Delay before returning to the day name
      this.hoverTimeout = setTimeout(() => {
        this.isTransitioning = true;
        setTimeout(() => {
          this.isHovering = false;
          setTimeout(() => {
            this.isTransitioning = false;
          }, 150);
        }, 100);
      }, 800); // 800ms de delay
    }
  },
  mounted() {
    this.updateSavedValues();
  },
  beforeUnmount() {
    if (this.changeTimeout) {
      clearTimeout(this.changeTimeout);
    }
    if (this.hoverTimeout) {
      clearTimeout(this.hoverTimeout);
    }
  }
}
</script>

<style scoped>
textarea.form-control {
    resize: none;
}

.day-row {
    transition: all 0.3s ease;
    overflow: hidden;
}

.current-day {
    background: linear-gradient(to right, rgba(13, 110, 253, 0.1), transparent);
    border-left: 5px solid #0d6efd;
    position: relative;
    overflow: hidden;
    border-radius: 0.25rem;
    margin: 0px;
    box-sizing: border-box;
}

.day-row:hover {
    background-color: rgba(0, 0, 0, 0.02);
}

.current-day:hover {
    background: linear-gradient(to right, rgba(13, 110, 253, 0.15), transparent);
}

.spinning {
    animation: spin 1s linear infinite;
}

@keyframes spin {
    from { transform: rotate(0deg); }
    to { transform: rotate(360deg); }
}

.badge {
    transition: all 0.3s ease;
}

.btn-outline-primary {
    border-width: 1px;
    padding: 0.25rem 0.5rem;
}

.btn-outline-primary:hover {
    transform: scale(1.05);
}

.btn-extra-small {
  padding: 0.15rem 0.3rem;
  font-size: 0.75rem;
}

.day-name-container {
    cursor: pointer;
    padding: 0.25rem 0.5rem;
    border-radius: 0.25rem;
    transition: all 0.4s ease;
    min-width: 80px;
    text-align: center;
    position: relative;
    overflow: hidden;
    display: inline-block;
    margin: 0;
    font-weight: inherit;
    opacity: 1;
}

.day-column-wrapper {
    position: relative;
    width: 100%;
}

.day-name-section {
    position: relative;
    z-index: 2;
}

.current-day-indicators {
    position: absolute;
    top: -30px;
    left: auto;
    transform: none;
    display: flex;
    align-items: center;
    gap: 0.25rem;
    z-index: 3;
}

.current-day-icon {
    font-size: 0.8rem;
    opacity: 0.7;
}

.current-day-badge {
    font-size: 0.6rem;
    padding: 0.1rem 0.3rem;
    opacity: 0.9;
    font-weight: 500;
}

.day-name-container.transitioning {
    opacity: 0.6;
    transform: scale(0.98);
}

.day-name-container:hover {
    animation: text-glow 0.3s ease-in-out;
}

@keyframes text-glow {
    0% { 
        opacity: 1; 
        text-shadow: none; 
    }
    50% { 
        opacity: 0.7; 
        text-shadow: 0 0 8px rgba(0, 123, 255, 0.3); 
    }
    100% { 
        opacity: 1; 
        text-shadow: 0 0 4px rgba(0, 123, 255, 0.2); 
    }
}

.out-of-period-row {
    background-color: #f8f9fa !important;
    opacity: 0.7;
    overflow: visible !important;
    position: relative !important;
    display: block !important;
    width: 100% !important;
    padding: 1rem 0 !important;
    margin: 0 0 2px 0 !important;
    min-height: 60px !important;
    box-sizing: border-box !important;
    transition: background-color 0.3s ease, opacity 0.3s ease !important;
}

.out-of-period-row:hover {
    background-color: #e9ecef !important;
    opacity: 0.8;
}

.out-of-period-row .row {
    margin: 0 !important;
    padding: 0 !important;
}

.day-name-out-of-period {
    font-weight: 500;
    color: #6c757d;
    font-size: 0.95rem;
}
</style>
