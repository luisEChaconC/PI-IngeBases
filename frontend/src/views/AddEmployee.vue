<template>
    <div class="d-flex justify-content-center align-items-center min-vh-100 my-5 my-md-0 position-relative">
        <router-link
          to="/employees-list"
          class="btn btn-outline-secondary position-absolute top-0 start-0 m-3"
          title="Volver a la lista de empleados"
        >
          ← Volver
        </router-link>        
        <div class="card w-50">
            <div class="card-body">
                <h2 class="card-title">Nuevo empleado</h2>
                <h5 class="card-subtitle mb-4">Ingrese la información de la persona</h5>
                <div class="row">
                    <div class="col-md-6">
                        <div class="mb-3">
                            <label for="firstName" class="form-label">Nombre</label>
                            <input
                                type="text"
                                class="form-control"
                                id="firstName"
                                v-model="formData.naturalPerson.firstName"
                                @blur="validateFirstName"
                                :class="{ 'is-invalid': errors.firstName }"
                                placeholder="Juan"
                                required
                            />
                            <div class="invalid-feedback" v-if="errors.firstName">
                                {{ errors.firstName }}
                            </div>
                        </div>
                        <div class="mb-3">
                            <label for="secondSurname" class="form-label">Segundo Apellido</label>
                            <input
                                type="text"
                                class="form-control"
                                id="secondSurname"
                                v-model="formData.naturalPerson.secondSurname"
                                @blur="validateSecondSurname"
                                :class="{ 'is-invalid': errors.secondSurname }"
                                placeholder="Pérez"
                                required
                            />
                            <div class="invalid-feedback" v-if="errors.secondSurname">
                                {{ errors.secondSurname }}
                            </div>
                        </div>
                        <div class="mb-3">
                            <label for="workerId" class="form-label">Id Trabajador</label>
                            <input
                                type="text"
                                class="form-control"
                                id="workerId"
                                v-model="formData.employee.workerId"
                                @blur="validateWorkerId"
                                :class="{ 'is-invalid': errors.workerId }"
                                placeholder="EMP-12345"
                                required
                            />
                            <div class="invalid-feedback" v-if="errors.workerId">
                                {{ errors.workerId }}
                            </div>
                        </div>
                        <div class="mb-3">
                            <label for="contractType" class="form-label">Tipo Contrato</label>
                            <select
                                class="form-select"
                                id="contractType"
                                v-model="formData.employee.contractType"
                                @blur="validateContractType"
                                :class="{ 'is-invalid': errors.contractType }"
                                required
                            >
                                <option value="">Seleccione una opción</option>
                                <option value="Part-Time">Media Jornada</option>
                                <option value="Full-Time">Jornada Completa</option>
                                <option value="Professional Services">Servicios Profesionales</option>
                                <option value="Hourly">Por hora</option>
                            </select>
                            <div class="invalid-feedback" v-if="errors.contractType">
                                {{ errors.contractType }}
                            </div>
                        </div>
                        <div class="mb-3">
                            <label for="email" class="form-label">Correo</label>
                            <input
                                type="email"
                                class="form-control"
                                id="email"
                                v-model="formData.user.email"
                                @blur="validateEmail"
                                :class="{ 'is-invalid': errors.email }"
                                placeholder="correo@dominio.com"
                                required
                            />
                            <div class="invalid-feedback" v-if="errors.email">
                                {{ errors.email }}
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="mb-3">
                            <label for="firstSurname" class="form-label">Primer Apellido</label>
                            <input
                                type="text"
                                class="form-control"
                                id="firstSurname"
                                v-model="formData.naturalPerson.firstSurname"
                                @blur="validateFirstSurname"
                                :class="{ 'is-invalid': errors.firstSurname }"
                                placeholder="Rodríguez"
                                required
                            />
                            <div class="invalid-feedback" v-if="errors.firstSurname">
                                {{ errors.firstSurname }}
                            </div>
                        </div>
                        <div class="mb-3">
                            <label for="legalId" class="form-label">Cédula de identidad</label>
                            <input
                                type="text"
                                class="form-control"
                                id="legalId"
                                v-model="formData.person.legalId"
                                @blur="validateLegalId"
                                :class="{ 'is-invalid': errors.legalId }"
                                placeholder="1-2345-6789"
                                required
                            />
                            <div class="invalid-feedback" v-if="errors.legalId">
                                {{ errors.legalId }}
                            </div>
                        </div>
                        <div class="mb-3">
                            <label for="role" class="form-label">Rol</label>
                            <select
                                class="form-select"
                                id="role"
                                v-model="formData.employeeRole"
                                @blur="validateEmployeeRole"
                                :class="{ 'is-invalid': errors.employeeRole }"
                                required
                            >
                                <option value="">Seleccione una opción</option>
                                <option value="Supervisor">Supervisor</option>
                                <option value="Payroll Manager">Encargado Planilla</option>
                                <option value="Collaborator">Colaborador</option>
                            </select>
                            <div class="invalid-feedback" v-if="errors.employeeRole">
                                {{ errors.employeeRole }}
                            </div>
                        </div>
                        <div class="mb-3">
                            <label for="grossSalary" class="form-label">Salario Bruto (Colones)</label>
                            <div class="input-group">
                                <span class="input-group-text">₡</span>
                                <input
                                    type="number"
                                    class="form-control"
                                    id="grossSalary"
                                    v-model="formData.employee.grossSalary"
                                    @blur="validateGrossSalary"
                                    :class="{ 'is-invalid': errors.grossSalary }"
                                    placeholder="500000"
                                    required
                                />
                                <div class="invalid-feedback" v-if="errors.grossSalary">
                                    {{ errors.grossSalary }}
                                </div>
                            </div>
                        </div>
                        <div class="mb-3">
                            <label for="phone" class="form-label">Teléfono</label>
                            <input
                                type="tel"
                                class="form-control"
                                id="phone"
                                v-model="formData.contact.phoneNumber"
                                @blur="validatePhoneNumber"
                                :class="{ 'is-invalid': errors.phoneNumber }"
                                placeholder="8888-8888"
                                required
                            />
                            <div class="invalid-feedback" v-if="errors.phoneNumber">
                                {{ errors.phoneNumber }}
                            </div>
                        </div>
                    </div>
                </div>
                <div class="d-flex justify-content-start mt-3">
                    <button type="button" class="btn btn-dark me-2" @click="submitForm">Agregar</button>
                    <a href="employees-list" class="btn btn-outline-dark">Cancelar</a>
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
                    additionalDirectionDetails: ""
                },
                user: {
                    email: "",
                    password: "",
                    isAdmin: false
                },
                naturalPerson: {
                    firstName: "",
                    firstSurname: "",
                    secondSurname: "",
                },
                contact: {
                    type:"Phone Number",
                    phoneNumber: ""                
                },
                employee: {
                    companyId: "",
                    workerId: "",
                    contractType: "",
                    grossSalary: 0,
                    hasToReportHours: false
                },
                employeeRole: "",
            },
            errors: {
                firstName: "",
                firstSurname: "",
                secondSurname: "",
                legalId: "",
                email: "",
                phoneNumber: "",
                workerId: "",
                contractType: "",
                grossSalary: "",
                employeeRole: "",
            },
        };
    },
    methods: {
        validateFirstName() {
            const validFormat = /^[a-zA-ZáéíóúñÁÉÍÓÚÑ\s]+$/;
            if (!this.formData.naturalPerson.firstName) {
                this.errors.firstName = "El nombre es requerido.";
            } else if (!validFormat.test(this.formData.naturalPerson.firstName)) {
                this.errors.firstName = "El nombre solo puede contener letras y espacios.";
            } else {
                this.errors.firstName = "";
            }
        },
        validateSecondSurname() {
            const validFormat = /^[a-zA-ZáéíóúñÁÉÍÓÚÑ\s]+$/;
            if (!this.formData.naturalPerson.secondSurname) {
                this.errors.secondSurname = "El segundo apellido es requerido.";
            } else if (!validFormat.test(this.formData.naturalPerson.secondSurname)) {
                this.errors.secondSurname = "El segundo apellido solo puede contener letras y espacios.";
            } else {
                this.errors.secondSurname = "";
            }
        },
        validateWorkerId() {
            if (!this.formData.employee.workerId) {
                this.errors.workerId = "El ID del trabajador es requerido.";
            } else {
                this.errors.workerId = "";
            }
        },
        validateContractType() {
            if (!this.formData.employee.contractType) {
                this.errors.contractType = "El tipo de contrato es requerido.";
            } else {
                this.errors.contractType = "";
            }
        },
        validateEmail() {
            const validFormat = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!this.formData.user.email) {
                this.errors.email = "El correo electrónico es requerido.";
            } else if (!validFormat.test(this.formData.user.email)) {
                this.errors.email = "El formato del correo electrónico no es válido.";
            } else {
                this.errors.email = "";
            }
        },
        validatePhoneNumber() {
            const validFormat = /^[0-9]{4}-[0-9]{4}$/;
            if (!this.formData.contact.phoneNumber) {
                this.errors.phoneNumber = "El número de teléfono es requerido.";
            } else if (!validFormat.test(this.formData.contact.phoneNumber)) {
                this.errors.phoneNumber = "El número de teléfono debe tener el formato ####-####.";
            } else {
                this.errors.phoneNumber = "";
            }
        },
        validateGrossSalary() {
            if (this.formData.employee.grossSalary <= 0) {
                this.errors.grossSalary = "El salario bruto debe ser mayor a 0.";
            } else {
                this.errors.grossSalary = "";
            }
        },
        validateEmployeeRole() {
            if (!this.formData.employeeRole) {
                this.errors.employeeRole = "El rol es requerido.";
            } else {
                this.errors.employeeRole = "";
            }
        },
        validateLegalId() {
            const validFormat = /^\d-\d{4}-\d{4}$/;
            if (!this.formData.person.legalId) {
                this.errors.legalId = "La cédula de identidad es requerida.";
            } else if (!validFormat.test(this.formData.person.legalId)) {
                this.errors.legalId = "La cédula debe seguir el formato #-####-####.";
            } else {
                this.errors.legalId = "";
            }
        },
        validateFirstSurname() {
            const validFormat = /^[a-zA-ZáéíóúñÁÉÍÓÚÑ\s]+$/;
            if (!this.formData.naturalPerson.firstSurname) {
                this.errors.firstSurname = "El primer apellido es requerido.";
            } else if (!validFormat.test(this.formData.naturalPerson.firstSurname)) {
                this.errors.firstSurname = "El primer apellido solo puede contener letras y espacios.";
            } else {
                this.errors.firstSurname = "";
            }
        },
        validateAllFields() {
            this.validateFirstName();
            this.validateSecondSurname();
            this.validateWorkerId();
            this.validateContractType();
            this.validateEmail();
            this.validatePhoneNumber();
            this.validateGrossSalary();
            this.validateEmployeeRole();
            this.validateLegalId();
            this.validateFirstSurname();
        },
        hasErrors() {
            return Object.values(this.errors).some(error => error !== "");
        },
        async submitForm() {
            this.validateAllFields();
            if (this.hasErrors()) {
                alert("Por favor, corrija los errores en el formulario.");
                return;
            }
            this.formData.user.password = `${this.formData.naturalPerson.firstSurname}${this.formData.person.legalId.slice(-3)}`;
            const currentUserInformation = currentUserService.getCurrentUserInformationFromLocalStorage();
            this.formData.employee.companyId = currentUserInformation.companyId;
            this.formData.person.legalId = this.formData.person.legalId.replace(/-/g, "");
            try {
                console.log(this.formData)
                const response = await axios.post(
                    "https://localhost:5000/api/Employee/CreateEmployeeWithDependencies",
                    this.formData
                );
                alert("Empleado agregado exitosamente");
                console.log(response.data);

                // Redirect to the employee-list view
                this.$router.push({ name: "EmployeesList" });
            } catch (error) {
                alert("Ocurrió un error al tratar de agregar el empleado.");
                console.log(error)
            }
        },
    },
};
</script>

<style scoped>
.card-subtitle {
    color: gray;
}
.is-invalid {
    border-color: #dc3545;
}
.invalid-feedback {
    display: block;
}
</style>