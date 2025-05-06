<template>
    <div class="container mt-4 mb-4">
        <form @submit.prevent="handleRegisterCompany" novalidate>
            <!-- First row -->
            <div class="row">
                <!-- Left Column -->
                <div class="col-md-6 mb-4">
                    <!-- "Información Básica" fieldset -->
                    <fieldset class="card h-100">
                        <div class="card-body d-flex flex-column">
                            <!-- This div groups the title and subtitle of the fieldset -->
                            <div class="mb-4">
                                <legend class="card-title h4 fw-bold mb-2">Información Básica</legend>
                                <h6 class="card-subtitle text-muted">Ingrese la información básica de su empresa</h6>
                            </div>
                            
                            <!-- This div expands to fill the available space -->
                            <!-- Inputs stay at the top of the div -->
                            <div class="flex-grow-1">
                                <!-- "Cédula Jurídica" input -->
                                <div class="mb-3">
                                    <label for="legalEntityId" class="form-label">Cédula Jurídica</label>
                                    <input
                                        type="text"
                                        class="form-control"
                                        :class="{ 'is-invalid': errors.legalEntityId }"
                                        v-model.trim="company.legalEntityId"
                                        @blur="validateLegalEntityId"
                                        required>
                                    <div class="invalid-feedback" v-if="errors.legalEntityId">
                                        {{ errors.legalEntityId }}
                                    </div>
                                </div>
    
                                <!-- "Nombre o Razón Social" input -->
                                <div class="mb-3">
                                    <label for="name" class="form-label">Nombre o Razón Social</label>
                                    <input
                                        type="text"
                                        class="form-control"
                                        :class="{ 'is-invalid': errors.name }"
                                        id="name"
                                        v-model.trim="company.name"
                                        required
                                        @blur="validateName">
                                    <div class="invalid-feedback" v-if="errors.name">
                                        {{ errors.name }}
                                    </div>
                                </div>

                                <!-- "Descripción" textarea -->
                                <div class="mb-3">
                                    <label for="description" class="form-label">Descripción</label>
                                    <textarea
                                        class="form-control"
                                        :class="{'is-invalid': errors.description}"
                                        id="description"
                                        v-model.trim="company.description"
                                        @blur="validateDescription"
                                        rows="4"
                                        required>
                                    </textarea>
                                    <div class="invalid-feedback" v-if="errors.description">
                                        {{ errors.description }}
                                    </div>
                                </div>

                                <!-- "Tipo de Pago" select -->
                                <div class="mb-3">
                                    <label for="paymentType" class="form-label">Tipo de Pago</label>
                                    <select
                                        class="form-select"
                                        :class="{ 'is-invalid': errors.paymentType }"
                                        id="paymentType"
                                        v-model="company.paymentType"
                                        required
                                        @blur="validatePaymentType">
                                        <option value="" hidden></option>
                                        <option value="monthly">Mensual</option>
                                        <option value="biweekly">Quincenal</option>
                                        <option value="weekly">Semanal</option>
                                    </select>
                                    <div class="invalid-feedback" v-if="errors.paymentType">
                                        {{ errors.paymentType }}
                                    </div>
                                </div>

                                <!-- "Cantidad Máxima de Beneficios" input -->
                                <div class="mb-4">
                                    <label for="maxBenefits" class="form-label">Cantidad Máxima de Beneficios</label>
                                    <input 
                                        type="number" 
                                        class="form-control" 
                                        :class="{ 'is-invalid': errors.maxBenefits }"
                                        id="maxBenefits" 
                                        v-model="company.maxBenefits" 
                                        @input="validateMaxBenefits"
                                        @blur="validateMaxBenefits"
                                        min="0"
                                        max="10"
                                        step="1"
                                        required>
                                    <div class="form-text text-muted">Cantidad máxima de beneficios que un empleado puede escoger</div>
                                    <div class="invalid-feedback" v-if="errors.maxBenefits">
                                        {{ errors.maxBenefits }}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </div>

                <!-- Right Column -->
                <div class="col-md-6 mb-4 d-flex flex-column">
                    <!-- "Dirección" fieldset -->
                    <fieldset class="card mb-4">
                        <div class="card-body d-flex flex-column">
                            <!-- This div groups the title and subtitle of the fieldset -->
                            <div class="mb-4">
                                <legend class="card-title h4 fw-bold mb-2">Dirección</legend>
                                <h6 class="card-subtitle text-muted">Ingrese la dirección de su empresa</h6>
                            </div>

                            <!-- first row -->
                            <div class="row mb-3">
                                <!-- "Provincia" input -->
                                <div class="col-md-6">
                                    <label for="province" class="form-label">Provincia</label>
                                    <input 
                                        type="text" 
                                        class="form-control" 
                                        :class="{ 'is-invalid': errors.direction.province }"
                                        id="province" 
                                        v-model.trim="company.direction.province" 
                                        @blur="validateProvince"
                                        required>
                                    <div class="invalid-feedback" v-if="errors.direction.province">
                                        {{ errors.direction.province }}
                                    </div>
                                </div>

                                <!-- "Cantón" input -->
                                <div class="col-md-6">
                                    <label for="canton" class="form-label">Cantón</label>
                                    <input 
                                        type="text" 
                                        class="form-control" 
                                        :class="{ 'is-invalid': errors.direction.canton }"
                                        id="canton" 
                                        v-model.trim="company.direction.canton" 
                                        @blur="validateCanton"
                                        required>
                                    <div class="invalid-feedback" v-if="errors.direction.canton">
                                        {{ errors.direction.canton }}
                                    </div>
                                </div>
                            </div>

                            <!-- second row -->
                            <div class="row mb-3">
                                <!-- "Distrito" input -->
                                <div class="col-md-6">
                                    <label for="district" class="form-label">Distrito</label>
                                    <input 
                                        type="text" 
                                        class="form-control" 
                                        :class="{ 'is-invalid': errors.direction.district }"
                                        id="district" 
                                        v-model.trim="company.direction.district"
                                        @blur="validateDistrict"
                                        required>
                                    <div class="invalid-feedback" v-if="errors.direction.district">
                                        {{ errors.direction.district }}
                                    </div>
                                </div>
                                
                                <!-- "Barrio" input -->
                                <div class="col-md-6">
                                    <label for="neighborhood" class="form-label">Barrio</label>
                                    <input 
                                        type="text" 
                                        class="form-control" 
                                        :class="{ 'is-invalid': errors.direction.neighborhood }"
                                        id="neighborhood" 
                                        v-model.trim="company.direction.neighborhood" 
                                        @blur="validateNeighborhood"
                                        required>
                                    <div class="invalid-feedback" v-if="errors.direction.neighborhood">
                                        {{ errors.direction.neighborhood }}
                                    </div>
                                </div>
                            </div>
    
                            <!-- "Otras Señas" textarea -->
                            <div class="mb-3">
                                <label for="additionalDetails" class="form-label">Otras Señas</label>
                                <textarea 
                                    class="form-control" 
                                    :class="{ 'is-invalid': errors.direction.additionalDetails }"
                                    id="additionalDetails" 
                                    v-model.trim="company.direction.additionalDetails" 
                                    @blur="validateAdditionalDetails"
                                    rows="4" 
                                    required></textarea>
                                <div class="invalid-feedback" v-if="errors.direction.additionalDetails">
                                    {{ errors.direction.additionalDetails }}
                                </div>
                            </div>
                        </div>
                    </fieldset>
    
                    <!-- "Contacto" fieldset -->
                    <fieldset class="card">
                        <div class="card-body d-flex flex-column">
                            <!-- This div groups the title and subtitle of the fieldset -->
                            <div class="mb-4">
                                <legend class="card-title h4 fw-bold mb-2">Contacto</legend>
                                <h6 class="card-subtitle text-muted">Ingrese los contactos principales de su empresa</h6>
                            </div>
                            
                            <!-- "Correo Electrónico" input -->
                            <div class="mb-3">
                                <label for="email" class="form-label">Correo Electrónico</label>
                                <input 
                                    type="email" 
                                    class="form-control" 
                                    :class="{ 'is-invalid': errors.contact.email }"
                                    id="email" 
                                    v-model.trim="company.contact.email" 
                                    @blur="validateEmail"
                                    required>
                                <div class="invalid-feedback" v-if="errors.contact.email">
                                    {{ errors.contact.email }}
                                </div>
                            </div>

                            <!-- "Número de Teléfono" input -->
                            <div class="mb-3">
                                <label for="phoneNumber" class="form-label">Número de Teléfono</label>
                                <input 
                                    type="tel" 
                                    class="form-control" 
                                    :class="{ 'is-invalid': errors.contact.phoneNumber }"
                                    id="phoneNumber" 
                                    v-model.trim="company.contact.phoneNumber" 
                                    @blur="validatePhoneNumber"
                                    required>
                                <div class="invalid-feedback" v-if="errors.contact.phoneNumber">
                                    {{ errors.contact.phoneNumber }}
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>

            <!-- Second row -->
            <div class="row mt-4">
                <div class="col-12 d-flex justify-content-between align-items-center">
                    <p class="mb-0">¿Ya tiene una empresa? <router-link to="/login">Ingrese</router-link></p>
                    <button type="submit" class="btn btn-dark btn-lg">Registrar Empresa</button>
                </div>
            </div>
        </form>
    </div>
