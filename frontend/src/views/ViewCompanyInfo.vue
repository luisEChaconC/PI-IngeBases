<template>
  <div class="position-relative">
    <button
      class="btn btn-outline-secondary position-absolute top-0 start-0 mt-2 ms-3"
      title="Volver"
      @click="handleBack"
    >
      ← Volver
    </button>

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
                    <label class="form-label">Dirección</label>

                    <!-- Modo edición, se separan los inputs-->
                    <div v-if="isEditing" class="row g-2">
                      <div class="col-md-3">
                        <input type="text" class="form-control" placeholder="Provincia" v-model="company.person.province" />
                      </div>
                      <div class="col-md-3">
                        <input type="text" class="form-control" placeholder="Cantón" v-model="company.person.canton" />
                      </div>
                      <div class="col-md-3">
                        <input type="text" class="form-control" placeholder="Barrio" v-model="company.person.neighborhood" />
                      </div>
                      <div class="col-md-3">
                        <input type="text" class="form-control" placeholder="Detalles adicionales" v-model="company.person.additionalDirectionDetails" />
                      </div>
                    </div>

                    <!-- Modo lectura, dirección completa en una sola línea -->
                    <input v-else type="text" class="form-control pe-5" :value="fullAddress" disabled />
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

                  <!-- Cantidad máxima beneficios -->
                  <div class="col-md-6 mb-3 position-relative">
                    <label for="maxBenefits" class="form-label">Cantidad máxima de beneficios por empleado</label>
                    <input type="tel" class="form-control pe-5" id="maxBenefits" v-model="company.maxBenefits" :disabled="!isEditing" />
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
        employeesCount: 0,
        paymentType: "",
        maxBenefits: 0,
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
      },
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

          console.log(response.data);

          this.company.maxBenefits = response.data.maxBenefitsPerEmployee;
       
        })
        .catch((error) => {
          console.error("Error retrieving company:", error);
        });
    },

    toggleEdit() {
      if (this.isEditing) {
        this.updateCompany(); 
      }
      this.isEditing = !this.isEditing;
    }, 

    updateCompany() {
      const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage();

      axios.put(`https://localhost:5000/api/Company/${currentUserInformation.companyId}`, this.company)
        .then(() => {
          alert("Company updated successfully");
        })
        .catch((error) => {
          if (error.response && error.response.status === 409) {
            alert(error.response.data.message || "There's a register with this data already.");
          } else {
            alert("An error occurred while updating the company.");
          }
          console.error("Error while updating company", error);
          console.error("Data sent:", this.company);
          console.error("Error while updating company", error.response?.data);
        });
    },

    },

    handleBack() {
      const raw = localStorage.getItem("currentUserInformation");
      const user = raw ? JSON.parse(raw) : null;

      if (user?.position?.trim() === "SoftwareManager") {
        this.$router.push("/view-companies-list");
      } else {
        this.$router.push("/home-view");
      }
    }
  },

  created() {
    const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage();
    this.getCompanyById(currentUserInformation.companyId); 
  },
  let companyId = null;

  const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage();
  
  if (currentUserInformation.companyId) {
    companyId = currentUserInformation.companyId;
  } else {
    companyId = localStorage.getItem("selectedCompanyId");
  }

  if (companyId) {
    this.getCompanyById(companyId);
  } else {
    console.error("No se encontró un companyId para mostrar.");
  }
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