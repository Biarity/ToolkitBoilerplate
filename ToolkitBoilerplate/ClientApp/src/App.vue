<template>
    <div id="app">
        <Navbar></Navbar>
        <br />
        <br />
        <div class="container">
            <router-view />
        </div>
        <Alerts />
        <Login v-if="!$store.getters.isAuthenticated" />
        <Flag />
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Alerts from '@/components/Alerts.vue';
    import Navbar from '@/components/Navbar.vue';
    import Login from '@/components/Login.vue';
    import Flag from '@/components/Flag.vue';

    export default Vue.extend({
        name: 'app',
        components: {
            Alerts,
            Navbar,
            Login,
            Flag
        },
    });
</script>

<style lang="scss">
</style>
