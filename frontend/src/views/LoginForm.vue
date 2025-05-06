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
import axios from "axios"; // Importing Axios for making HTTP requests
import currentUserService from "@/services/currentUserService";

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
    /**
     * Handles the login process when the form is submitted.
     * Makes an API call to validate the user's credentials.
     */
    async handleLogin() {
      // Reset the error message before starting the login process
      this.errorMessage = "";

      try {
        // Make a GET request to the backend to fetch user data by email
        const response = await axios.get("https://localhost:5000/api/User/GetUserByEmail", {
          params: { email: this.email } // Pass the email as a query parameter
        });

        // Check if the response status is 200 (OK)
        if (response.status === 200) {
          const user = response.data; // Extract user data from the response

          // Validate the password entered by the user
          if (this.password !== user.password) {
            // Set an error message if the password is incorrect
            this.errorMessage = "Correo electrónico o contraseña incorrectos.";
            return; // Exit the function
          }

          // Use the global service instance to fetch and save user information
          await currentUserService.fetchAndSaveCurrentUserInformationToLocalStorage(this.email);

          // Simulate a successful login and redirect to the main menu
          alert("Se inició sesión con éxito! Redirigiendo al menú principal...");
          console.log("Login exitoso → redirigiendo al menú principal");
          // Uncomment the following line to enable navigation
          // this.$router.push({ name: "MainMenu" });
        }
      } catch (error) {
        // Handle errors that occur during the API call
        if (error.response) {
          // Handle specific HTTP status codes
          if (error.response.status === 404) {
            // User not found
            this.errorMessage = "Correo electrónico o contraseña incorrectos.";
          } else if (error.response.status === 500) {
            // Server error
            this.errorMessage = "Ocurrió un error en el servidor. Inténtelo más tarde.";
          }
        } else {
          // Handle unexpected errors (e.g., network issues)
          this.errorMessage = "No se pudo conectar con el servidor.";
        }
      }
    },
    /**
     * Handles the registration process when the "Registrar Empresa" button is clicked.
     * Simulates navigation to the registration page.
     */
    handleRegister() {
      alert("Se debería reenviar a otra vista!"); // Simulate navigation
      console.log("Navegar a Registro de Usuario → luego Registro de Empresa");
      // Uncomment the following line to enable navigation
      // this.$router.push({ name: "UserRegister" });
    }
  }
};
</script>

<style scoped>
/* Scoped styles to apply only to this component */
</style>