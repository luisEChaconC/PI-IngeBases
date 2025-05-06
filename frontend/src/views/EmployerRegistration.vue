<template>
    <div class="container mt-4 mb-4">
        <form @submit.prevent="handleRegisterEmployer" novalidate>
            <div class="row justify-content-center">

                <!-- "Regístrese Primero" fieldset -->
                <fieldset class="col-md-8">
                    <div class="card">
                        <div class="card-body p-4">
                            
                            <!-- This div groups the title and subtitle of the fieldset -->
                            <div class="mb-4">
                                <legend class="card-title h4 fw-bold mb-2">Regístrese Primero</legend>
                                <h6 class="card-subtitle text-muted">Ingrese su información personal antes de registrar su empresa</h6>
                            </div>
                            
                            <!-- This div groups the "Nombre", "Primer Apellido" and "Segundo Apellido" inputs -->
                            <div class="row g-3 mb-3">
                                <!-- "Nombre" input -->
                                <div class="col-md-4">
                                    <label for="firstName" class="form-label">Nombre</label>
                                    <input 
                                        type="text" 
                                        class="form-control" 
                                        :class="{ 'is-invalid': errors.name.firstName }"
                                        id="firstName" 
                                        v-model="employer.name.firstName"
                                        @blur="validateFirstName">
                                    <div class="invalid-feedback" v-if="errors.name.firstName">
                                        {{ errors.name.firstName }}
                                    </div>
                                </div>
                                <!-- "Primer Apellido" input -->
                                <div class="col-md-4">
                                    <label for="firstSurname" class="form-label">Primer Apellido</label>
                                    <input 
                                        type="text" 
                                        class="form-control" 
                                        :class="{ 'is-invalid': errors.name.firstSurname }"
                                        id="firstSurname" 
                                        v-model="employer.name.firstSurname"
                                        @blur="validateFirstSurname">
                                    <div class="invalid-feedback" v-if="errors.name.firstSurname">
                                        {{ errors.name.firstSurname }}
                                    </div>
                                </div>
                                <!-- "Segundo Apellido" input -->
                                <div class="col-md-4">
                                    <label for="secondSurname" class="form-label">Segundo Apellido</label>
                                    <input 
                                        type="text" 
                                        class="form-control" 
                                        :class="{ 'is-invalid': errors.name.secondSurname }"
                                        id="secondSurname" 
                                        v-model="employer.name.secondSurname"
                                        @blur="validateSecondSurname">
                                    <div class="invalid-feedback" v-if="errors.name.secondSurname">
                                        {{ errors.name.secondSurname }}
                                    </div>
                                </div>
                            </div>
                            
                            <!-- "Cédula de Identidad" input -->
                            <div class="mb-3">
                                <label for="legalId" class="form-label">Cédula de Identidad</label>
                                <input 
                                    type="text" 
                                    class="form-control" 
                                    :class="{ 'is-invalid': errors.legalId }"
                                    id="legalId" 
                                    v-model="employer.legalId"
                                    @blur="validateLegalId">
                                <div class="invalid-feedback" v-if="errors.legalId">
                                    {{ errors.legalId }}
                                </div>
                            </div>

                            <!-- "Teléfono" input -->
                            <div class="mb-3">
                                <label for="phoneNumber" class="form-label">Teléfono</label>
                                <input 
                                    type="text" 
                                    class="form-control" 
                                    :class="{ 'is-invalid': errors.phoneNumber }"
                                    id="phoneNumber" 
                                    v-model="employer.phoneNumber"
                                    @blur="validatePhoneNumber">
                                <div class="invalid-feedback" v-if="errors.phoneNumber">
                                    {{ errors.phoneNumber }}
                                </div>
                            </div>

                            <!-- This div groups the "Correo" and "Contraseña" inputs -->
                            <div class="row g-3 mb-4">
                                <!-- "Correo" input -->
                                <div class="col-md-6">
                                    <label for="email" class="form-label">Correo</label>
                                    <input 
                                        type="email" 
                                        class="form-control" 
                                        :class="{ 'is-invalid': errors.email }"
                                        id="email" 
                                        v-model="employer.email"
                                        @blur="validateEmail">
                                    <div class="invalid-feedback" v-if="errors.email">
                                        {{ errors.email }}
                                    </div>
                                </div>
                                <!-- "Contraseña" input -->
                                <div class="col-md-6">
                                    <label for="password" class="form-label">Contraseña</label>
                                    <input 
                                        type="password" 
                                        class="form-control" 
                                        :class="{ 'is-invalid': errors.password }"
                                        id="password" 
                                        v-model="employer.password"
                                        @blur="validatePassword">
                                    <div class="invalid-feedback" v-if="errors.password">
                                        {{ errors.password }}
                                    </div>
                                </div>
                            </div>
                            
                            <!-- This div groups the "Registrarse" button and the "Inicie sesión" link -->
                            <div>
                                <!-- "Registrarse" button -->
                                <button type="submit" class="btn btn-dark">Registrarse</button>
                                <!-- "Inicie sesión" link -->
                                <p class="mt-3 mb-0">¿Ya está registrado? <a href="/login" class="text-decoration-none">Inicie sesión</a></p>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </div>
        </form>
    </div>
