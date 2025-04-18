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
            <div v-if="errorEmail" class="text-danger small mt-1">
              {{ errorEmail }}
            </div>
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
            <div v-if="errorPassword" class="text-danger small mt-1">
              {{ errorPassword }}
            </div>
          </div>

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
      password: '',
      errorEmail: '',
      errorPassword: ''
    }
  },
  methods: {
    handleLogin() {
      // resetear errores
      this.errorEmail = ''
      this.errorPassword = ''

      console.log('Intentando iniciar sesión con', this.email)

      // datos simulados en DB
      const existingEmail = 'admin@gmail.com'
      const existingPassword = 'admin'

      if (this.email !== existingEmail) {
        this.errorEmail = 'Usuario no está registrado.'
        return
      }
      if (this.password !== existingPassword) {
        this.errorPassword = 'Contraseña incorrecta. Inténtelo nuevamente.'
        return
      }

      alert('Se inicio sesion con exito!')
      console.log('Login exitoso → redirigiendo al menú principal')
      // aquí iría: this.$router.push({ name: 'MainMenu' })
    },
    handleRegister() {
      alert('Se deberia reenviar a otra vista!')
      console.log('Navegar a Registro de Usuario → luego Registro de Empresa')
      // aquí iría: this.$router.push({ name: 'UserRegister' })
    }
  }
}
</script>

<style scoped></style>