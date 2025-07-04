<template>
  <div class="position-relative">
    <button
      class="btn btn-outline-secondary position-absolute top-0 start-0 mt-2 ms-3"
      title="Volver"
      @click="handleBack"
    >
      ← Volver
    </button>
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
                    
                  </div>

                 <!-- Cédula Jurídica -->
                <div class="col-md-6 mb-3 position-relative">
                  <label for="legalId" class="form-label">Cédula Jurídica</label>
                  <input type="text" class="form-control pe-5" id="legalId" v-model="company.person.legalId" @input="isEditingLegalId = true" :disabled="!isEditing || hasPayroll" placeholder="X-XXX-XXXXXX"/>
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

                    
                  </div>

                  <!-- Correo -->
                <div class="col-md-6 mb-3 position-relative">
                  <label for="email" class="form-label">Correo</label>
                  <input type="email" class="form-control pe-5" id="email" v-model="company.contact.email" @input="isEditingEmail = true" :disabled="!isEditing" placeholder="nombre@ejemplo.com"/>
                </div>

                <!-- Teléfono -->
                <div class="col-md-6 mb-3 position-relative">
                  <label for="phoneNumber" class="form-label">Teléfono</label>
                  <input type="tel" class="form-control pe-5" id="phoneNumber" v-model="company.contact.phoneNumber" @input="isEditingPhone = true" :disabled="!isEditing" placeholder="8888-8888"/>
                </div>

                  <!-- Cantidad máxima beneficios -->
                  <div class="col-md-6 mb-3 position-relative">
                    <label for="maxBenefits" class="form-label">Cantidad máxima de beneficios por empleado</label>
                    <input type="tel" class="form-control pe-5" id="maxBenefits" v-model="company.maxBenefits" :disabled="!isEditing" />
                    
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
      hasPayroll: false,
      isEditingEmail: false,
      isEditingPhone: false,
      isEditingLegalId: false,
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
      originalCompany: {},
    };
  },

  methods: {
    async getCompanyById(companyId) {
      try {
 
        const companyResponse = await axios.get(`https://localhost:5000/api/Company/GetCompanyById/${companyId}`);
        this.company = companyResponse.data;
        this.company.employeesCount = companyResponse.data.employeesDynamic.length;
        
        const contactsList = companyResponse.data.contact; 

        this.company.contact = {
          phoneNumber: contactsList.find(c => c.phoneNumber)?.phoneNumber || "",
          email: contactsList.find(c => c.email)?.email || ""
        };

        this.company.maxBenefits = companyResponse.data.maxBenefitsPerEmployee;

        try {
          // verifica si la empresa tiene planilla
          const payrollResponse = await axios.get(`https://localhost:5000/api/Payroll/company/${companyId}`);
          this.hasPayroll = payrollResponse.data && payrollResponse.data.length > 0;
        } catch (payrollError) {
       
          this.hasPayroll = false;
        }
      } catch (error) {
        console.error("Error retrieving company:", error);
      }
    },

    toggleEdit() {
      if (this.isEditing) {
        this.updateCompany(); 
      } else {
        // Guardamos una copia del objeto original
        this.originalCompany = JSON.parse(JSON.stringify(this.company));
      }

      this.isEditing = !this.isEditing;
    }, 

   updateCompany() {
 
      if (this.isEditingEmail && !this.emailHasValidFormat()) {
        alert("El correo no tiene un formato válido.");
        return;
      }

   
      if (this.isEditingPhone && !this.phoneNumberHasValidFormat()) {
        alert("El teléfono debe tener el formato ####-####.");
        return;
      }

      if (this.isEditingLegalId && !this.legalEntityIdHasAValidFormat() && !this.hasPayroll) {
        alert("La cédula jurídica debe tener el formato #-###-######.");
        return;
      }

      const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage();

      const companyId = currentUserInformation.position?.trim() === "SoftwareManager"
        ? localStorage.getItem("selectedCompanyId")
        : currentUserInformation.companyId;

      this.company.id = companyId;
      this.company.person.legalId = this.company.person.legalId.replace(/-/g, '');
      this.company.contact.phoneNumber = this.company.contact.phoneNumber.replace(/-/g, '');
      axios.put(`https://localhost:5000/api/Company/${companyId}`, this.company)
        .then(() => {
          alert("Company updated successfully");
        })
        .catch((error) => {
          if (error.response && error.response.status === 409) {
            alert(error.response.data.message || "There's a register with this data already.");
          } else {
            alert("An error occurred while updating the company.");
          }
          console.error("Data sent:", this.company);
          console.error("Error while updating company", error.response?.data);
        });
    },


    handleBack() {
      const raw = localStorage.getItem("currentUserInformation");
      const user = raw ? JSON.parse(raw) : null;

      if (user?.position?.trim() === "SoftwareManager") {
        this.$router.push("/view-companies-list");
      } else {
        this.$router.push("/home-view");
      }
    },

      cancelEdit() {
        this.company = JSON.parse(JSON.stringify(this.originalCompany));
        this.isEditing = false;
        this.isEditingEmail = false;
        this.isEditingPhone = false;
        this.isEditingLegalId = false;
      },
      phoneNumberHasValidFormat() {
        const validFormat = /^[0-9]{4}-[0-9]{4}$/;
        return validFormat.test(this.company.contact.phoneNumber);
      },

      emailHasValidFormat() {
        const validFormat = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return validFormat.test(this.company.contact.email);
      },

      legalEntityIdHasAValidFormat() {
        const validFormat = /^\d-\d{3}-\d{6}$/;
        return validFormat.test(this.company.person.legalId);
      }
  },



  created() {
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