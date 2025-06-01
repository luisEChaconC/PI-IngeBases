<template>
  <div
    v-if="visible"
    class="modal-backdrop"
    @mousedown.self="$emit('close')"
  >
    <div class="modal-dialog">
      <div class="modal-content p-5">
        <h5 class="text-center">Seleccione el periodo de la planilla</h5>
        <div class="my-4">
          <DatePicker
            :enable-time-picker="false"
            @date-update="handleDayChange"
            v-model="period"
            locale="es"
            :highlight="highlightedDates"
            select-text="Seleccionar"
            cancel-text="Cancelar"
            :format="formatPicker"
          />
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

// --- Utility functions ---
function pad(n) {
  return n.toString().padStart(2, '0');
}

function toCRDate(date) {
  // Returns a Date object in Costa Rica time
  return new Date(date.toLocaleString('en-US', { timeZone: 'America/Costa_Rica' }));
}

function toCRDateString(date) {
  // Returns YYYY-MM-DD in Costa Rica time
  const d = toCRDate(date);
  return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}`;
}

function formatDateDisplay(dateStr) {
  if (!dateStr) return '';
  const [year, month, day] = dateStr.split('-');
  return `${day}/${month}/${year}`;
}

function getRangeDates(baseDate, days, formatter) {
  // Returns an array of formatted dates for the range
  return Array.from({ length: days }, (_, i) => {
    const d = new Date(baseDate.getTime());
    d.setDate(baseDate.getDate() + i);
    return formatter(d);
  });
}

// --- Vue component ---
export default {
  name: 'GeneratePayrollModal',
  components: { DatePicker },
  props: { visible: { type: Boolean, required: true } },
  data() {
    return {
      period: null,
      startDate: null,
      endDate: null,
      highlightedDates: [],
    }
  },
  computed: {
    formattedStartDate() { return formatDateDisplay(this.startDate); },
    formattedEndDate() { return formatDateDisplay(this.endDate); }
  },
  methods: {
    handleGenerate() {
      this.$emit('generate', { startDate: this.startDate, endDate: this.endDate });
    },
    handleDayChange(date) {
      const type = 'weekly'; // Change as needed
      const days = type === 'biweekly' ? 15 : type === 'monthly' ? 30 : 7;

      if (!date) {
        this.highlightedDates = [];
        this.startDate = this.endDate = null;
        return;
      }

      const baseDateCR = toCRDate(date);

      // Highlight uses ISO (picker expects this format)
      this.highlightedDates = getRangeDates(baseDateCR, days, d => d.toISOString().split('T')[0]);

      // Start/end use Costa Rica time string
      this.startDate = toCRDateString(baseDateCR);
      const endDateCR = new Date(baseDateCR.getTime());
      endDateCR.setDate(baseDateCR.getDate() + days - 1);
      this.endDate = toCRDateString(endDateCR);
    },
    formatPicker() {
      if (this.startDate && this.endDate) {
        const [sy, sm, sd] = this.startDate.split('-');
        const [ey, em, ed] = this.endDate.split('-');
        return `${sd}/${sm}/${sy} - ${ed}/${em}/${ey}`;
      }
      return '';
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
  min-width: 500px;
  max-width: 90vw;
  box-shadow: 0 2px 16px rgba(0,0,0,0.2);
}
</style>