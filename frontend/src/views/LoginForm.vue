<template>
  <div class="container vh-100 d-flex flex-column align-items-center justify-content-center">
    <div class="card w-100" style="max-width: 400px">
      <div class="card-body text-center text-md-start">
        <h2 class="card-title mb-4">Iniciar Sesión</h2>
        <form @submit.prevent="handleLogin">
          <div 
            v-if="errorMessage" 
            class="alert alert-danger my-3 p-3"
          >
            {{ errorMessage }}
          </div>
          <!-- Email Input -->
          <div class="mb-3">
            <label for="email" class="form-label">Correo electrónico</label>
            <input
              id="email"
              v-model="email"
              type="email"
              required
              class="form-control"
              :class="{ 'is-invalid': errorMessage}"
            />
          </div>

          <!-- Password Input -->
          <div class="mb-3">
            <label for="password" class="form-label">Contraseña</label>
            <input
              id="password"
              v-model="password"
              type="password"
              required
              class="form-control"
              :class="{ 'is-invalid': errorMessage}"
            />
          </div>

          <!-- Submit Button -->
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
import axios from "axios";

export default {
  name: "LoginForm",
  data() {
    return {
      email: "",
      password: "",
      errorMessage: ""
    };
  },
  methods: {
    async handleLogin() {
      // Reset error message
      this.errorMessage = "";

      try {
        // Make a request to the backend
        const response = await axios.get("https://localhost:5000/api/User/GetUserByEmail", {
          params: { email: this.email }
        });

        if (response.status === 200) {
          const user = response.data;

          // Validate password
          if (this.password !== user.password) {
            this.errorMessage = "Correo electrónico o contraseña incorrectos.";
            return;
          }

          // Mock successful login redirection
          alert("Se inició sesión con éxito! Redirigiendo al menú principal...");
          console.log("Login exitoso → redirigiendo al menú principal");
          // this.$router.push({ name: "MainMenu" });
        }
      } catch (error) {
        if (error.response) {
          // Handle expected errors without logging them to the console
          if (error.response.status === 404) {
            this.errorMessage = "Correo electrónico o contraseña incorrectos.";
          } else if (error.response.status === 500) {
            this.errorMessage = "Ocurrió un error en el servidor. Inténtelo más tarde.";
          }
        } else {
          // Log unexpected errors (e.g., network issues)
          this.errorMessage = "No se pudo conectar con el servidor.";
        }
      }
    },
    handleRegister() {
      alert("Se debería reenviar a otra vista!");
      console.log("Navegar a Registro de Usuario → luego Registro de Empresa");
      // this.$router.push({ name: "UserRegister" });
    }
  }
};
</script>

<style scoped></style>