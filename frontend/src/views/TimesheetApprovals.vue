<template>
  <div class="container mt-4">
    <!-- Loading state -->
    <div v-if="isLoading" class="text-center py-5">
      <div class="spinner-border text-primary" role="status" style="width: 3rem; height: 3rem;">
        <span class="visually-hidden">Cargando...</span>
      </div>
      <p class="mt-3 text-muted">Cargando aprobaciones pendientes...</p>
    </div>

    <!-- Error state -->
    <div v-else-if="error" class="alert alert-danger" role="alert">
      <h5><i class="bi bi-exclamation-triangle me-2"></i>Error al cargar los datos</h5>
      <p>{{ error }}</p>
      <button class="btn btn-outline-danger" @click="loadPendingApprovals">
        <i class="bi bi-arrow-clockwise me-1"></i> Reintentar
      </button>
    </div>

    <!-- No pending approvals -->
    <div v-else-if="pendingApprovals.length === 0" class="text-center py-5">
      <div class="mb-4">
        <i class="bi bi-check-circle-fill text-success" style="font-size: 4rem;"></i>
      </div>
      <h4 class="text-muted">No hay aprobaciones pendientes</h4>
      <p class="text-muted">Todos los timesheets están al día.</p>
      <button class="btn btn-primary" @click="loadPendingApprovals">
        <i class="bi bi-arrow-clockwise me-1"></i> Actualizar
      </button>
    </div>

    <!-- Pending approvals list -->
    <div v-else>
      <div class="row mb-3">
        <div class="col">
          <div class="alert alert-info d-flex align-items-center">
            <i class="bi bi-info-circle me-2"></i>
            <span>
              Se encontraron <strong>{{ totalPendingDays }}</strong> días pendientes de aprobación 
              de <strong>{{ pendingApprovals.length }}</strong> empleados.
            </span>
          </div>
        </div>
        <div class="col-auto">
          <button class="btn btn-outline-primary" @click="loadPendingApprovals">
            <i class="bi bi-arrow-clockwise me-1"></i> Actualizar
          </button>
        </div>
      </div>

      <!-- Employee table -->
      <div class="card shadow">
        <div class="card-body">
          <div class="table-responsive" style="max-height: 600px; overflow-y: auto">
            <table class="table table-striped table-bordered">
              <thead class="table-dark sticky-header">
                <tr>
                  <th>Nombre Completo</th>
                  <th>Cédula</th>
                  <th>Días Pendientes</th>
                  <th>Última Actualización</th>
                  <th>Acciones</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="approval in pendingApprovals" :key="approval.employeeId">
                  <td>
                    <strong>{{ approval.fullName || 'N/A' }}</strong>
                  </td>
                  <td>{{ formatCedula(approval.cedula) }}</td>
                  <td>
                    <span class="badge bg-warning text-dark">
                      {{ approval.pendingDaysCount }} días
                    </span>
                  </td>
                  <td>
                    <small class="text-muted">
                      <i class="bi bi-clock me-1"></i>
                      {{ dateService.getTimeAgo(approval.latestSubmissionTimestamp) }}
                    </small>
                  </td>
                  <td>
                    <button 
                      class="btn btn-sm btn-outline-primary"
                      @click="viewEmployeePendingDays(approval.employeeId)"
                      :disabled="loadingEmployeeId === approval.employeeId"
                    >
                      <span v-if="loadingEmployeeId === approval.employeeId" class="spinner-border spinner-border-sm me-1"></span>
                      <i v-else class="bi bi-eye me-1"></i>
                      Ver detalles
                    </button>
                  </td>
                </tr>
                <tr v-if="pendingApprovals.length === 0">
                  <td colspan="5" class="text-center">No se encontraron empleados con días pendientes.</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>

    <!-- Employee Days Modal -->
    <div class="modal fade" id="employeeDaysModal" tabindex="-1" ref="employeeDaysModal">
      <div class="modal-dialog modal-xl">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">
              <i class="bi bi-calendar-check me-2"></i>
              Días pendientes de aprobación
            </h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
          </div>
          
          <div class="modal-body">
            <!-- Loading days -->
            <div v-if="loadingDays" class="text-center py-4">
              <div class="spinner-border" role="status">
                <span class="visually-hidden">Cargando días...</span>
              </div>
              <p class="mt-2">Cargando días pendientes...</p>
            </div>

            <!-- Days list -->
            <div v-else-if="selectedEmployeeDays.length > 0">
              <div class="table-responsive">
                <table class="table table-hover">
                  <thead class="table-dark">
                    <tr>
                      <th style="width: 15%;">Fecha</th>
                      <th style="width: 10%;">Horas</th>
                      <th style="width: 55%;">Descripción</th>
                      <th style="width: 20%;">Acciones</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr v-for="day in selectedEmployeeDays" :key="day.id">
                      <td>
                        <strong>{{ dateService.formatDate(day.date) }}</strong>
                        <br>
                        <small class="text-muted">{{ dateService.getDayName(day.date) }}</small>
                      </td>
                      <td>
                        <span class="badge bg-info">{{ day.hoursWorked || 0 }} hrs</span>
                      </td>
                      <td>
                        <div class="description-cell">
                          {{ day.workDescription || 'Sin descripción' }}
                        </div>
                      </td>
                      <td>
                        <div class="btn-group" role="group">
                          <button 
                            class="btn btn-sm btn-success"
                            @click="approveDay(day.id)"
                            :disabled="approvingDayId === day.id"
                          >
                            <span v-if="approvingDayId === day.id" class="spinner-border spinner-border-sm me-1"></span>
                            <i v-else class="bi bi-check me-1"></i>
                            Aprobar
                          </button>
                        </div>
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>

            <!-- No days -->
            <div v-else class="text-center py-4">
              <i class="bi bi-calendar-x text-muted" style="font-size: 3rem;"></i>
              <p class="mt-2 text-muted">No se encontraron días pendientes para este empleado.</p>
            </div>
          </div>
          
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import approvalService from '@/services/approvalService';
import currentUserService from '@/services/currentUserService';
import dateService from '@/services/dateService';
import { Modal } from 'bootstrap';

