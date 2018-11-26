<template>
    <div class="navbar-profile-list">
        <div>
            <a class="dropdown-item navbar-anti-burger" :href="getProfileUrl()">
                <span class="icon"> 
                    <i class="fas fa-user" aria-hidden="true"></i>&nbsp;
                </span>
                <span v-if="isMobile">
                    {{this.$store.getters.userName}}
                </span>
                <span v-else>
                    Profile
                </span>
            </a>
            <a class="dropdown-item navbar-anti-burger" href="#">
                <span class="icon">
                    <i class="fas fa-file" aria-hidden="true"></i>&nbsp;
                </span>
                <span>
                    Posts
                </span>
            </a>
            <hr class="navbar-divider">
            <a @click="logout" class="dropdown-item navbar-anti-burger" href="#">
                <span class="icon">
                    <i class="fas fa-sign-out-alt" aria-hidden="true"></i>&nbsp;
                </span>
                <span>
                    Logout
                </span>
            </a>
        </div>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Fetch from '@/services/Fetch';

    export default Vue.extend({
        name: 'NavbarProfileList',
        props: {
            isMobile: Boolean,
        },
        methods: {
            getProfileUrl() {
                // PROFILE URL
            },
            async logout() {
                const res = await Fetch.Req('account/logout', {
                    method: 'POST',
                });
                window.location.reload();
            },
        },
    });
</script>

<style scoped lang="scss">
</style>
