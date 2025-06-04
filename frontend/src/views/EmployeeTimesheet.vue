<template>
  <div class="container mt-4">
    <!-- Loading state -->
    <div v-if="isLoading" class="text-center py-4">
      <div class="spinner-border" role="status">
        <span class="visually-hidden">Cargando...</span>
      </div>
      <p class="mt-2">Cargando datos del timesheet...</p>
    </div>
    
    <!-- Error state -->
    <div v-else-if="error" class="alert alert-danger" role="alert">
      <h5>Error al cargar los datos</h5>
      <p>{{ error }}</p>
      <button class="btn btn-outline-danger" @click="loadCurrentWeek">
        <i class="bi bi-arrow-clockwise"></i> Reintentar
      </button>
    </div>
    
    <!-- Main content -->
    <div v-else>
      <!-- Header -->
      <div class="row fw-bold border-bottom pb-5 g-4">
        <div class="col-md-2 pe-4">Día</div>
        <div class="col-md-2 text-center px-3">Horas Trabajadas</div>
        <div class="col-md-6 text-center px-4">Descripción</div>
        <div class="col-md-2 text-center ps-2">Estado</div>
      </div>
      
      <!-- Days of the week -->
      <DayRow 
        v-for="(day, index) in days" 
        :key="index"
        :day="day.name"
        :date="day.date"
        v-model:hours="day.hours"
        v-model:description="day.description"
        :status="day.status"
        :isCurrentDay="day.isCurrent"
        :isOutOfPeriod="day.isOutOfPeriod"
        :isWeekend="day.isWeekend"
        :backendId="day.backendId"
        @save-day="handleSaveDay"
      />
      
      <!-- Summary info -->
      <div class="row mt-4">
        <div class="col">
          <div class="alert alert-info">
            <strong>Total de horas esta semana:</strong> {{ totalHours }} horas
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import DayRow from '@/components/timesheet/DayRow.vue';
import dateService from '@/services/dateService';
import timesheetService from '@/services/timesheetService';

export default {
  name: 'EmployeeTimesheet',
  components: {
    DayRow
  },
  data() {
    return {
      days: [],
      currentTimesheet: null,
      isLoading: false,
      error: null,
      employeeId: null
    };
  },
  async mounted() {
    await this.initializeTimesheet();
  },
  computed: {
    totalHours() {
      return this.days
        .filter(day => !day.isOutOfPeriod)
        .reduce((total, day) => total + (day.hours || 0), 0);
    },
    daysOutOfPeriod() {
      return this.days.filter(day => day.isOutOfPeriod).length;
    }
  },
  methods: {
    async initializeTimesheet() {
      this.isLoading = true;
      this.error = null;
      
      try {
        this.employeeId = timesheetService.getCurrentEmployeeId();
        
        await this.loadCurrentWeek();
      } catch (error) {
        console.error('Error initializing timesheet:', error);
        this.error = 'Error al inicializar el timesheet. Por favor, verifica tu conexión.';
      } finally {
        this.isLoading = false;
      }
    },

    async loadCurrentWeek() {
      try {
        this.isLoading = true;
        this.error = null;

        const today = await dateService.getCurrentDate();
        
        const timesheet = await timesheetService.getEmployeeTimesheetByDate(this.employeeId, today);
        
        if (!timesheet) {
          this.error = 'No se encontró un timesheet para la fecha actual.';
          return;
        }
        
        this.currentTimesheet = timesheet;
        
        const backendDays = await timesheetService.getDaysByTimesheetId(timesheet.id);
        
        await this.generateWeekDays(today, timesheet, backendDays);
        
      } catch (error) {
        console.error('Error loading timesheet:', error);
        this.error = error.response?.data?.message || 'Error al cargar los datos del timesheet.';
      } finally {
        this.isLoading = false;
      }
    },

    async generateWeekDays(referenceDate, timesheet, backendDays) {
      const weekDays = dateService.generateWeekDays(referenceDate);
      
      const timesheetStartDate = new Date(timesheet.startDate);
      const timesheetEndDate = new Date(timesheet.endDate);
      
      this.days = timesheetService.mapBackendDaysToFrontend(
        backendDays, 
        weekDays, 
        timesheetStartDate, 
        timesheetEndDate
      );
      
      const today = await dateService.getCurrentDate();
      const currentDayName = dateService.getCurrentDayName(today);
      
      this.days.forEach(day => {
        day.isCurrent = day.name === currentDayName && dateService.isSameDate(day.date, today);
      });
    },
    
    async handleSaveDay(dayData) {
      try {
        console.log('Saving individual day:', dayData);
        
        if (!dayData.backendId) {
          console.warn('Cannot save day without backendId:', dayData);
          throw new Error('Este día no existe en el backend y no puede ser actualizado. Solo los días que ya existen pueden ser modificados.');
        }
        
        const dayWithDate = {
          ...dayData,
          date: this.days.find(d => d.name === dayData.day)?.date
        };
        
        await timesheetService.saveDay(dayWithDate);
        
      } catch (error) {
        console.error('Error saving day:', error);
        throw error;
      }
    },
    
    formatDate(dateString) {
      const date = new Date(dateString);
      return date.toLocaleDateString('es-ES', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
      });
    }
  }
}
</script>

<style scoped>
.container {
  max-width: 1000px;
}

.spinner-border {
  width: 3rem;
  height: 3rem;
}
</style>
