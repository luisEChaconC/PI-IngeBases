<template>
  <div
    v-if="visible"
    class="modal-backdrop"
    @mousedown.self="$emit('close')"
  >
    <div class="modal-dialog">
      <div class="modal-content p-5">
        <h2 class="text-center modal-title-custom">Seleccione el periodo de la planilla</h2>
        <h5 class="text-center text-muted modal-subtitle-custom">Esta empresa paga de manera {{ paymentTypeSpanish }}</h5>
        <div v-if="errorMessage" class="alert alert-danger text-center my-1">
          {{ errorMessage }}
        </div>
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
import axios from "axios";
import currentUserService from "@/services/currentUserService";
import DatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css';

// --- Utility functions ---
function formatDateDisplay(dateStr) {
  if (!dateStr) return '';
  const [year, month, day] = dateStr.split('-');
  return `${day}/${month}/${year}`;
}

function getRangeDates(baseDate, days) {
  // Returns an array of Date objects for the range
  return Array.from({ length: days }, (_, i) => {
    const d = new Date(baseDate.getTime());
    d.setDate(baseDate.getDate() + i);
    return d;
  });
}

function formatDateToString(dateObj) {
  const yyyy = dateObj.getFullYear();
  const mm = String(dateObj.getMonth() + 1).padStart(2, '0');
  const dd = String(dateObj.getDate()).padStart(2, '0');
  return `${yyyy}-${mm}-${dd}`;
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
      highlightedDates: [], // Array of Date objects
      paymentType: 'weekly', // default, will be updated
      errorMessage: ''
    }
  },
  computed: {
    formattedStartDate() { return formatDateDisplay(this.startDate); },
    formattedEndDate() { return formatDateDisplay(this.endDate); },
    paymentTypeSpanish() {
      switch (this.paymentType) {
        case 'weekly': return 'semanal';
        case 'biweekly': return 'quincenal';
        case 'monthly': return 'mensual';
        default: return this.paymentType;
      }
    }
  },
  async created() {
    try {
      const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage();
      const companyId = currentUserInformation.companyId;
      const response = await axios.get(`https://localhost:5000/api/Company/GetCompanyById/${companyId}`);
      this.paymentType = response.data && response.data.paymentType
        ? response.data.paymentType.toLowerCase()
        : 'weekly'
    } catch (e) {
      this.errorMessage = 'Error al obtener el tipo de pago de la empresa';
      this.paymentType = 'weekly';
    }
  },
  methods: {
    async handleGenerate() {
      this.errorMessage = '';
      if (!this.startDate || !this.endDate) {
        this.errorMessage = 'Debe seleccionar un periodo válido para la planilla.';
        return;
      }

      const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage();
      const companyId = currentUserInformation.companyId;
      const payrollManagerId = currentUserInformation.idPerson;

      // Prepare the payload
      const payload = {
        startDate: this.startDate,
        endDate: this.endDate,
        companyId: companyId,
        payrollManagerId: payrollManagerId
      };

      try {
        await axios.post('https://localhost:5000/api/Payroll', payload);
        this.$emit('payroll-generated'); // Emit event for parent to refresh
        this.$emit('close');
      } catch (error) {
        // Default message
        let message = 'Error al generar la planilla';

        // If backend sends errorType, show a specific message
        const errorType = error.response?.data?.errorType;
        if (errorType === 'AlreadyExists') {
          message = 'Ya existe una planilla para el periodo seleccionado.';
        } else if (errorType === 'NoEmployees') {
          message = 'No hay empleados registrados en la empresa.';
        } else if (errorType === 'InvalidPaymentType') {
          message = 'El tipo de pago definido no coincide con el pago de la empresa.';
        }

        this.errorMessage = message;
      }
    },
    async handleDayChange(date) {
      const type = this.paymentType;
      const days = type === 'biweekly' ? 15 : type === 'monthly' ? 30 : 7;

      if (!date) {
        this.highlightedDates = [];
        this.startDate = this.endDate = null;
        return;
      }

      // Always use local midnight to avoid timezone shift
      let baseDateLocal;
      if (typeof date === 'string') {
        const d = new Date(date);
        baseDateLocal = new Date(d.getFullYear(), d.getMonth(), d.getDate());
      } else if (date instanceof Date) {
        baseDateLocal = new Date(date.getFullYear(), date.getMonth(), date.getDate());
      } else if (date.year && date.month && date.day) {
        baseDateLocal = new Date(date.year, date.month - 1, date.day);
      } else {
        baseDateLocal = new Date();
      }

      // Highlight expects array of Date objects
      this.highlightedDates = getRangeDates(baseDateLocal, days);

      // For backend, format as yyyy-mm-dd
      this.startDate = formatDateToString(this.highlightedDates[0]);
      this.endDate = formatDateToString(this.highlightedDates[this.highlightedDates.length - 1]);
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
.modal-title-custom {
  font-size: 1.5rem;
  font-weight: bold;
  margin-bottom: 0.5rem;
}
.modal-subtitle-custom {
  font-size: 1.15rem;
  font-weight: 400;
  margin-bottom: 1.5rem;
}
</style>