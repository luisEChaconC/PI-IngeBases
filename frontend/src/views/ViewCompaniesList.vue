<template>
    <div class="container my-5">
      <div class="card shadow">
        <div class="card-body">
          <h2 class="card-title mb-4">Empresas</h2>
          <div class="table-responsive" style="max-height: 600px; overflow-y: auto;">
            <table class="table table-striped table-bordered">
              <thead class="table-dark">
                <tr>
                  <th>Nombre</th>
                  <th>Dueño</th>
                  <th>Cédula Jurídica</th>
                  <th>Cantidad de empleados</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(company, index) in companies" :key="index">
                  <td>{{ company.name }}</td>
                  <td>{{ company.owner }}</td>
                  <td>{{ company.legalId }}</td>
                  <td>{{ company.employeeCount }}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    name: 'ViewCompaniesList',
    data() {
      return {
        companies: [
          { name: 'Blitz', owner: 'Andy Hernández Rojas', legalId: '3-101-456789', employeeCount: 5 },
          { name: 'Vorlagenersteller S.A.', owner: 'Rachit Diaz Vega', legalId: '3-102-987654', employeeCount: 4 },
          { name: 'GEEMS', owner: 'Gabriel Zúñiga Vega', legalId: '3-103-123456', employeeCount: 5 },
          { name: 'F.R.I.E.N.D.S', owner: 'Erick Sibaja', legalId: '3-104-654321', employeeCount: 5 },
          { name: 'Infinipay CO', owner: 'Daniel Gomez Solis', legalId: '3-105-789123', employeeCount: 4 },
          { name: 'Blitz', owner: 'Andy Hernández Rojas', legalId: '3-101-456789', employeeCount: 5 },
          { name: 'Vorlagenersteller S.A.', owner: 'Rachit Diaz Vega', legalId: '3-102-987654', employeeCount: 4 },
          { name: 'GEEMS', owner: 'Gabriel Zúñiga Vega', legalId: '3-103-123456', employeeCount: 5 },
          { name: 'F.R.I.E.N.D.S', owner: 'Erick Sibaja', legalId: '3-104-654321', employeeCount: 5 },
          { name: 'Infinipay CO', owner: 'Daniel Gomez Solis', legalId: '3-105-789123', employeeCount: 4 },
          { name: 'Blitz', owner: 'Andy Hernández Rojas', legalId: '3-101-456789', employeeCount: 5 },
          { name: 'Vorlagenersteller S.A.', owner: 'Rachit Diaz Vega', legalId: '3-102-987654', employeeCount: 4 },
          { name: 'GEEMS', owner: 'Gabriel Zúñiga Vega', legalId: '3-103-123456', employeeCount: 5 },
          { name: 'F.R.I.E.N.D.S', owner: 'Erick Sibaja', legalId: '3-104-654321', employeeCount: 5 },
          { name: 'Infinipay CO', owner: 'Daniel Gomez Solis', legalId: '3-105-789123', employeeCount: 4 },
          { name: 'Blitz', owner: 'Andy Hernández Rojas', legalId: '3-101-456789', employeeCount: 5 },
          { name: 'Vorlagenersteller S.A.', owner: 'Rachit Diaz Vega', legalId: '3-102-987654', employeeCount: 4 },
          { name: 'GEEMS', owner: 'Gabriel Zúñiga Vega', legalId: '3-103-123456', employeeCount: 5 },
          { name: 'F.R.I.E.N.D.S', owner: 'Erick Sibaja', legalId: '3-104-654321', employeeCount: 5 },
          { name: 'Infinipay CO', owner: 'Daniel Gomez Solis', legalId: '3-105-789123', employeeCount: 4 },
          { name: 'Blitz', owner: 'Andy Hernández Rojas', legalId: '3-101-456789', employeeCount: 5 },
          { name: 'Vorlagenersteller S.A.', owner: 'Rachit Diaz Vega', legalId: '3-102-987654', employeeCount: 4 },
          { name: 'GEEMS', owner: 'Gabriel Zúñiga Vega', legalId: '3-103-123456', employeeCount: 5 },
          { name: 'F.R.I.E.N.D.S', owner: 'Erick Sibaja', legalId: '3-104-654321', employeeCount: 5 },
          { name: 'Infinipay CO', owner: 'Daniel Gomez Solis', legalId: '3-105-789123', employeeCount: 4 },
          { name: 'Blitz', owner: 'Andy Hernández Rojas', legalId: '3-101-456789', employeeCount: 5 },
          { name: 'Vorlagenersteller S.A.', owner: 'Rachit Diaz Vega', legalId: '3-102-987654', employeeCount: 4 },
          { name: 'GEEMS', owner: 'Gabriel Zúñiga Vega', legalId: '3-103-123456', employeeCount: 5 },
          { name: 'F.R.I.E.N.D.S', owner: 'Erick Sibaja', legalId: '3-104-654321', employeeCount: 5 },
          { name: 'Infinipay CO', owner: 'Daniel Gomez Solis', legalId: '3-105-789123', employeeCount: 4 },
          { name: 'Blitz', owner: 'Andy Hernández Rojas', legalId: '3-101-456789', employeeCount: 5 },
          { name: 'Vorlagenersteller S.A.', owner: 'Rachit Diaz Vega', legalId: '3-102-987654', employeeCount: 4 },
          { name: 'GEEMS', owner: 'Gabriel Zúñiga Vega', legalId: '3-103-123456', employeeCount: 5 },
          { name: 'F.R.I.E.N.D.S', owner: 'Erick Sibaja', legalId: '3-104-654321', employeeCount: 5 },
          { name: 'Infinipay CO', owner: 'Daniel Gomez Solis', legalId: '3-105-789123', employeeCount: 4 },
          { name: 'Blitz', owner: 'Andy Hernández Rojas', legalId: '3-101-456789', employeeCount: 5 },
          { name: 'Vorlagenersteller S.A.', owner: 'Rachit Diaz Vega', legalId: '3-102-987654', employeeCount: 4 },
          { name: 'GEEMS', owner: 'Gabriel Zúñiga Vega', legalId: '3-103-123456', employeeCount: 5 },
          { name: 'F.R.I.E.N.D.S', owner: 'Erick Sibaja', legalId: '3-104-654321', employeeCount: 5 },
          { name: 'Infinipay CO', owner: 'Daniel Gomez Solis', legalId: '3-105-789123', employeeCount: 4 },
          { name: 'Blitz', owner: 'Andy Hernández Rojas', legalId: '3-101-456789', employeeCount: 5 },
          { name: 'Vorlagenersteller S.A.', owner: 'Rachit Diaz Vega', legalId: '3-102-987654', employeeCount: 4 },
          { name: 'GEEMS', owner: 'Gabriel Zúñiga Vega', legalId: '3-103-123456', employeeCount: 5 },
          { name: 'F.R.I.E.N.D.S', owner: 'Erick Sibaja', legalId: '3-104-654321', employeeCount: 5 },
          { name: 'Infinipay CO', owner: 'Daniel Gomez Solis', legalId: '3-105-789123', employeeCount: 4 },
          { name: 'Blitz', owner: 'Andy Hernández Rojas', legalId: '3-101-456789', employeeCount: 5 },
          { name: 'Vorlagenersteller S.A.', owner: 'Rachit Diaz Vega', legalId: '3-102-987654', employeeCount: 4 },
          { name: 'GEEMS', owner: 'Gabriel Zúñiga Vega', legalId: '3-103-123456', employeeCount: 5 },
          { name: 'F.R.I.E.N.D.S', owner: 'Erick Sibaja', legalId: '3-104-654321', employeeCount: 5 },
          { name: 'Infinipay CO', owner: 'Daniel Gomez Solis', legalId: '3-105-789123', employeeCount: 4 },
        ]
      }
    }
  }
  </script>
  
  <style scoped>
  .card-title {
    font-size: 1.75rem;
    font-weight: bold;
  }
  </style>
  