<template>
    <div>
        <form @submit.prevent="handleRegisterCompany">
            <fieldset>
                <legend>Información Básica</legend>
                <small>Ingrese la información básica de su empresa</small>

                <label for="legalEntityId">Cédula Jurídica</label>
                <input type="text" id="legalEntityId" v-model="company.legalEntityId" required>

                <label for="name">Nombre o Razón Social</label>
                <input type="text" id="name" v-model="company.name" required>

                <label for="description">Descripción</label>
                <textarea id="description" v-model="company.description" required></textarea>

                <label for="paymentType">Tipo de Pago</label>
                <select id="paymentType" v-model="company.paymentType" required>
                    <option value="monthly">Mensual</option>
                    <option value="biweekly">Quincenal</option>
                    <option value="weekly">Semanal</option>
                </select>

                <label for="maxBenefits">Cantidad Máxima de Beneficios</label>
                <input type="number" id="maxBenefits" v-model="company.maxBenefits" required>
                <small>Cantidad máxima de beneficios que un empleado puede escoger</small>

                <button type="submit">Registrar Empresa</button>
                <p>¿Ya tiene una empresa? <a href="/login">Ingrese</a></p>
            </fieldset>

            <fieldset>
                <legend>Dirección</legend>
                <small>Ingrese la dirección de su empresa</small>

                <label for="province">Provincia</label>
                <input type="text" id="province" v-model="company.direction.province" required>

                <label for="canton">Cantón</label>
                <input type="text" id="canton" v-model="company.direction.canton" required>

                <label for="district">Distrito</label>
                <input type="text" id="district" v-model="company.direction.district" required>

                <label for="neighborhood">Barrio</label>
                <input type="text" id="neighborhood" v-model="company.direction.neighborhood" required>

                <label for="otherSigns">Otras Señas</label>
                <textarea id="otherSigns" v-model="company.direction.otherSigns" required></textarea>
            </fieldset>

            <fieldset>
                <legend>Contacto</legend>
                <small>Ingrese la información de contacto de su empresa</small>

                <label for="email">Correo Electrónico</label>
                <input type="email" id="email" v-model="company.contact.email" required>

                <label for="phoneNumber">Número de Teléfono</label>
                <input type="tel" id="phoneNumber" v-model="company.contact.phoneNumber" required>
            </fieldset>
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
<style>
</style>
