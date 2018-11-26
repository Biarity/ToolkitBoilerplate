<template>
    <div class="navbar-notification-list">
        <nav class="panel navbar-anti-burger">
            <p class="panel-heading">
                repositories
            </p>
            <div class="panel-block">
                <p class="control has-icons-left">
                    <input class="input is-small" type="text" placeholder="search">
                    <span class="icon is-small is-left">
                        <i class="fas fa-search" aria-hidden="true"></i>
                    </span>
                </p>
            </div>
            <p class="panel-tabs">
                <a class="is-active">all</a>
                <a>public</a>
                <a>private</a>
                <a>sources</a>
                <a>forks</a>
            </p>
            <a class="panel-block is-active">
                <span class="panel-icon">
                    <i class="fas fa-book" aria-hidden="true"></i>
                </span>
                bulma
            </a>
            <a class="panel-block">
                <span class="panel-icon">
                    <i class="fas fa-book" aria-hidden="true"></i>
                </span>
                marksheet
            </a>
            <a class="panel-block">
                <span class="panel-icon">
                    <i class="fas fa-book" aria-hidden="true"></i>
                </span>
                minireset.css
            </a>
            <a class="panel-block">
                <span class="panel-icon">
                    <i class="fas fa-book" aria-hidden="true"></i>
                </span>
                jgthms.github.io
            </a>
            <a class="panel-block">
                <span class="panel-icon">
                    <i class="fas fa-code-branch" aria-hidden="true"></i>
                </span>
                daniellowtw/infboard
            </a>
            <a class="panel-block">
                <span class="panel-icon">
                    <i class="fas fa-code-branch" aria-hidden="true"></i>
                </span>
                mojs
            </a>
            <label class="panel-block">
                <input type="checkbox">
                remember me
            </label>
            <div class="panel-block">
                <button class="button is-link is-outlined is-fullwidth">
                    reset all filters
                </button>
            </div>
        </nav>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';

    export default Vue.extend({
        name: 'NavbarNotificationList',
        props: {
            isMobile: Boolean,
        },
        methods: {
        },
    });
</script>

<style scoped lang="scss">
</style>
