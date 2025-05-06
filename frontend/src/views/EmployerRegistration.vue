<template>
    <div class="container mt-4 mb-4">
        <form @submit.prevent="registerEmployer">
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
                                    <input type="text" class="form-control" id="firstName" v-model="employer.name.firstName">
                                </div>
                                <!-- "Primer Apellido" input -->
                                <div class="col-md-4">
                                    <label for="firstSurname" class="form-label">Primer Apellido</label>
                                    <input type="text" class="form-control" id="firstSurname" v-model="employer.name.firstSurname">
                                </div>
                                <!-- "Segundo Apellido" input -->
                                <div class="col-md-4">
                                    <label for="secondSurname" class="form-label">Segundo Apellido</label>
                                    <input type="text" class="form-control" id="secondSurname" v-model="employer.name.secondSurname">
                                </div>
                            </div>
                            
                            <!-- "Cédula de Identidad" input -->
                            <div class="mb-3">
                                <label for="legalId" class="form-label">Cédula de Identidad</label>
                                <input type="text" class="form-control" id="legalId" v-model="employer.legalId">
                            </div>

                            <!-- "Teléfono" input -->
                            <div class="mb-3">
                                <label for="phoneNumber" class="form-label">Teléfono</label>
                                <input type="text" class="form-control" id="phoneNumber" v-model="employer.phoneNumber">
                            </div>

                            <!-- This div groups the "Correo" and "Contraseña" inputs -->
                            <div class="row g-3 mb-4">
                                <!-- "Correo" input -->
                                <div class="col-md-6">
                                    <label for="email" class="form-label">Correo</label>
                                    <input type="email" class="form-control" id="email" v-model="employer.email">
                                </div>
                                <!-- "Contraseña" input -->
                                <div class="col-md-6">
                                    <label for="password" class="form-label">Contraseña</label>
                                    <input type="password" class="form-control" id="password" v-model="employer.password">
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