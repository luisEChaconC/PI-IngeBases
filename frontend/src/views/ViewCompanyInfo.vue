<template>
    <div class="d-flex justify-content-center align-items-center min-vh-100 my-5">
      <div class="card w-75">
        <div class="card-body">
          <h2 class="card-title">Información empresa</h2>
          <div class="row gap-5">
            <!-- Left column elements -->
            <div class="col-md-5">
              <div class="mb-3">
                <label for="name" class="form-label">Nombre</label>
                <input type="text" class="form-control" id="name" v-model="company.name" disabled />
              </div>
              <div class="mb-3">
              <label for="address" class="form-label">Dirección</label>
              <input type="text" class="form-control" id="address" :value="fullAddress" disabled/>
            </div>
              <div class="mb-3">
                <label for="paymentType" class="form-label">Tipo de pago</label>
                <input type="email" class="form-control" id="paymentType" v-model="company.paymentType" disabled />
              </div>
              <div class="mb-3">
                <label for="phoneNumber" class="form-label">Teléfono</label>
                <input type="tel" class="form-control" id="phoneNumber" v-model="company.contact.phoneNumber" disabled />
              </div>
    
            </div>
  
            <!-- Right column elements  -->
            <div class="col-md-5 ms-md-4">
              <div class="mb-3">
                <label for="legalId" class="form-label">Cédula Jurídica</label>
                <input type="text" class="form-control" id="legalId" v-model="company.person.legalId" disabled />
              </div>
              <div class="mb-3">
                <label for="employeesCount" class="form-label">Cantidad de empleados</label>
                <input type="int" class="form-control" id="employeesCount" v-model="company.employeesCount" disabled />
              </div>
              <div class="mb-3">
                <label for="email" class="form-label">Correo</label>
                <input type="text" class="form-control" id="email" v-model="company.contact.email" disabled />
              </div>
            </div>
          </div>
  
          <div class="d-flex justify-content-center mt-3">
            <button type="button" class="btn btn-dark">Editar</button>
          </div>
        </div>
      </div>
    </div>
  </template>
  
<script>
import axios from 'axios';
//import currentUserService from "@/services/currentUserService";
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
   // const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage();
    this.getCompanyById('b4d86d82-d42c-4477-86b0-a9555fd01c38'); // Changed ID
    
    console.log('b4d86d82-d42c-4477-86b0-a9555fd01c38');
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

.card {
  border-radius: 10px;
  max-width: 30%;
}
.card-title {
  font-size: 35px;
  font-weight: bold;
  margin-bottom: 35px;
}

.btn-dark {
  padding-left: 30px;
  padding-right: 30px;
  margin-top: 30px;
}

.form-control {
  font-size: 16px;
  padding: 10px;
  margin-bottom: 20px;
}

input[disabled] {
  width: 100%;
  background-color: #f0efef;
  color: #000;           
  opacity: 1;          
}

</style> 