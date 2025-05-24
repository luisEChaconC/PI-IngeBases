<template>
  <div class="container my-5">
    <div class="position-relative mb-4">
      <router-link
        to="/main-menu"
        class="btn btn-outline-secondary"
        title="Volver al menú principal"
      >
        ← Volver
      </router-link>
    </div>
    <div class="card shadow">
      <div class="card-body">
        <div class="d-flex justify-content-between mx-2 mb-4">
          <h2 class="card-title">Planillas</h2>
          <router-link to="/add-payroll" class="btn btn-dark">
            <i class="fas fa-plus me-2" />
            Nueva Planilla
          </router-link>
        </div>
        <div class="table-responsive" style="max-height: 600px; overflow-y: auto">
          <table class="table table-striped table-bordered">
            <thead class="table-dark sticky-header">
              <tr>
                <th>Nombre completo del encargado</th>
                <th>Fecha inicio</th>
                <th>Fecha fin</th>
                <th>Monto deducido</th>
                <th>Salario bruto</th>
                <th>Salario neto</th>
                <th>Estado</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(payroll, index) in payrolls" :key="index">
                <td>{{ payroll.managerFullName }}</td>
                <td>{{ formatDate(payroll.startDate) }}</td>
                <td>{{ formatDate(payroll.endDate) }}</td>
                <td>{{ formatCurrency(payroll.deductedAmount) }}</td>
                <td>{{ formatCurrency(payroll.grossSalary) }}</td>
                <td>{{ formatCurrency(payroll.netSalary) }}</td>
                <td>{{ translateState(payroll.state) }}</td>
              </tr>
              <tr v-if="payrolls.length === 0">
                <td colspan="7" class="text-center">No se encontraron planillas.</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'PayrollsList',
  data() {
    return {
      payrolls: [
        {
          managerFullName: 'Carlos Ramírez',
          startDate: '2025-05-01',
          endDate: '2025-05-15',
          deductedAmount: 120000,
          grossSalary: 1500000,
          netSalary: 1380000,
          state: 'Open'
        },
        {
          managerFullName: 'María Fernández',
          startDate: '2025-04-16',
          endDate: '2025-04-30',
          deductedAmount: 95000,
          grossSalary: 1400000,
          netSalary: 1305000,
          state: 'Closed'
        },
        {
          managerFullName: 'Luis Gómez',
          startDate: '2025-05-01',
          endDate: '2025-05-15',
          deductedAmount: 110000,
          grossSalary: 1350000,
          netSalary: 1240000,
          state: 'Processing'
        },
        {
          managerFullName: 'Ana Solís',
          startDate: '2025-03-01',
          endDate: '2025-03-15',
          deductedAmount: 105000,
          grossSalary: 1250000,
          netSalary: 1145000,
          state: 'Closed'
        },
        {
          managerFullName: 'Pedro Vargas',
          startDate: '2025-02-16',
          endDate: '2025-02-28',
          deductedAmount: 98000,
          grossSalary: 1320000,
          netSalary: 1222000,
          state: 'Open'
        },
        {
          managerFullName: 'Laura Jiménez',
          startDate: '2025-01-01',
          endDate: '2025-01-15',
          deductedAmount: 112000,
          grossSalary: 1450000,
          netSalary: 1338000,
          state: 'Closed'
        },
        {
          managerFullName: 'Jorge Castro',
          startDate: '2024-12-16',
          endDate: '2024-12-31',
          deductedAmount: 101000,
          grossSalary: 1200000,
          netSalary: 1099000,
          state: 'Processing'
        },
        {
          managerFullName: 'Gabriela Mora',
          startDate: '2024-11-01',
          endDate: '2024-11-15',
          deductedAmount: 99000,
          grossSalary: 1280000,
          netSalary: 1181000,
          state: 'Closed'
        },
        {
          managerFullName: 'Ricardo Soto',
          startDate: '2024-10-16',
          endDate: '2024-10-31',
          deductedAmount: 87000,
          grossSalary: 1190000,
          netSalary: 1103000,
          state: 'Open'
        },
        {
          managerFullName: 'Patricia Méndez',
          startDate: '2024-09-01',
          endDate: '2024-09-15',
          deductedAmount: 93000,
          grossSalary: 1230000,
          netSalary: 1137000,
          state: 'Closed'
        },
        {
          managerFullName: 'Esteban Rojas',
          startDate: '2024-08-16',
          endDate: '2024-08-31',
          deductedAmount: 102000,
          grossSalary: 1370000,
          netSalary: 1268000,
          state: 'Processing'
        },
        {
          managerFullName: 'Daniela Porras',
          startDate: '2024-07-01',
          endDate: '2024-07-15',
          deductedAmount: 95000,
          grossSalary: 1300000,
          netSalary: 1205000,
          state: 'Closed'
        },
        {
          managerFullName: 'Andrés Chaves',
          startDate: '2024-06-16',
          endDate: '2024-06-30',
          deductedAmount: 99000,
          grossSalary: 1270000,
          netSalary: 1171000,
          state: 'Open'
        },
        {
          managerFullName: 'Sofía Navarro',
          startDate: '2024-05-01',
          endDate: '2024-05-15',
          deductedAmount: 108000,
          grossSalary: 1420000,
          netSalary: 1312000,
          state: 'Closed'
        },
        {
          managerFullName: 'Mauricio Quesada',
          startDate: '2024-04-16',
          endDate: '2024-04-30',
          deductedAmount: 97000,
          grossSalary: 1210000,
          netSalary: 1113000,
          state: 'Processing'
        },
        {
          managerFullName: 'Valeria Ureña',
          startDate: '2024-03-01',
          endDate: '2024-03-15',
          deductedAmount: 94000,
          grossSalary: 1240000,
          netSalary: 1146000,
          state: 'Closed'
        },
        {
          managerFullName: 'Oscar Salazar',
          startDate: '2024-02-16',
          endDate: '2024-02-28',
          deductedAmount: 99000,
          grossSalary: 1290000,
          netSalary: 1191000,
          state: 'Open'
        },
        {
          managerFullName: 'Marina Acuña',
          startDate: '2024-01-01',
          endDate: '2024-01-15',
          deductedAmount: 100000,
          grossSalary: 1350000,
          netSalary: 1250000,
          state: 'Closed'
        },
        {
          managerFullName: 'Felipe Araya',
          startDate: '2023-12-16',
          endDate: '2023-12-31',
          deductedAmount: 95000,
          grossSalary: 1320000,
          netSalary: 1225000,
          state: 'Processing'
        },
        {
          managerFullName: 'Natalia Céspedes',
          startDate: '2023-11-01',
          endDate: '2023-11-15',
          deductedAmount: 92000,
          grossSalary: 1200000,
          netSalary: 1108000,
          state: 'Closed'
        }
      ]
    }
  },
  methods: {
    formatDate(dateStr) {
      if (!dateStr) return ''
      const date = new Date(dateStr)
      return date.toLocaleDateString('es-CR')
    },
    formatCurrency(amount) {
      if (amount == null) return ''
      // Format as ": xxx.xxx.xxx,00" (colon at the start, no CRC)
      return '₡ ' + amount.toLocaleString('de-DE', { minimumFractionDigits: 2, maximumFractionDigits: 2 })
    },
    translateState(state) {
      switch (state) {
        case 'Open':
          return 'Abierta'
        case 'Closed':
          return 'Cerrada'
        case 'Processing':
          return 'Procesando'
        default:
          return 'Desconocido'
      }
    }
  }
}
</script>

<style scoped>
.card-title {
  font-size: 1.75rem;
  font-weight: bold;
}

.sticky-header th {
  position: sticky;
  top: 0;
  z-index: 10;
}

.table-responsive {
  border-radius: 0.5rem;
  /* Bootstrap handles overflow and responsiveness */
}

.table {
  margin-bottom: 0;
}
</style>