export default {
  name: 'TimesheetApprovals',
  data() {
    return {
      pendingApprovals: [],
      selectedEmployeeDays: [],
      selectedEmployeeId: null,
      isLoading: false,
      loadingDays: false,
      loadingEmployeeId: null,
      approvingDayId: null,
      error: null,
      employeeDaysModal: null,
      dateService: dateService
    };
  },
  computed: {
    totalPendingDays() {
      return this.pendingApprovals.reduce((total, approval) => total + approval.pendingDaysCount, 0);
    }
  },
  async mounted() {
    await this.loadPendingApprovals();
    this.employeeDaysModal = new Modal(this.$refs.employeeDaysModal);
  },
  methods: {
    formatCedula(cedula) {
      if (!cedula) return 'N/A';
      const cleanCedula = cedula.replace(/[-\s]/g, '');
      
      // Apply format #-####-####
      if (cleanCedula.length === 9) {
        return `${cleanCedula.substring(0, 1)}-${cleanCedula.substring(1, 5)}-${cleanCedula.substring(5)}`;
      }
      else {
        // Return original if format is unexpected
        return cedula;
      }
    },

    async loadPendingApprovals() {
      this.isLoading = true;
      this.error = null;
      
      try {
        this.pendingApprovals = await approvalService.getPendingApprovalsByEmployeeWithInfo();
      } catch (error) {
        console.error('Error loading pending approvals:', error);
        this.error = error.message || 'Error al cargar las aprobaciones pendientes.';
      } finally {
        this.isLoading = false;
      }
    },

    async viewEmployeePendingDays(employeeId) {
      this.selectedEmployeeId = employeeId;
      this.loadingEmployeeId = employeeId;
      this.loadingDays = true;
      this.selectedEmployeeDays = [];
      
      try {
        this.employeeDaysModal.show();
        const fetchedDays = await approvalService.getPendingDaysByEmployee(employeeId);
        this.selectedEmployeeDays = fetchedDays;

        console.log('Días pendientes fetched:', fetchedDays);
        if (fetchedDays && fetchedDays.length > 0) {
            const firstDay = fetchedDays[0];
            console.log('Timestamp crudo del primer día:', firstDay.lastSubmitTimestamp);
            const dateObj = new Date(firstDay.lastSubmitTimestamp);
            console.log('Objeto Date del primer día:', dateObj);
            console.log('Timestamp en ISO string (siempre UTC):', dateObj.toISOString());
            console.log('Timestamp en string local:', dateObj.toString());
        }

      } catch (error) {
        console.error('Error loading employee days:', error);
        // TODO: Show error in modal
      } finally {
        this.loadingDays = false;
        this.loadingEmployeeId = null;
      }
    },

    async approveDay(dayId) {
      this.approvingDayId = dayId;
      
      try {
        // Get supervisor ID from current user information
        const userInfo = currentUserService.getCurrentUserInformationFromLocalStorage();
        if (!userInfo || !userInfo.idNaturalPerson) {
          throw new Error('No se pudo obtener la información del usuario supervisor. Por favor, inicie sesión nuevamente.');
        }
        const supervisorId = userInfo.idNaturalPerson;
        
        await approvalService.approveDay(dayId, supervisorId);
        
        // Remove the approved day from the list
        this.selectedEmployeeDays = this.selectedEmployeeDays.filter(day => day.id !== dayId);
        
        // Refresh the main list
        await this.loadPendingApprovals();
        
        // If no more days for this employee, close modal
        if (this.selectedEmployeeDays.length === 0) {
          this.employeeDaysModal.hide();
        }
        
      } catch (error) {
        console.error('Error approving day:', error);
        alert('Error al aprobar el día: ' + (error.message || 'Error desconocido'));
      } finally {
        this.approvingDayId = null;
      }
    },
  }
}
</script>

<style scoped>
.container {
  max-width: 1200px;
}

.card {
  transition: all 0.3s ease;
}

.card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15) !important;
}

.table th {
  font-weight: 600;
  font-size: 0.9rem;
}

.badge {
  font-size: 0.8rem;
}

.btn-group .btn {
  transition: all 0.2s ease;
}

.btn:hover {
  transform: scale(1.02);
}

.description-cell {
  word-wrap: break-word;
  word-break: break-word;
  white-space: normal;
  line-height: 1.4;
  max-height: none;
}

.modal-xl {
  max-width: 1200px;
}

.sticky-header th {
  position: sticky;
  top: 0;
  z-index: 10;
}

.table-responsive {
  border-radius: 0.5rem;
}

.table {
  margin-bottom: 0;
}

@media (max-width: 768px) {
  .modal-xl {
    max-width: 95%;
  }
  
  .card-body {
    padding: 1rem;
  }
  
  .table-responsive {
    font-size: 0.875rem;
  }
}
</style> 