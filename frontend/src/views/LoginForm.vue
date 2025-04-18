<template>
  <div class="container vh-100 d-flex flex-column align-items-center justify-content-center">
    <div class="card w-100" style="max-width: 400px;">
      <div class="card-body text-center text-md-start">
        <h2 class="card-title mb-4">Iniciar Sesión</h2>
        <form @submit.prevent="handleLogin">
          <div class="mb-3">
            <label for="email" class="form-label">Correo electrónico</label>
            <input
              id="email"
              v-model="email"
              type="email"
              required
              placeholder="correo@ejemplo.com"
              class="form-control"
            />
          </div>
          <div class="mb-3">
            <label for="password" class="form-label">Contraseña</label>
            <input
              id="password"
              v-model="password"
              type="password"
              required
              placeholder="••••••••"
              class="form-control"
            />
          </div>

          <!-- replace the single button with a responsive row -->
          <div class="row justify-content-center justify-content-md-start mb-3">
            <div class="col-12 col-md-6">
              <button type="submit" class="btn btn-dark w-100">
                Iniciar sesión
              </button>
            </div>
          </div>

        </form>
      </div>
    </div>
    <div class="mt-3 d-flex align-items-center justify-content-center justify-content-md-start">
      <span class="me-2">¿Aún no es cliente?</span>
      <button @click="handleRegister" class="btn btn-outline-secondary">
        Registrar Empresa
      </button>
    </div>
  </div>
</template>

<script>
export default {
  name: 'LoginForm',
  data() {
    return {
      email: '',
      password: ''
    }
  },
  methods: {
    handleLogin() {
      console.log('Login con', this.email, this.password)
    },
    handleRegister() {
      console.log("Reenviar a vista de registro")
    }
  }
}
</script>
<style></style>
