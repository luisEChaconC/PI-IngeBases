<template>
    <div class="d-flex justify-content-center align-items-center min-vh-100 my-5">
    <div class="card w-50">
        <div class="card-body">
        <h2 class="card-title">Nuevo empleado</h2>
        <h5 class="card-subtitle mb-4">Ingrese la información de la persona</h5>
        <div class="row">
            <div class="col-md-6">
            <div class="mb-3">
                <label for="firstName" class="form-label">Nombre</label>
                <input type="text" class="form-control" id="firstName" />
            </div>
            <div class="mb-3">
                <label for="secondLastName" class="form-label">Segundo Apellido</label>
                <input type="text" class="form-control" id="secondLastName" />
            </div>
            <div class="mb-3">
                <label for="workerId" class="form-label">Id Trabajador</label>
                <input type="text" class="form-control" id="workerId" />
            </div>
            <div class="mb-3">
                <label for="contractType" class="form-label">Tipo Contrato</label>
                <select class="form-select" id="contractType">
                <option value="">Seleccione una opción</option>
                <option value="PartTime">Media Jornada</option>
                <option value="FullTime">Jornada Completa</option>
                <option value="ProfessionalServices">Servicios Profesionales</option>
                </select>
            </div>
            <div class="mb-3">
                <label for="email" class="form-label">Correo</label>
                <input type="email" class="form-control" id="email" />
            </div>
            <div class="mb-3">
                <label for="password" class="form-label">Contraseña</label>
                <input type="password" class="form-control" id="password" />
            </div>
            </div>
            <div class="col-md-6">
            <div class="mb-3">
                <label for="firstLastName" class="form-label">Primer Apellido</label>
                <input type="text" class="form-control" id="firstLastName" />
            </div>
            <div class="mb-3">
                <label for="identityCard" class="form-label">Cédula de identidad</label>
                <input type="text" class="form-control" id="identityCard" />
            </div>
            <div class="mb-3">
                <label for="role" class="form-label">Rol</label>
                <select class="form-select" id="role">
                <option value="">Seleccione una opción</option>
                <option value="Supervisor">Supervisor</option>
                <option value="PayrollManager">Encargado Planilla</option>
                <option value="Collaborator">Colaborador</option>
                </select>
            </div>
            <div class="mb-3">
                <label for="grossSalary" class="form-label">Salario Bruto</label>
                <input type="number" class="form-control" id="grossSalary" />
            </div>
            <div class="mb-3">
                <label for="phone" class="form-label">Teléfono</label>
                <input type="tel" class="form-control" id="phone" />
            </div>
            </div>
        </div>
        <div class="d-flex justify-content-start mt-3">
            <button type="button" class="btn btn-dark me-2">Agregar empleado</button>
            <button type="button" class="btn btn-outline-dark">Cancelar</button>
        </div>
        </div>
    </div>
    </div>
</template>

<script>
    export default {
    name: 'AddEmployee'
    }
</script>

<style scoped>
  .card-subtitle {
    color: gray;
  }
</style>