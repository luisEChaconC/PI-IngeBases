<template>
  <!-- Main container for the login form, styled to center content vertically and horizontally -->
  <div class="container vh-100 d-flex flex-column align-items-center justify-content-center">
    <!-- Card container to hold the login form -->
    <div class="card w-100" style="max-width: 400px">
      <div class="card-body text-center text-md-start">
        <!-- Title of the login form -->
        <h2 class="card-title mb-4">Iniciar Sesión</h2>
        <!-- Form element with a submit handler to call the handleLogin method -->
        <form @submit.prevent="handleLogin">
          <!-- Error message container, displayed only if errorMessage is not empty -->
          <div 
            v-if="errorMessage" 
            class="alert alert-danger my-3 p-3"
          >
            <!-- Display the error message dynamically -->
            {{ errorMessage }}
          </div>
          <!-- Email Input Section -->
          <div class="mb-3">
            <!-- Label for the email input field -->
            <label for="email" class="form-label">Correo electrónico</label>
            <!-- Email input field bound to the 'email' data property -->
            <input
              id="email"
              v-model="email"
              type="email"
              required
              class="form-control"
              :class="{ 'is-invalid': errorMessage }"
            />
            <!-- Adds 'is-invalid' class if there is an error -->
          </div>

          <!-- Password Input Section -->
          <div class="mb-3">
            <!-- Label for the password input field -->
            <label for="password" class="form-label">Contraseña</label>
            <!-- Password input field bound to the 'password' data property -->
            <input
              id="password"
              v-model="password"
              type="password"
              required
              class="form-control"
              :class="{ 'is-invalid': errorMessage }"
            />
            <!-- Adds 'is-invalid' class if there is an error -->
          </div>

          <!-- Submit Button Section -->
          <div class="row justify-content-center justify-content-md-start mb-3">
            <div class="col-12 col-md-6">
              <!-- Submit button to trigger the form submission -->
              <button type="submit" class="btn btn-dark w-100">
                Iniciar sesión
              </button>
            </div>
          </div>
        </form>
      </div>
    </div>
    <!-- Section for registration link -->
    <div class="mt-3 d-flex align-items-center justify-content-center justify-content-md-start">
      <span class="me-2">¿Aún no es cliente?</span>
      <!-- Button to navigate to the registration page -->
      <button @click="handleRegister" class="btn btn-outline-secondary">
        Registrar Empresa
      </button>
    </div>
  </div>
</template>

<script>
import userService from "@/services/userService";
import companyService from "@/services/companyService";
import currentUserService from "@/services/currentUserService";
import Swal from 'sweetalert2'; 

export default {
  name: "LoginForm", // Component name
  data() {
    return {
      email: "", // Holds the email input value
      password: "", // Holds the password input value
      errorMessage: "" // Holds the error message to display in the UI
    };
  },
  methods: {
    async handleLogin() {
      this.errorMessage = "";

      try {
        await userService.login(this.email, this.password);

        const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage();
        const companyId = currentUserInformation.companyId;
        
        let company = null;
        
        if(companyId) {
          company = await companyService.getCompanyById(companyId);
        }

        if (((company && company.isDeleted) || companyId === null) && currentUserInformation.position != "SoftwareManager") {
          await Swal.fire({
            icon: 'error',
            title: 'Empresa eliminada',
            text: 'Su empresa ha sido eliminada del sistema. Por favor, contacte con el equipo de soporte.',
            confirmButtonText: 'Entendido',
            confirmButtonColor: '#d33'
          });
          return;
        }

        this.$router.push({ name: "HomeView" });
      } catch (error) {
        this.errorMessage = error.message;
      }
    },
    /**
     * Handles the registration process when the "Registrar Empresa" button is clicked.
     * Simulates navigation to the registration page.
     */
    handleRegister() {
      this.$router.push('/company-registration');
    }
  }
};
</script>

<style scoped>
/* Scoped styles to apply only to this component */
</style>