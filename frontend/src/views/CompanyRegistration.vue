<template>
    <div class="container mt-4">
        <form @submit.prevent="handleRegisterCompany">
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
                                    <input type="text" class="form-control" id="legalEntityId" v-model="company.legalEntityId" required>
                                </div>
    
                                <!-- "Nombre o Razón Social" input -->
                                <div class="mb-3">
                                    <label for="name" class="form-label">Nombre o Razón Social</label>
                                    <input type="text" class="form-control" id="name" v-model="company.name" required>
                                </div>

                                <!-- "Descripción" textarea -->
                                <div class="mb-3">
                                    <label for="description" class="form-label">Descripción</label>
                                    <textarea class="form-control" id="description" v-model="company.description" rows="4" required></textarea>
                                </div>

                                <!-- "Tipo de Pago" select -->
                                <div class="mb-3">
                                    <label for="paymentType" class="form-label">Tipo de Pago</label>
                                    <select class="form-select" id="paymentType" v-model="company.paymentType" required>
                                        <option value="" hidden></option>
                                        <option value="monthly">Mensual</option>
                                        <option value="biweekly">Quincenal</option>
                                        <option value="weekly">Semanal</option>
                                    </select>
                                </div>

                                <!-- "Cantidad Máxima de Beneficios" input -->
                                <div class="mb-4">
                                    <label for="maxBenefits" class="form-label">Cantidad Máxima de Beneficios</label>
                                    <input type="number" class="form-control" id="maxBenefits" v-model="company.maxBenefits" required>
                                    <div class="form-text text-muted">Cantidad máxima de beneficios que un empleado puede escoger</div>
                                </div>
                            </div>

                            <!-- This div groups the "Registrar Empresa" button and the "¿Ya tiene una empresa?" link and pushes them to the bottom -->
                            <div class="mt-auto">
                                <button type="submit" class="btn btn-dark mb-2">Registrar Empresa</button>
                                <p class="mt-2 mb-0">¿Ya tiene una empresa? <router-link to="/login">Ingrese</router-link></p>
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
                                    <input type="text" class="form-control" id="province" v-model="company.direction.province" required>
                                </div>

                                <!-- "Cantón" input -->
                                <div class="col-md-6">
                                    <label for="canton" class="form-label">Cantón</label>
                                    <input type="text" class="form-control" id="canton" v-model="company.direction.canton" required>
                                </div>
                            </div>

                            <!-- second row -->
                            <div class="row mb-3">
                                <!-- "Distrito" input -->
                                <div class="col-md-6">
                                    <label for="district" class="form-label">Distrito</label>
                                    <input type="text" class="form-control" id="district" v-model="company.direction.district" required>
                                </div>
                                
                                <!-- "Barrio" input -->
                                <div class="col-md-6">
                                    <label for="neighborhood" class="form-label">Barrio</label>
                                    <input type="text" class="form-control" id="neighborhood" v-model="company.direction.neighborhood" required>
                                </div>
                            </div>
    
                            <!-- "Otras Señas" textarea -->
                            <div class="mb-3">
                                <label for="otherSigns" class="form-label">Otras Señas</label>
                                <textarea class="form-control" id="otherSigns" v-model="company.direction.otherSigns" rows="4" required></textarea>
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
                                <input type="email" class="form-control" id="email" v-model="company.contact.email" required>
                            </div>

                            <!-- "Número de Teléfono" input -->
                            <div class="mb-3">
                                <label for="phoneNumber" class="form-label">Número de Teléfono</label>
                                <input type="tel" class="form-control" id="phoneNumber" v-model="company.contact.phoneNumber" required>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>
        </form>
    </div>
</template>

<script>
export default {
    name: 'CompanyRegistration',
    data() {
        return {
            company: {
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
                    otherSigns: ''
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
            console.log(this.company)
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