</template>

<script>
import axios from 'axios';
export default {
    name: 'CompanyRegistration',
    data() {
        return {
            company: {
                legalEntityId: '',
                name: '',
                description: '',
                paymentType: '',
                maxBenefits: null,
                direction: {
                    province: '',
                    canton: '',
                    district: '',
                    neighborhood: '',
                    additionalDetails: ''
                },
                contact: {
                    email: '',
                    phoneNumber: ''
                }
            },
            errors: {
                legalEntityId: '',
                name: '',
                description: '',
                paymentType: '',
                maxBenefits: '',
                direction: {
                    province: '',
                    canton: '',
                    district: '',
                    neighborhood: '',
                    additionalDetails: ''
                },
                contact: {
                    email: '',
                    phoneNumber: ''
                }
            }
        }
    },
    methods: {
        handleRegisterCompany() {
            // Validate all fields before submitting
            this.validateLegalEntityId();
            this.validateName();
            this.validateDescription();
            this.validatePaymentType();
            this.validateMaxBenefits();
            this.validateAddressFields();
            this.validateAdditionalDetails();
            this.validateEmail();
            this.validatePhoneNumber();
            
            // Check if there are any errors
            if (this.hasErrors()) {
                return;
            }
            
            this.registerCompany();
        },
        async registerCompany() {
            try {
                // Create request payload using form data
                const payload = {
                    person: {
                        id: "",
                        legalId: this.removeHyphens(this.company.legalEntityId),
                        type: "Legal Entity",
                        province: this.company.direction.province,
                        canton: this.company.direction.canton,
                        district: this.company.direction.district,
                        neighborhood: this.company.direction.neighborhood,
                        additionalDirectionDetails: this.company.direction.additionalDetails
                    },
                    contacts: [
                        {
                            id: "",
                            type: "Email",
                            phoneNumber: "",
                            email: this.company.contact.email,
                            personId: ""
                        },
                        {
                            id: "",
                            type: "Phone Number",
                            phoneNumber: this.removeHyphens(this.company.contact.phoneNumber),
                            personId: ""
                        }
                    ],
                    company: {
                        id: "",
                        name: this.company.name,
                        description: this.company.description,
                        paymentType: this.company.paymentType === "monthly" ? "Monthly" : 
                                   this.company.paymentType === "biweekly" ? "Biweekly" : "Weekly",
                        employees: [],
                        maxBenefitsPerEmployee: parseInt(this.company.maxBenefits),
                        creationDate: new Date().toISOString(),
                        creationAuthor: "system",
                        lastModificationDate: new Date().toISOString(),
                        lastModificationAuthor: "system"
                    }
                };
                
                console.log("Sending data:", JSON.stringify(payload, null, 2));
                
                const response = await axios.post('https://localhost:5000/api/Company/CreateCompanyWithDependencies', payload);

                if (response.status == 201) {
                    alert("Se registró la empresa exitosamente!");
                    this.$router.push('/main-menu');
                }
            } catch (error) {
                alert("No se pudo registrar la empresa");

                console.error("Error details:", error);
                if (error.response) {
                    console.error("Response data:", error.response.data);
                    console.error("Status:", error.response.status);
                    if (error.response.data && error.response.data.errors) {
                        console.error("Validation errors:", error.response.data.errors);
                    }
                } else if (error.request) {
                    // The request was made but no response was received
                    console.error("No response received:", error.request);
                } else {
                    // Something happened in setting up the request that triggered an error
                    console.error("Error:", error.message);
                }
            }
        },
        hasErrors() {
            // Check basic information errors
            if (this.errors.legalEntityId || this.errors.name || this.errors.description || 
                this.errors.paymentType || this.errors.maxBenefits) {
                return true;
            }
            
            // Check direction errors
            const directionErrors = Object.values(this.errors.direction);
            if (directionErrors.some(error => error !== '')) {
                return true;
            }
            
            // Check contact errors
            const contactErrors = Object.values(this.errors.contact);
            if (contactErrors.some(error => error !== '')) {
                return true;
            }
            
            return false;
        },
        validateLegalEntityId() {
            if (!this.company.legalEntityId) {
                this.errors.legalEntityId = 'La cédula jurídica es requerida';
                return;
            }

            if (!this.legalEntityIdHasAValidFormat()) {
                this.errors.legalEntityId = 'La cédula jurídica debe seguir el formato #-###-######';
                return;
            }

            this.errors.legalEntityId = '';
            return;
        },
        legalEntityIdHasAValidFormat() {
            const validFormat = /^\d-\d\d\d-\d\d\d\d\d\d$/;
            return validFormat.test(this.company.legalEntityId);
        },
        validateName() {
            if (!this.company.name) {
                this.errors.name = 'El nombre es requerido';
                return;
            }
            
            if (!this.nameContainsOnlyValidCharacters()) {
                this.errors.name = 'El nombre solo puede contener letras, números, puntos, comas, ampersands y espacios';
                return;
            }

            if (!this.nameHasAValidLength()) {
                this.errors.name = 'El nombre debe tener entre 2 y 50 caracteres';
                return;
            }

            this.errors.name = '';
            return;
        },
        nameHasAValidLength() {
            return this.company.name.length >= 2 && this.company.name.length <= 50;
        },
        nameContainsOnlyValidCharacters() {
            const validFormat = /^[a-zA-ZáéíóúñÁÉÍÓÚÑ0-9.,& -]+$/;
            return validFormat.test(this.company.name);
        },
        validateDescription() {
            if (!this.company.description) {
                this.errors.description = 'La descripción es requerida';
                return;
            }

            if (!this.descriptionContainsOnlyValidCharacters()) {
                this.errors.description = 'La descripción solo puede contener letras, números, puntos, comas, ampersands y espacios';
                return;
            }

            if (!this.descriptionHasAValidLength()) {
                this.errors.description = 'La descripción debe tener entre 10 y 300 caracteres';
                return;
            }

            this.errors.description = '';
            return;
        },
        descriptionHasAValidLength() {
            return this.company.description.length >= 10 && this.company.description.length <= 300;
        },
        descriptionContainsOnlyValidCharacters() {
            const validFormat = /^[a-zA-ZáéíóúñÁÉÍÓÚÑ0-9.,& -]+$/;
            return validFormat.test(this.company.description);
        },
        validatePaymentType() {
            if (!this.company.paymentType) {
                this.errors.paymentType = 'El tipo de pago es requerido';
                return;
            }

            this.errors.paymentType = '';
            return;
        },
        validateMaxBenefits() {
            if (this.company.maxBenefits === null || this.company.maxBenefits === '') {
                this.errors.maxBenefits = 'La cantidad máxima de beneficios es requerida';
                return;
            }

            const valueStr = String(this.company.maxBenefits);
            if (valueStr.length > 2) {
                this.errors.maxBenefits = 'La cantidad debe ser un número entre 0 y 10';
                this.company.maxBenefits = 10; // Truncate to 10
                return;
            }

            const value = parseInt(this.company.maxBenefits);
            if (isNaN(value) || value < 0 || value > 10) {
                this.errors.maxBenefits = 'La cantidad debe ser un número entre 0 y 10';
                if (value > 10) {
                    this.company.maxBenefits = 10;
                }
                return;
            }

            this.errors.maxBenefits = '';
            return;
        },
        validateAddressFields() {
            this.validateProvince();
            this.validateCanton();
            this.validateDistrict();
            this.validateNeighborhood();
        },
        validateProvince() {
            if (!this.company.direction.province) {
                this.errors.direction.province = 'La provincia es requerida';
                return;
            }

            if (!this.addressFieldOnlyContainsValidCharacters(this.company.direction.province)) {
                this.errors.direction.province = 'La provincia contiene caracteres no permitidos';
                return;
            }

            if (!this.addressFieldHasValidLength(this.company.direction.province)) {
                this.errors.direction.province = 'La provincia debe tener entre 3 y 30 caracteres';
                return;
            }

            this.errors.direction.province = '';
            return;
        },
        validateCanton() {
            if (!this.company.direction.canton) {
                this.errors.direction.canton = 'El cantón es requerido';
                return;
            }

            if (!this.addressFieldOnlyContainsValidCharacters(this.company.direction.canton)) {
                this.errors.direction.canton = 'El cantón contiene caracteres no permitidos';
                return;
            }

            if (!this.addressFieldHasValidLength(this.company.direction.canton)) {
                this.errors.direction.canton = 'El cantón debe tener entre 3 y 30 caracteres';
                return;
            }

            this.errors.direction.canton = '';
            return;
        },
        validateDistrict() {
            if (!this.company.direction.district) {
                this.errors.direction.district = 'El distrito es requerido';
                return;
            }

            if (!this.addressFieldOnlyContainsValidCharacters(this.company.direction.district)) {
                this.errors.direction.district = 'El distrito contiene caracteres no permitidos';
                return;
            }

            if (!this.addressFieldHasValidLength(this.company.direction.district)) {
                this.errors.direction.district = 'El distrito debe tener entre 3 y 30 caracteres';
                return;
            }

            this.errors.direction.district = '';
            return;
        },
        validateNeighborhood() {
            if (!this.company.direction.neighborhood) {
                this.errors.direction.neighborhood = 'El barrio es requerido';
                return;
            }

            if (!this.addressFieldOnlyContainsValidCharacters(this.company.direction.neighborhood)) {
                this.errors.direction.neighborhood = 'El barrio contiene caracteres no permitidos';
                return;
            }

            if (!this.addressFieldHasValidLength(this.company.direction.neighborhood)) {
                this.errors.direction.neighborhood = 'El barrio debe tener entre 3 y 30 caracteres';
                return;
            }

            this.errors.direction.neighborhood = '';
            return;
        },
        addressFieldHasValidLength(value) {
            return value.length >= 3 && value.length <= 30;
        },
        addressFieldOnlyContainsValidCharacters(value) {
            const validFormat = /^[a-zA-ZáéíóúñÁÉÍÓÚÑ0-9 -]+$/;
            return validFormat.test(value);
        },
        validateAdditionalDetails() {
            if (!this.company.direction.additionalDetails) {
                this.errors.direction.additionalDetails = 'Otras señas son requeridas';
                return;
            }

            if (!this.additionalDetailsContainsValidCharacters()) {
                this.errors.direction.additionalDetails = 'Las otras señas solo pueden contener letras, números, espacios, puntos, comas y guiones';
                return;
            }

            if (!this.additionalDetailsHasValidLength()) {
                this.errors.direction.additionalDetails = 'Las otras señas deben tener entre 10 y 150 caracteres';
                return;
            }

            this.errors.direction.additionalDetails = '';
        },
        additionalDetailsHasValidLength() {
            return this.company.direction.additionalDetails.length >= 10 && 
                   this.company.direction.additionalDetails.length <= 150;
        },
        additionalDetailsContainsValidCharacters() {
            const validFormat = /^[a-zA-ZáéíóúñÁÉÍÓÚÑ0-9 .,-]+$/;
            return validFormat.test(this.company.direction.additionalDetails);
        },
        validateEmail() {
            if (!this.company.contact.email) {
                this.errors.contact.email = 'El correo electrónico es requerido';
                return;
            }
            
            if (!this.emailHasValidFormat()) {
                this.errors.contact.email = 'El formato del correo electrónico no es válido';
                return;
            }

            if (!this.emailHasValidLength()) {
                this.errors.contact.email = 'El correo electrónico debe tener entre 5 y 50 caracteres';
                return;
            }

            this.errors.contact.email = '';
        },
        emailHasValidLength() {
            return this.company.contact.email.length >= 5
                && this.company.contact.email.length <= 50;
        },
        emailHasValidFormat() {
            const validFormat = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            return validFormat.test(this.company.contact.email);
        },
        validatePhoneNumber() {
            if (!this.company.contact.phoneNumber) {
                this.errors.contact.phoneNumber = 'El número de teléfono es requerido';
                return;
            }

            if (!this.phoneNumberHasValidFormat()) {
                this.errors.contact.phoneNumber = 'El número de teléfono debe tener el formato ####-####';
                return;
            }

            this.errors.contact.phoneNumber = '';
        },
        phoneNumberHasValidFormat() {
            const validFormat = /^[0-9]{4}-[0-9]{4}$/;
            return validFormat.test(this.company.contact.phoneNumber);
        },
        removeHyphens(string) {
            return string.replace(/-/g, '');
        }
        
    }
}
</script>

<style scoped>
textarea.form-control {
    resize: none;
}

.btn-dark {
    padding: 0.5rem 2rem;
}
</style>
