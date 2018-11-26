<template>
    <nav id="navbar" class="navbar is-fixed-top is-primary"> <!-- ADD is-dark FOR DARK -->
        <div class="container">
            <div class="navbar-brand">
                <router-link to="/" class="navbar-item">
                    Toolkit Boilerplate
                    <!--<img src="https://bulma.io/images/bulma-logo.png" alt="Bulma: a modern CSS framework based on Flexbox" width="112" height="28">-->
                </router-link>

                <!-- MOBILE -->
                <div v-if="this.$store.getters.isAuthenticated" class="navbar-burger navbar-item is-hidden-desktop" data-target="messages" :data-badge="3">
                    <i class="fas fa-inbox" aria-hidden="true"></i>
                </div>
                <div v-if="this.$store.getters.isAuthenticated" class="navbar-burger navbar-item is-hidden-desktop" data-target="profile">
                    <i class="fas fa-user" aria-hidden="true"></i>
                </div>
                <div v-else class="navbar-burger navbar-item is-hidden-desktop" @click="$store.commit('toggleLoginModal')">
                    <i class="fas fa-sign-in-alt" aria-hidden="true"></i>
                </div>
                <div class="navbar-burger navbar-item is-hidden-desktop" data-target="browse">
                    <i class="fas fa-bars" aria-hidden="true"></i>
                </div>

            </div>

            <!-- BROWSE MENU -->
            <div class="navbar-menu navbar-anti-burger" id="browse">
                <div class="navbar-start">
                    <router-link to="/discover" class="navbar-item">
                        <span class="icon">
                            <i class="far fa-star"></i>
                        </span>&nbsp;
                        <span>
                            Discover
                        </span>
                    </router-link>
                    <router-link to="/discuss" class="navbar-item">
                        <span class="icon">
                            <i class="far fa-comment-alt"></i>
                        </span>&nbsp;
                        <span>
                            Discuss
                        </span>
                    </router-link>
                    <router-link to="/create" class="navbar-item">
                        <span class="icon">
                            <i class="fas fa-plus"></i>
                        </span>&nbsp;
                        <span>
                            Create
                        </span>
                    </router-link>
                </div>
                <div class="navbar-end">
                </div>
            </div>



            <!-- MOBILE -->
            <div class="navbar-menu is-hidden-desktop" id="messages">
                <NavbarNotificationList />
            </div>
            <div class="navbar-menu is-hidden-desktop" id="profile">
                <NavbarProfileList isMobile />
            </div>


            <!-- DESKTOP -->
            <div class="navbar-menu" id="mainNavbar">

                <div class="navbar-start">
                </div>

                <div class="navbar-end">
                    <!-- MESSAGES -->
                    <a v-if="this.$store.getters.isAuthenticated" class="navbar-item has-dropdown is-hoverable">
                        <div class="navbar-item" href="#" :data-badge="3">
                            <i class="fas fa-inbox" aria-hidden="true"></i>
                        </div>
                        <div class="navbar-dropdown is-right">
                            <NavbarNotificationList />
                        </div>
                    </a>
                    <!-- PROFILE -->
                    <a v-if="this.$store.getters.isAuthenticated" class="navbar-item has-dropdown is-hoverable">
                        <div class="navbar-item" href="#">
                            <span class="icon is-normal">
                                <i class="fas fa-user" aria-hidden="true"></i>
                            </span> &nbsp;
                            <span>{{ $store.getters.userName }}</span>
                        </div>
                        <div class="navbar-dropdown is-right">
                            <NavbarProfileList />
                        </div>
                    </a>

                    <!-- LOGIN PROMPT BUTTON -->
                    <a v-show="!this.$store.getters.isAuthenticated" class="navbar-item has-dropdown" @click="$store.commit('toggleLoginModal')">
                        <div class="navbar-item">
                            <div class="button">
                                <span class="icon">
                                    <i class="fas fa-sign-in-alt"></i>
                                </span>
                                <span>
                                    Register or Login
                                </span>
                            </div>
                        </div>
                    </a>

                </div>

            </div>


        </div>
    </nav>
</template>

<script lang="ts">
    import Vue from 'vue';
    import NavbarProfileList from './Navbar/NavbarProfileList.vue';
    import NavbarNotificationList from './Navbar/NavbarNotificationList.vue';

    export default Vue.extend({
        name: 'Navbar',
        mounted() {
            // Make mobile nav buttons work
            document.addEventListener('DOMContentLoaded', () => {
                const $navbarBurgers = Array.prototype.slice.call(document.querySelectorAll('.navbar-burger'), 0);
                if ($navbarBurgers.length > 0) {
                    $navbarBurgers.forEach(($el: any) => {
                        $el.addEventListener('click', () => {
                            $navbarBurgers.forEach(($elOther: any) => {
                                if ($el !== $elOther) {
                                    const target = $elOther.dataset.target;
                                    const $target = document.getElementById(target);
                                    $elOther.classList.remove('is-active');
                                    if ($target != null) {
                                        $target.classList.remove('is-active');
                                    }
                                }
                            });
                            const target1 = $el.dataset.target;
                            const $target1 = document.getElementById(target1);
                            $el.classList.toggle('is-active');
                            if ($target1 != null) {
                                $target1.classList.toggle('is-active');
                            }
                        });
                    });
                }
                const $navbarAntiBurgers = Array.prototype.slice
                    .call(document.querySelectorAll('.navbar-anti-burger'), 0);
                if ($navbarAntiBurgers.length > 0) {
                    $navbarAntiBurgers.forEach(($el: any) => {
                        $el.addEventListener('click', () => {
                            $navbarBurgers.forEach(($elOther: any) => {
                                const target2 = $elOther.dataset.target;
                                const $target2 = document.getElementById(target2);
                                $elOther.classList.remove('is-active');
                                if ($target2 != null) {
                                    $target2.classList.remove('is-active');
                                }
                            });
                        });
                    });
                }
            });
        },
        components: {
            NavbarProfileList,
            NavbarNotificationList,
        },
    });
</script>

<style scoped lang="scss">
    .navbar-dropdown {
        padding-top: 0
    }

    .navbar {
        font-size: 1.12rem;
    }

    .navbar-brand {
        font-size: 1.2rem;
    }

    #navbar {
        border-top: 3px solid white;
        border-bottom: 1px solid black;
    }

    [data-badge] {
        position: relative;
    }

    [data-badge]:after {
        content: attr(data-badge);
        position: absolute;
        top: 6px;
        right: 3px;
        font-size: .7em;
        background: #a20025;
        color: white;
        width: 15px;
        max-height: 15px;
        text-align: center;
        line-height: 15px;
        border-radius: 50%;
    }
</style>
