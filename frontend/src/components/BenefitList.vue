<template>
    <div class="container mt-5">
        <h1 class="mb-4">Beneficios</h1>

        <div class="row align-items-center mb-4">
            <div class="col-auto">
                <label for="maxBenefits" class="form-label fw-bold">Cantidad maxima de beneficios</label>
            </div>
            <div class="col-auto">
                <select id="maxBenefits" v-model="maxBenefits" class="form-select">
                    <option v-for="n in 5" :key="n" :value="n">{{ n }}</option>
                </select>
            </div>
            <div class="col text-end">
                <button class="btn btn-dark">
                    + Nuevo beneficio
                </button>
            </div>
        </div>

        <table class="table table-bordered">
            <thead class="table-light">
                <tr>
                    <th>Nombre</th>
                    <th>Estado</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(benefit, index) in benefits" :key="index">
                    <td>{{ benefit.name }}</td>
                    <td>{{ benefit.isActive ? 'Activo' : 'Inactivo' }}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { ref, onMounted } from 'vue'

    const maxBenefits = ref(3)

    const benefits = ref([])

    // Backend connection

    const getBenefits = async () => {
        try {
            const response = await axios.get("https://localhost:5000/api/benefit")
            benefits.value = response.data
        } catch (error) {
            console.error("Not able to obtain the benefits:", error)
        }
    }
    // When it the component starts
    onMounted(() => {
        getBenefits()
    })

</script>

<style scoped>
 
</style>
