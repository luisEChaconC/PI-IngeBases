<template>
  <div v-if="visible" class="modal-backdrop">
    <div class="modal-dialog">
      <div class="modal-content p-4">
        <div class="mb-3 mt-2">
          <h5>Seleccione el periodo de la planilla</h5>
          <DatePicker
            :enable-time-picker="false"
            @date-update="handleDayChange"
            v-model="period"
            :locale="'es'"
            :highlight="highlightedDates"
          />
        </div>
        <div class="mb-3">
          <div>
            <strong>Fecha de inicio:</strong>
            <span>{{ formattedStartDate }}</span>
          </div>
          <div>
            <strong>Fecha de fin:</strong>
            <span>{{ formattedEndDate }}</span>
          </div>
        </div>
        <div class="d-flex justify-content-center gap-2">
          <button class="btn btn-dark" @click="handleGenerate">Generar</button>
          <button class="btn btn-secondary" @click="$emit('close')">Cancelar</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import DatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css';

function formatDate(dateStr) {
  if (!dateStr) return '';
  // Always show as DD/MM/YYYY in Costa Rica timezone
  const date = new Date(dateStr);
  return date.toLocaleDateString('es-CR', { timeZone: 'America/Costa_Rica' });
}

export default {
  name: 'GeneratePayrollModal',
  components: { DatePicker },
  props: {
    visible: { type: Boolean, required: true }
  },
  data() {
    return {
      period: null, // The selected day by the user
      startDate: null,
      endDate: null,
      highlightedDates: [],
    }
  },
  computed: {
    formattedStartDate() {
      return formatDate(this.startDate);
    },
    formattedEndDate() {
      return formatDate(this.endDate);
    }
  },
  methods: {
    handleGenerate() {
      this.$emit('generate', { startDate: this.startDate, endDate: this.endDate });
    },
    handleDayChange(date) {
      // Mocked type for demonstration
      const type = 'weekly'; // Change to 'weekly', 'biweekly', or 'monthly' as needed

      let days = 8;
      if (type === 'biweekly') days = 15;
      if (type === 'monthly') days = 30;

      if (!date) {
        this.highlightedDates = [];
        this.startDate = null;
        this.endDate = null;
        return;
      }
      const baseDate = new Date(date);
      this.highlightedDates = Array.from({ length: days }, (_, i) => {
        const d = new Date(baseDate);
        d.setDate(baseDate.getDate() + i);
        return d.toISOString().split('T')[0];
      });
      this.startDate = baseDate.toISOString().split('T')[0];
      const lastDate = new Date(baseDate);
      lastDate.setDate(baseDate.getDate() + days - 1);
      this.endDate = lastDate.toISOString().split('T')[0];
    }
  }
}
</script>

<style scoped>
.modal-backdrop {
  position: fixed;
  top: 0; left: 0; right: 0; bottom: 0;
  background: rgba(0,0,0,0.3);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1050;
}
.modal-dialog {
  background: #fff;
  border-radius: 8px;
  min-width: 350px;
  max-width: 90vw;
  box-shadow: 0 2px 16px rgba(0,0,0,0.2);
}
</style>