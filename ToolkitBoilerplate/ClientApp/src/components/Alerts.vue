<template>
    <div class="alerts">
        <div class="alert" v-for="alert in $store.state.alerts" :key="alert.id">
            <div class="notification" :class="'is-' + alert.type">
                <button class="delete" aria-label="delete" @click="removeAlert(alert.id)"></button>
                <strong v-if="alert.tagline.length >= 1">{{alert.tagline}} &nbsp;</strong>
                {{alert.text}}
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';

    export default Vue.extend({
        name: 'Alerts',
        props: {
            msg: String,
        },
        methods: {
            removeAlert(alertId: any) {
                this.$store.commit('removeAlert', alertId);
            },
        },
    });
</script>

<style scoped lang="scss">
    .alert {
        padding-top: 10px;
    }

    .alerts {
        z-index: 2147483647;
        position: fixed;
        bottom: 10px;
        right: 10px;
        width: 35%;
    }
</style>
