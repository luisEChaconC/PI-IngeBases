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
                <input type="text" class="form-control" id="address" v-model="company.address" disabled />
              </div>
              <div class="mb-3">
                <label for="paymentType" class="form-label">Tipo de pago</label>
                <input type="email" class="form-control" id="paymentType" v-model="company.paymentType" disabled />
              </div>
              <div class="mb-3">
                <label for="phoneNumber" class="form-label">Teléfono</label>
                <input type="tel" class="form-control" id="phoneNumber" v-model="company.phoneNumber" disabled />
              </div>
    
            </div>
  
            <!-- Right column elements  -->
            <div class="col-md-5 ms-md-4">
              <div class="mb-3">
                <label for="legalId" class="form-label">Cédula Jurídica</label>
                <input type="text" class="form-control" id="legalId" v-model="company.legalId" disabled />
              </div>
              <div class="mb-3">
                <label for="employeesCount" class="form-label">Cantidad de empleados</label>
                <input type="int" class="form-control" id="employeesCount" v-model="company.employeesCount" disabled />
              </div>
              <div class="mb-3">
                <label for="email" class="form-label">Correo</label>
                <input type="text" class="form-control" id="email" v-model="company.email" disabled />
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
  export default {
    name: 'ViewCompanyInfo',
    data() {
      return {
        company: {
          name: 'Blitz S.A.',
          address: 'San Pedro, San José',
          phoneNumber: '8886-4575',
          paymentType: 'mensual',
          legalId: '112345674',
          employeesCount: 50,
          email: 'blitz@company.com'
        }
      };
    }
  };
  </script>

<style scoped>

.card {
  border-radius: 10px;
  max-width: 30%;
}
.card-title {
  font-size: 30px;
  font-weight: bold;
  margin-bottom: 65px;
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
  background-color: #fff;
  color: #000;           
  opacity: 1;          
}

</style> 