</template>

<script>
    export default {
        name: 'EmployerRegistration',
        data() {
            return {
                employer: {
                    name: {
                        firstName: '',
                        firstSurname: '',
                        secondSurname: '',
                    },
                    legalId: '',
                    phoneNumber: '',
                    email: '',
                    password: '',
                },
                errors: {
                    name: {
                        firstName: '',
                        firstSurname: '',
                        secondSurname: '',
                    },
                    legalId: '',
                    phoneNumber: '',
                    email: '',
                    password: '',
                }
            }
        },

        methods: {
            handleRegisterEmployer() {
                this.validateAllFields();
                
                if (this.hasErrors()) {
                    return;
                }
                
                console.log(this.employer);
                // Here will be the code to register the employer
            },
            
            validateAllFields() {
                this.validateFirstName();
                this.validateFirstSurname();
                this.validateSecondSurname();
                this.validateLegalId();
                this.validatePhoneNumber();
                this.validateEmail();
                this.validatePassword();
            },
            
            hasErrors() {
                // Check name errors
                const nameErrors = Object.values(this.errors.name);
                if (nameErrors.some(error => error !== '')) {
                    return true;
                }
                
                // Check other fields errors
                if (this.errors.legalId || this.errors.phoneNumber || 
                    this.errors.email || this.errors.password) {
                    return true;
                }
                
                return false;
            },
            
            validateFirstName() {
                if (!this.employer.name.firstName) {
                    this.errors.name.firstName = 'El nombre es requerido';
                    return;
                }
                
                if (!this.nameContainsOnlyValidCharacters(this.employer.name.firstName)) {
                    this.errors.name.firstName = 'El nombre solo puede contener letras y espacios';
                    return;
                }
                
                if (!this.nameHasValidLength(this.employer.name.firstName)) {
                    this.errors.name.firstName = 'El nombre debe tener entre 3 y 50 caracteres';
                    return;
                }
                
                this.errors.name.firstName = '';
            },
            
            validateFirstSurname() {
                if (!this.employer.name.firstSurname) {
                    this.errors.name.firstSurname = 'El primer apellido es requerido';
                    return;
                }
                
                if (!this.nameContainsOnlyValidCharacters(this.employer.name.firstSurname)) {
                    this.errors.name.firstSurname = 'El primer apellido solo puede contener letras y espacios';
                    return;
                }
                
                if (!this.nameHasValidLength(this.employer.name.firstSurname)) {
                    this.errors.name.firstSurname = 'El primer apellido debe tener entre 3 y 50 caracteres';
                    return;
                }
                
                this.errors.name.firstSurname = '';
            },
            
            validateSecondSurname() {
                if (!this.employer.name.secondSurname) {
                    this.errors.name.secondSurname = 'El segundo apellido es requerido';
                    return;
                }
                
                if (!this.nameContainsOnlyValidCharacters(this.employer.name.secondSurname)) {
                    this.errors.name.secondSurname = 'El segundo apellido solo puede contener letras y espacios';
                    return;
                }
                
                if (!this.nameHasValidLength(this.employer.name.secondSurname)) {
                    this.errors.name.secondSurname = 'El segundo apellido debe tener entre 3 y 50 caracteres';
                    return;
                }
                
                this.errors.name.secondSurname = '';
            },
            
            nameHasValidLength(value) {
                return value.length >= 3 && value.length <= 50;
            },
            
            nameContainsOnlyValidCharacters(value) {
                const validFormat = /^[a-zA-ZáéíóúñÁÉÍÓÚÑ ]+$/;
                return validFormat.test(value);
            },
            
            validateLegalId() {
                if (!this.employer.legalId) {
                    this.errors.legalId = 'La cédula de identidad es requerida';
                    return;
                }
                
                if (!this.legalIdHasValidFormat()) {
                    this.errors.legalId = 'La cédula de identidad debe seguir el formato #-####-####';
                    return;
                }
                
                if (!this.legalIdHasValidLength()) {
                    this.errors.legalId = 'La cédula de identidad debe contener exactamente 10 caracteres numéricos';
                    return;
                }
                
                this.errors.legalId = '';
            },
            
            legalIdHasValidLength() {
                return this.employer.legalId.length === 10;
            },
            
            legalIdHasValidFormat() {
                const validFormat = /^\d-\d\d\d\d-\d\d\d\d$/;
                return validFormat.test(this.employer.legalId);
            },
            validatePhoneNumber() {
                if (!this.employer.phoneNumber) {
                    this.errors.phoneNumber = 'El número de teléfono es requerido';
                    return;
                }
                
                if (!this.phoneNumberHasValidFormat()) {
                    this.errors.phoneNumber = 'El número de teléfono debe tener el formato ####-####';
                    return;
                }
                
                this.errors.phoneNumber = '';
            },
            
            phoneNumberHasValidFormat() {
                const validFormat = /^[0-9]{4}-[0-9]{4}$/;
                return validFormat.test(this.employer.phoneNumber);
            },
            validateEmail() {
                if (!this.employer.email) {
                    this.errors.email = 'El correo electrónico es requerido';
                    return;
                }
                
                if (!this.emailContainsOnlyValidCharacters()) {
                    this.errors.email = 'El correo contiene caracteres no permitidos';
                    return;
                }

                if (!this.emailHasValidFormat()) {
                    this.errors.email = 'El formato del correo electrónico no es válido';
                    return;
                }
                
                if (!this.emailHasValidLength()) {
                    this.errors.email = 'El correo electrónico debe tener entre 5 y 50 caracteres';
                    return;
                }

                this.errors.email = '';
            },
            
            emailHasValidLength() {
                return this.employer.email.length >= 5 && this.employer.email.length <= 50;
            },
            
            emailHasValidFormat() {
                const validFormat = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                return validFormat.test(this.employer.email);
            },
            
            emailContainsOnlyValidCharacters() {            
                const validFormat = /^[a-zA-Z0-9\-_!@.]+$/;
                return validFormat.test(this.employer.email);
            },
            validatePassword() {
                if (!this.employer.password) {
                    this.errors.password = 'La contraseña es requerida';
                    return;
                }
                
                if (!this.passwordContainsValidCharacters()) {
                    this.errors.password = 'La contraseña contiene caracteres no permitidos';
                    return;
                }

                if (!this.passwordHasValidLength()) {
                    this.errors.password = 'La contraseña debe tener entre 8 y 100 caracteres';
                    return;
                }
                
                if (!this.passwordMeetsComplexityRequirements()) {
                    this.errors.password = 'La contraseña debe contener al menos una letra mayúscula, una minúscula, un número y un caracter especial (#, $, !, ?, %, &, /)';
                    return;
                }
                
                this.errors.password = '';
            },
            
            passwordHasValidLength() {
                return this.employer.password.length >= 8 && this.employer.password.length <= 100;
            },
            
            passwordContainsValidCharacters() {
                const validFormat = /^[a-zA-ZáéíóúñÁÉÍÓÚÑ0-9\-_#$!¿?%&/\\]+$/;
                return validFormat.test(this.employer.password);
            },
            
            passwordMeetsComplexityRequirements() {
                // Check for at least one uppercase letter
                const hasUpperCase = /[A-ZÁÉÍÓÚÑáéíóúñ]/.test(this.employer.password);
                
                // Check for at least one lowercase letter
                const hasLowerCase = /[a-zÁÉÍÓÚÑáéíóúñ]/.test(this.employer.password);
                
                // Check for at least one number
                const hasNumber = /[0-9]/.test(this.employer.password);
                
                // Check for at least one special character
                const hasSpecialChar = /[#$!?%&/]/.test(this.employer.password);
                
                return hasUpperCase && hasLowerCase && hasNumber && hasSpecialChar;
            }
        }
    }
</script>

<style lang="scss" scoped>
</style>