<template>
  <div class="container vh-100 d-flex flex-column align-items-center justify-content-center">
    <div class="card shadow-lg border-0 w-100" style="max-width: 420px; background: #fff;">
      <div class="card-body text-center">
        <div class="mb-3">
          <span class="display-1 text-danger">
            <i class="fas fa-ban"></i>
          </span>
        </div>
        <h2 class="mb-3 text-danger fw-bold">Acceso no autorizado</h2>
        <p class="mb-4 text-dark">
          Usted no tiene permisos para ver esta página.<br>
          Si considera que esto es un error, por favor contacte al administrador.
        </p>
        <router-link to="/login" class="btn btn-dark btn-lg w-100">
          Volver
        </router-link>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'UnauthorizedView'
}
</script>

<style scoped>
.card {
  border-radius: 1rem;
}
.display-1 {
  font-size: 3.5rem;
}
</style>