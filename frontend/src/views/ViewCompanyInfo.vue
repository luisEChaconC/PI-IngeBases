<template>
  <div class="position-relative">
    <router-link
      to="/home-view"
      class="btn btn-outline-secondary position-absolute top-0 start-0 mt-2 ms-3"
      title="Volver"
    >
      ← Volver
    </router-link>

    <!-- Contenido principal con espaciado para evitar la barra superior -->
    <div class="main-content">
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-lg-10">
            <div class="card shadow">
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
                    <select class="form-select" id="paymentType" disabled>
                      <option>{{ translatedPaymentType }}</option>
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
    },
    
    translatedPaymentType() {
      const translations = {
        'Monthly': 'Mensual',
        'Biweekly': 'Quincenal', 
        'Weekly': 'Semanal'
      };
      
      return translations[this.company.paymentType] || this.company.paymentType;
    }
  }
};
</script>

<style scoped>
.top-bar {
  background-color: white;
  padding: 15px 20px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  position: fixed;
  top: 56px;
  left: 0;
  width: 100%;
  z-index: 1000;
}

.main-content {
  padding-top: 150px;
  padding-bottom: 40px;
}

.btn-dark {
  padding: 0.5rem 2rem;
}

.form-control:disabled, .form-select:disabled {
  background-color: #f8f9fa;
  color: #212529;
  opacity: 1;
}

.card.shadow {
  box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15) !important;
}

.card-title {
  font-size: 1.5rem;
  font-weight: bold;
}

</style> 