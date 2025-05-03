<template>
  <div class="container my-5">
    <div class="card shadow">
      <div class="card-body">
        <div class="d-flex justify-content-between mx-2 mb-4">
          <h2 class="card-title">Empleados</h2>
          <a href="#" class="btn btn-dark">
            <i class="fas fa-user-plus me-2"/>
            Nuevo Empleado
          </a>
        </div>
        <div class="table-responsive" style="max-height: 600px; overflow-y: auto;">
          <table class="table table-striped table-bordered">
            <thead class="table-dark sticky-header">
              <tr>
                <th>Nombre completo</th>
                <th>Cédula</th>
                <th>Posición</th>
                <th>Acciones</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(employee, index) in employees" :key="index">
                <td>{{ employee.fullName }}</td>
                <td>{{ employee.legalId }}</td>
                <td>{{ employee.position }}</td>
                <td class="d-flex justify-content-center">
                  <button class="btn btn-outline-dark btn-sm">
                    <i class="fas fa-eye"></i>
                  </button>
                </td>
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
  name: 'EmployeesList',
  data() {
    return {
      employees: [
        { fullName: 'Juan Pérez González', legalId: '1-101-123456', position: 'Colaborador' },
        { fullName: 'María Rodríguez López', legalId: '1-102-654321', position: 'Supervisor' },
        { fullName: 'Carlos Jiménez Vargas', legalId: '1-103-789123', position: 'Encargado de Planilla' },
        { fullName: 'Ana María Solís', legalId: '1-104-456789', position: 'Colaborador' },
        { fullName: 'Luis Fernández Mora', legalId: '1-105-321654', position: 'Supervisor' },
        { fullName: 'Sofía Castro Rojas', legalId: '1-106-987654', position: 'Encargado de Planilla' },
        { fullName: 'Pedro Ramírez Soto', legalId: '1-107-123789', position: 'Colaborador' },
        { fullName: 'Laura Vargas Chacón', legalId: '1-108-654987', position: 'Supervisor' },
        { fullName: 'Andrés Mora Paniagua', legalId: '1-109-789456', position: 'Encargado de Planilla' },
        { fullName: 'Gabriela Sánchez Quesada', legalId: '1-110-456123', position: 'Colaborador' },
        { fullName: 'Diego López Herrera', legalId: '1-111-321987', position: 'Supervisor' },
        { fullName: 'Carolina Méndez Salazar', legalId: '1-112-987321', position: 'Encargado de Planilla' },
        { fullName: 'Jorge Castro Ureña', legalId: '1-113-123654', position: 'Colaborador' },
        { fullName: 'Natalia Rojas Vargas', legalId: '1-114-654123', position: 'Supervisor' },
        { fullName: 'Ricardo Gómez Solano', legalId: '1-115-789321', position: 'Encargado de Planilla' },
        { fullName: 'Isabel Fernández Mora', legalId: '1-116-456987', position: 'Colaborador' },
        { fullName: 'Manuel Jiménez Soto', legalId: '1-117-321456', position: 'Supervisor' },
        { fullName: 'Paula Vargas Chacón', legalId: '1-118-987654', position: 'Encargado de Planilla' },
        { fullName: 'Esteban Ramírez Quesada', legalId: '1-119-123987', position: 'Colaborador' },
        { fullName: 'Daniela Solís Herrera', legalId: '1-120-654789', position: 'Supervisor' },
        { fullName: 'José Pérez González', legalId: '1-121-789654', position: 'Encargado de Planilla' },
        { fullName: 'Mariana Rodríguez López', legalId: '1-122-456321', position: 'Colaborador' },
        { fullName: 'Felipe Jiménez Vargas', legalId: '1-123-321789', position: 'Supervisor' },
        { fullName: 'Camila Solís Rojas', legalId: '1-124-987123', position: 'Encargado de Planilla' },
        { fullName: 'Oscar Fernández Mora', legalId: '1-125-123654', position: 'Colaborador' }
      ]
    };
  }
};
</script>

<style scoped>
.card-title {
  font-size: 1.75rem;
  font-weight: bold;
}

.sticky-header th {
  position: sticky;
  top: 0;
  z-index: 2;
  background-color: #343a40; /* Matches the table-dark background */
  color: white;
}
</style>