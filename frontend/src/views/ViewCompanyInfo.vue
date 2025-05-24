<template>
  <div>
    <!-- Barra superior fija con botón de regreso -->
    <div class="top-bar">
      <router-link
        to="/main-menu"
        class="btn btn-outline-secondary"
        title="Volver al menú principal"
      >
        ← Volver
      </router-link>
    </div>

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
                  <div class="col-md-6 mb-3 position-relative">
                    <label for="name" class="form-label">Nombre</label>
                    <input type="text" class="form-control pe-5" id="name" v-model="company.name" :disabled="!isEditing" />
                    <i v-if="isEditing" class="bi bi-pencil-fill text-dark position-absolute" style="top: 40px; right: 17px;"></i>
                  </div>

                  <!-- Cédula Jurídica -->
                  <div class="col-md-6 mb-3 position-relative">
                    <label for="legalId" class="form-label">Cédula Jurídica</label>
                    <input type="text" class="form-control pe-5" id="legalId" v-model="company.person.legalId" :disabled="!isEditing" />
                    <i v-if="isEditing" class="bi bi-pencil-fill text-dark position-absolute" style="top: 40px; right: 17px;"></i>
                  </div>

                  <!-- Dirección -->
                  <div class="col-12 mb-3 position-relative">
                    <label for="address" class="form-label">Dirección</label>
                    <input type="text" class="form-control pe-5" id="address" :value="fullAddress" :disabled="!isEditing" />
                    <i v-if="isEditing" class="bi bi-pencil-fill text-dark position-absolute" style="top: 40px; right: 17px;"></i>
                  </div>
                  
                  <!-- Tercera fila -->
                  <div class="col-md-6 mb-3">
                    <label for="employeesCount" class="form-label">Cantidad empleados</label>
                    <input type="number" class="form-control" id="employeesCount" v-model="company.employeesCount" disabled />
                  </div>

                  <!-- Tipo de Pago -->
                  <div class="col-md-6 mb-3 position-relative">
                    <label for="paymentType" class="form-label">Tipo de Pago</label>
                 
                    <!-- Modo edición -->
                    <select v-if="isEditing" class="form-select pe-5 no-arrow" id="paymentType" v-model="company.paymentType">
                      <option disabled value="">Seleccione...</option>
                      <option v-if="company.paymentType && !['Monthly', 'Biweekly', 'Weekly'].includes(company.paymentType)"  :value="company.paymentType">
                        {{ translatedPaymentType }}
                      </option>
                      <option value="Monthly">Mensual</option>
                      <option value="Weekly">Semanal</option>
                      <option value="Biweekly">Quincenal</option>
                    </select>

                    <!-- Modo lectura -->
                    <select v-else class="form-select pe-5" id="paymentType" disabled>
                      <option>{{ translatedPaymentType }}</option>
                    </select>

                    <i v-if="isEditing" class="bi bi-pencil-fill text-dark position-absolute" style="top: 40px; right: 17px;"></i>
                  </div>

                  <!-- Correo -->
                  <div class="col-md-6 mb-3 position-relative">
                    <label for="email" class="form-label">Correo</label>
                    <input type="email" class="form-control pe-5" id="email" v-model="company.contact.email" :disabled="!isEditing" />
                    <i v-if="isEditing" class="bi bi-pencil-fill text-dark position-absolute" style="top: 40px; right: 17px;"></i>
                  </div>

                  <!-- Teléfono -->
                  <div class="col-md-6 mb-3 position-relative">
                    <label for="phoneNumber" class="form-label">Teléfono</label>
                    <input type="tel" class="form-control pe-5" id="phoneNumber" v-model="company.contact.phoneNumber" :disabled="!isEditing" />
                    <i v-if="isEditing" class="bi bi-pencil-fill text-dark position-absolute" style="top: 40px; right: 17px;"></i>
                  </div>
                  
                  <!-- Editar -->
                  <div class="mt-4 d-flex gap-3">
                    <button type="button" class="btn btn-dark px-4" @click="toggleEdit">
                      {{ isEditing ? 'Guardar cambios' : 'Editar' }}
                    </button>

                    <button v-if="isEditing" type="button" class="btn btn-outline-secondary px-4" @click="cancelEdit">
                      Cancelar
                    </button>
                  </div>
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
      isEditing: false,
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
    },

    toggleEdit() {
      if (this.isEditing) {
        //this.updateCompany(); 
      }
      this.isEditing = !this.isEditing;
    },
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
  top: 0;
  left: 0;
  width: 100%;
  z-index: 1000;
}

.main-content {
  padding-top: 80px;
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

.no-arrow {
  appearance: none;
  -webkit-appearance: none;
  -moz-appearance: none;
  background-image: none !important;
}

</style> 