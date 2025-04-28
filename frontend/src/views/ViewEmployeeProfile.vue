<template>
    <div class="d-flex justify-content-center align-items-center min-vh-100 my-5">
      <div class="card w-50">
        <div class="card-body">
          <h2 class="card-title">Perfil de Empleado</h2>
          <div class="row">
            <div class="col-md-6">
              <div class="mb-3">
                <label for="firstName" class="form-label">Nombre</label>
                <input type="text" class="form-control" id="firstName" v-model="employee.firstName" disabled />
              </div>
              <div class="mb-3">
                <label for="secondLastName" class="form-label">Segundo apellido</label>
                <input type="text" class="form-control" id="secondLastName" v-model="employee.secondLastName" disabled />
              </div>
              <div class="mb-3">
                <label for="workerId" class="form-label">ID Trabajador</label>
                <input type="text" class="form-control" id="workerId" v-model="employee.workerId" disabled />
              </div>
              <div class="mb-3">
                <label for="contractType" class="form-label">Tipo de Contrato</label>
              <input type="text" class="form-control" id="contractType" v-model="employee.contractType" disabled />
            </div>
              <div class="mb-3">
                <label for="email" class="form-label">Email</label>
                <input type="email" class="form-control" id="email" v-model="employee.email" disabled />
              </div>
              <div class="mb-3">
                <label for="password" class="form-label">Contraseña</label>
                <input type="password" class="form-control" id="password" v-model="employee.password" disabled />
              </div>
            </div>
            <div class="col-md-6">
              <div class="mb-3">
                <label for="firstLastName" class="form-label">Primer apellido</label>
                <input type="text" class="form-control" id="firstLastName" v-model="employee.firstLastName" disabled />
              </div>
              <div class="mb-3">
                <label for="identityCard" class="form-label">Cédula</label>
                <input type="text" class="form-control" id="identityCard" v-model="employee.identityCard" disabled />
              </div>
              <div class="mb-3">
                <label for="role" class="form-label">Rol</label>
              <input type="text" class="form-control" id="role" v-model="employee.role" disabled />
            </div>
              <div class="mb-3">
                <label for="grossSalary" class="form-label">Salario Bruto</label>
                <div class="input-group">
                <span class="input-group-text">₡</span>
                <input type="number" class="form-control" id="grossSalary" v-model="employee.grossSalary" disabled />
                </div>
              </div>
              <div class="mb-3">
                <label for="phone" class="form-label">Número de Teléfono</label>
                <input type="tel" class="form-control" id="phone" v-model="employee.phone" disabled />
              </div>
            </div>
          </div>
          <div class="d-flex justify-content-start mt-3">
            <button type="button" class="btn btn-dark">Edit</button>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    name: 'ViewEmployeeProfile',
    data() {
      return {
        employee: {
          firstName: 'Juan',
          firstLastName: 'Molina',
          secondLastName: 'Rojas',
          identityCard: '1-1186-3294',
          workerId: '1B50#47',
          role: 'Supervisor',
          contractType: 'Tiempo completo',
          grossSalary: 1275600,
          email: 'juan.moliroj@gmail.com',
          phone: '8672-6138',
          password: '********'
        }
      }
    }
  }
  </script>
  
  <style scoped>
  .card-title {
    margin-bottom: 1.5rem;
  }
  </style>