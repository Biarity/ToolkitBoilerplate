<template>
    <div class="alerts">
        <transition-group name="slide-fade">
            <div class="alert" v-for="alert in $store.state.alerts" :key="alert.id">
                <div class="notification" :class="'is-' + alert.type">
                    <button class="delete" aria-label="delete" @click="removeAlert(alert.id)"></button>
                    <strong v-if="alert.tagline.length >= 1">{{alert.tagline}} &nbsp;</strong>
                    {{alert.text}}
                </div>
            </div>
        </transition-group>

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

    /* Enter and leave animations can use different */
    /* durations and timing functions.              */
    .slide-fade-enter-active {
        transition: all .3s ease;
    }

    .slide-fade-leave-active {
        transition: all .8s cubic-bezier(1.0, 0.5, 0.8, 1.0);
    }

    .slide-fade-enter, .slide-fade-leave-to
    /* .slide-fade-leave-active below version 2.1.8 */ {
        transform: translateX(10px);
        opacity: 0;
    }

</style>
