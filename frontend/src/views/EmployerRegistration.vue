<template>
    <div class="container mt-4">
        <form @submit.prevent="registerEmployer">
            <fieldset>
                <div>
                    <legend>Regístrese Primero</legend>
                    <h6>Ingrese su información personal antes de registrar su empresa</h6>
                </div>

                <div>
                    <div>
                        <label for="firstName">Nombre</label>
                        <input type="text" id="firstName" v-model="employer.name.firstName">
                    </div>

                    <div>
                        <label for="firstSurname">Primer apellido</label>
                        <input type="text" id="firstSurname" v-model="employer.name.firstSurname">
                    </div>

                    <div>
                        <label for="secondSurname">Segundo apellido</label>
                        <input type="text" id="secondSurname" v-model="employer.name.secondSurname">
                    </div>
                </div>

                <div>
                    <label for="personalId">Cédula de Identidad</label>
                    <input type="text" id="personalId" v-model="employer.personalId">
                </div>

                <div>
                    <label for="phoneNumber">Número de Teléfono</label>
                    <input type="text" id="phoneNumber" v-model="employer.phoneNumber">
                </div>

                <div>
                    <div>
                        <label for="email">Correo Electrónico</label>
                        <input type="text" id="email" v-model="employer.email">
                    </div>

                    <div>
                        <label for="password">Contraseña</label>
                        <input type="password" id="password" v-model="employer.password">
                    </div>
                </div>

                <div>
                    <button type="submit">Registrarse</button>

                    <p>¿Ya está registrado? <a href="/login">Inicie sesión</a></p>
                </div>
            </fieldset>
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
                    personalId: '',
                    phoneNumber: '',
                    email: '',
                    password: '',
                }
            }
        },

        methods: {
            registerEmployer() {
                console.log(this.employer);
            }
        }
    }
</script>

<style lang="scss" scoped>

</style>