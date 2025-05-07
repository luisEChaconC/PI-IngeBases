<template>
    <div class="d-flex justify-content-center align-items-center my-5">
      <router-link
        to="/main-menu"
        class="btn btn-outline-secondary position-absolute top-0 start-0 m-3"
        title="Volver al menú principal"
      >
        ← Volver
      </router-link>
      <div class="card" style="max-width: 800px; width: 100%;">
        <div class="card-body p-4">
          <h4 class="card-title fw-bold mb-4">Información empresa</h4>
          
          <div class="row g-3">
            <!-- Primera fila -->
            <div class="col-md-6 mb-3">
              <label for="name" class="form-label">Nombre</label>
              <input type="text" class="form-control" id="name" v-model="company.name" disabled />
            </div>
            <div class="col-md-6 mb-3">
              <label for="legalId" class="form-label">Cédula Jurídica</label>
              <input type="text" class="form-control" id="legalId" v-model="company.person.legalId" disabled />
            </div>
            
            <!-- Fila de dirección -->
            <div class="col-12 mb-3">
              <label for="address" class="form-label">Dirección</label>
              <input type="text" class="form-control" id="address" :value="fullAddress" disabled/>
            </div>
            
            <!-- Tercera fila -->
            <div class="col-md-6 mb-3">
              <label for="employeesCount" class="form-label">Cantidad empleados</label>
              <input type="number" class="form-control" id="employeesCount" v-model="company.employeesCount" disabled />
            </div>
            <div class="col-md-6 mb-3">
              <label for="paymentType" class="form-label">Tipo de Pago</label>
              <select class="form-select" id="paymentType" v-model="company.paymentType" disabled>
                <option value="" disabled>Seleccionar</option>
              </select>
            </div>
            
            <!-- Cuarta fila -->
            <div class="col-md-6 mb-3">
              <label for="email" class="form-label">Correo</label>
              <input type="email" class="form-control" id="email" v-model="company.contact.email" disabled />
            </div>
            <div class="col-md-6 mb-3">
              <label for="phoneNumber" class="form-label">Teléfono</label>
              <input type="tel" class="form-control" id="phoneNumber" v-model="company.contact.phoneNumber" disabled />
            </div>
          </div>
  
          <div class="mt-4">
            <button type="button" class="btn btn-dark px-4">Editar</button>
          </div>
        </div>
      </div>
    </div>
  </template>
  
<script>
import axios from 'axios';
import currentUserService from "@/services/currentUserService";
export default {
  name: 'ViewCompanyInfo',
  data() {
    return {
      company: {
        name: "",
        province: "",
        canton: "",
        neighborhood: "",
        additionalDirectionDetails: "",
        legalId: "", 
        employeesCount: 0,
        paymentType: "",
        person: {
          province: "",
          canton: "",
          neighborhood: "",
          additionalDirectionDetails: "",
          legalId: "",
        },
        contact: {
          phoneNumber: "",
          email: ""
        },
        employees: []
      }
    };
  },

  methods: {
    getCompanyById(companyId) {
      axios.get(`https://localhost:5000/api/Company/GetCompanyById/${companyId}`)
        .then((response) => {
          this.company = response.data;
          this.company.employeesCount = response.data.employeesDynamic.length;
          
      
          const contactsList = response.data.contact; 

          this.company.contact = {
            phoneNumber: contactsList.find(c => c.phoneNumber)?.phoneNumber || "",
            email: contactsList.find(c => c.email)?.email || ""
          };

          console.log("Company loaded:", this.company);
        })
        .catch((error) => {
          console.error("Error retrieving company:", error);
        });
    }
  },

  created() {
    const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage();
    this.getCompanyById(currentUserInformation.companyId); // Changed ID
  },

  computed: {
    fullAddress() {
      const { province, canton, neighborhood, additionalDirectionDetails } = this.company.person;
      return [province, canton, neighborhood, additionalDirectionDetails].filter(Boolean).join(', ');
    }
  }
};
</script>

<style scoped>
.btn-dark {
  padding: 0.5rem 2rem;
}

.form-control:disabled, .form-select:disabled {
  background-color: #f8f9fa;
  color: #212529;
  opacity: 1;
}
</style> 