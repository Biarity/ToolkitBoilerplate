<template>
    <div class="discover">
        <!-- Main container -->
        <div class="columns">
            <div class="column is-2">

                <aside class="menu">

                    <ul class="menu-list">
                        <li><a @click="mainFilter=''" :class="mainFilter==='' ? 'is-active' : ''">Discover</a></li>
                    </ul>

                    <p class="menu-label">
                        My Games
                    </p>
                    <ul class="menu-list">
                        <li v-for="filter in mainFilters"><a @click="mainFilter = mainFilter===filter ? '' : filter" :class="mainFilter===filter ? 'is-active' : ''">{{filter}}</a></li>
                    </ul>
                    <p class="menu-label">
                        Browse by genre
                    </p>
                    <ul class="menu-list">
                        <li v-for="filter in genreFilters"><a @click="genreFilter = genreFilter===filter ? '' : filter" :class="genreFilter===filter ? 'is-active' : ''">{{filter}}</a></li>
                    </ul>
                    <p class="menu-label">
                        Narrow it down
                    </p>
                    <ul class="menu-list">
                        <li v-for="filter in narrowFilters"><a @click="narrowFilter = narrowFilter===filter ? '' : filter" :class="narrowFilter===filter ? 'is-active' : ''">{{filter}}</a></li>

                    </ul>
                    <p class="menu-label">
                        Jump to
                    </p>
                    <ul class="menu-list">
                        <li><a>Discussions</a></li>
                        <li><a>Create Game</a></li>
                        <li><a>My Profile</a></li>
                        <li><a>About</a></li>
                        <li><a>Terms of Service</a></li>
                        <li><a>Privacy &amp; Cookies</a></li>
                    </ul>
                </aside>

            </div>
            <div class="column">


                <nav class="level">
                    <div class="level-left">
                        <a v-for="sort in mainSorts" class="level-item" :class="mainSort === sort ? 'has-text-weight-bold' : ''" @click="mainSort = sort">{{ sort }}</a>
                    </div>

                    <div class="level-right">

                        <p class="level-item">
                            <router-link to="/creator" class="button is-primary">
                                <span class="icon">
                                    <i class="fas fa-plus"></i>
                                </span>
                                <span>
                                    Create
                                </span>
                            </router-link>
                        </p>
                        <div class="level-item">
                            <form @submit.prevent="search">
                                <div class="field has-addons">
                                    <p class="control">
                                        <input class="input" type="text" v-model.trim="searchTerm" placeholder="Quick search..." maxlength="20">
                                    </p>
                                    <p class="control">
                                        <button class="button" type="submit">
                                            <i class="fas fa-search"></i>
                                        </button>
                                    </p>
                                </div>
                            </form>
                        </div>
                    </div>
                </nav>

                <my-infinite-loading entityPath="posts"> <!--:filters="filters" :sorts="sorts"-->
                    <PostsViewer slot="content" slot-scope="p" :data="p.data" :create-mode="$route.name === 'create'" />
                </my-infinite-loading>

            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import MyInfiniteLoading from '@/components/MyInfiniteLoading.vue';
    import PostsViewer from '@/components/PostsViewer.vue';

    export default Vue.extend({
        name: 'Discover',
        data() {
            return {
                searchTerm: '',
                mainSorts: ['Featured', 'Popular', 'Top', 'Recent'],
                mainSort: 'Featured',
                searchFilter: '',
                mainFilters: ['Played', 'Created', 'Starred'],
                mainFilter: '',
                genreFilters: ['Fantasy', 'Adventure', 'Mystery', 'SciFi',
                               'Drama', 'Romance', 'Comedy', 'Horror', 'Crime'],
                genreFilter: '',
                narrowFilters: ['Free', 'Online', 'Draft', 'Explicit'],
                narrowFilter: '',
            };
        },
        mounted() {
            if (this.$route.name === 'create') {
                this.mainFilter = 'Created';
            }
        },
        computed: {
            filters() {
                const f0 = this.searchFilter === '' ? '' : `Search==${this.searchFilter},`;
                const f1 = this.mainFilter === '' ? '' : `Status==${this.mainFilter},`;
                const f2 = this.genreFilter === '' ? '' : `Genre==${this.genreFilter},`;
                const f3 = this.narrowFilter === '' ? '' : `Filter==${this.narrowFilter}`;
                let filter = `${f0}${f1}${f2}${f3}`;
                if (filter.endsWith(',')) {
                    filter = filter.slice(0, -1);
                }
                return filter;
            },
            sorts() {
                return this.mainSort;
            },
        },
        methods: {
            search() {
                this.searchFilter = this.searchTerm.replace(',', '');
            },
        },
        components: {
            PostsViewer,
            MyInfiniteLoading,
        },
    });
</script>

<style lang="scss">
</style>
