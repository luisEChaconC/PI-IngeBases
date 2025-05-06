<template>
    <div class="d-flex justify-content-center align-items-center min-vh-100 my-5 my-md-0">        
        <div class="card w-50">
            <div class="card-body">
                <h2 class="card-title">Nuevo empleado</h2>
                <h5 class="card-subtitle mb-4">Ingrese la información de la persona</h5>
                <div class="row">
                    <div class="col-md-6">
                        <div class="mb-3">
                            <label for="firstName" class="form-label">Nombre</label>
                            <input type="text" class="form-control" id="firstName" v-model="formData.naturalPerson.firstName" placeholder="Juan" required />
                        </div>
                        <div class="mb-3">
                            <label for="secondSurname" class="form-label">Segundo Apellido</label>
                            <input type="text" class="form-control" id="secondSurname" v-model="formData.naturalPerson.secondSurname" placeholder="Pérez" required />
                        </div>
                        <div class="mb-3">
                            <label for="workerId" class="form-label">Id Trabajador</label>
                            <input type="text" class="form-control" id="workerId" v-model="formData.employee.workerId" placeholder="EMP-12345" required />
                        </div>
                        <div class="mb-3">
                            <label for="contractType" class="form-label">Tipo Contrato</label>
                            <select class="form-select" id="contractType" v-model="formData.employee.contractType" required>
                                <option value="">Seleccione una opción</option>
                                <option value="Part-Time">Media Jornada</option>
                                <option value="Full-Time">Jornada Completa</option>
                                <option value="Professional Services">Servicios Profesionales</option>
                                <option value="Hourly">Por hora</option>
                            </select>
                        </div>
                        <div class="mb-3">
                            <label for="email" class="form-label">Correo</label>
                            <input type="email" class="form-control" id="email" v-model="formData.user.email" placeholder="correo@dominio.com" required />
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="mb-3">
                            <label for="firstSurname" class="form-label">Primer Apellido</label>
                            <input type="text" class="form-control" id="firstSurname" v-model="formData.naturalPerson.firstSurname" placeholder="Rodríguez" required />
                        </div>
                        <div class="mb-3">
                            <label for="legalId" class="form-label">Cédula de identidad</label>
                            <input type="text" class="form-control" id="legalId" v-model="formData.person.legalId" placeholder="1-2345-6789" required />
                        </div>
                        <div class="mb-3">
                            <label for="role" class="form-label">Rol</label>
                            <select class="form-select" id="role" v-model="formData.employeeRole" required>
                                <option value="">Seleccione una opción</option>
                                <option value="Supervisor">Supervisor</option>
                                <option value="Payroll Manager">Encargado Planilla</option>
                                <option value="Collaborator">Colaborador</option>
                            </select>
                        </div>
                        <div class="mb-3">
                            <label for="grossSalary" class="form-label">Salario Bruto (Colones)</label>
                            <div class="input-group">
                                <span class="input-group-text">₡</span>
                                <input type="number" class="form-control" id="grossSalary" v-model="formData.employee.grossSalary" placeholder="500000" required />
                            </div>                        
                        </div>
                        <div class="mb-3">
                            <label for="phone" class="form-label">Teléfono</label>
                            <input type="tel" class="form-control" id="phone" v-model="formData.contact.phoneNumber" placeholder="8888-8888" required />
                        </div>
                    </div>
                </div>
                <div class="d-flex justify-content-start mt-3">
                    <button type="button" class="btn btn-dark me-2" @click="submitForm">Agregar</button>
                    <a href="#" class="btn btn-outline-dark">Cancelar</a>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import axios from "axios";
import currentUserService from "@/services/currentUserService";

export default {
    name: "AddEmployee",
    data() {
        return {
            formData: {
                person: {
                    legalId: "",
                    type: "Natural Person",
                    province: "",
                    canton: "",
                    neighborhood: "",
                    additionalDirectionDetails: "",
                },
                user: {
                    email: "",
                    password: "",
                    isAdmin: false,
                },
                naturalPerson: {
                    firstName: "",
                    firstSurname: "",
                    secondSurname: "",
                },
                contact: {
                    type: "Phone Number",
                    phoneNumber: "",
                },
                employee: {
                    workerId: "",
                    companyId: "",
                    contractType: "",
                    grossSalary: 0,
                    hasToReportHours: false,
                },
                employeeRole: "",
            },
        };
    },
    methods: {
        async submitForm() {
            // Dynamically set the password before submitting
            // The user password is the first surname and the last 3 digits of the legal id
            this.formData.user.password = `${this.formData.naturalPerson.firstSurname}${this.formData.person.legalId.slice(-3)}`
            const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage()
            this.formData.employee.companyId = currentUserInformation.companyId
            try {
                const response = await axios.post(
                    "https://localhost:5000/api/Employee/CreateEmployeeWithDependencies",
                    this.formData
                );
                alert("Empleado agregado exitosamente")
                console.log(response.data)
            } catch (error) {
                alert("Ocurrió un error al tratar de agregar el empleado.")
            }
        },
    },
};
</script>

<style scoped>
.card-subtitle {
    color: gray;
}
</style>