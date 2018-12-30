<template>
    <div class="masonry posts-viewer">
        <router-link :to="{name:'post', params:{id:post.id}}" class="item" v-for="post in data" :key="post.Id">
            <div class="card ">
                <div class="card-image">
                    <figure class="image is-4by3">
                        <img :src="'https://picsum.photos/1280/960/?random&' + Math.floor(Math.random() * 100)" alt="Placeholder image">
                    </figure>
                </div>
                <div class="card-content">
                    <div class="media">
                        <div class="media-content">
                            <span class="title is-4">{{ post.name }}</span>
                        </div>
                    </div>

                    <div class="content">
                        {{ post.tagline }}
                        <span style="bottom: 20px; right: 20px; position: absolute">

                            <br />
                            <span v-if="post.isMature" class="tag is-pulled-right">Explicit</span>
                            <br />
                        </span>

                    </div>
                </div>
            </div>
        </router-link>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';

    export default Vue.extend({
        name: 'PostsViewer',
        props: {
            data: Array,
            createMode: Boolean,
        }
    });
</script>

<style scoped lang="scss">
    .card {
        margin-bottom: 20px;
        height: 400px;
        transition: all .2s ease-in-out;
    }

        .card:hover {
            transform: scale(1.01);
        }

    /* From https://stackoverflow.com/questions/44377343/css-only-masonry-layout-but-with-elements-ordered-horizontally */
    .masonry {
        display: grid;
        grid-auto-rows: 400px; /* Same as card height */
        grid-gap: 10px;
        grid-template-columns: repeat(auto-fill, minmax(30%, 1fr));
    }

    [short] {
        grid-row: span 1;
    }

    [tall] {
        grid-row: span 2;
    }

    [taller] {
        grid-row: span 3;
    }

    [tallest] {
        grid-row: span 4;
    }

    .item {
        display: flex;
    }

    /* Masonry on large screens */
    @media only screen and (min-width: 1024px) {
        .masonry {
            column-count: 3;
        }
    }

    /* Masonry on medium-sized screens */
    @media only screen and (max-width: 1023px) and (min-width: 768px) {
        .masonry {
            column-count: 2;
        }
    }

    /* Masonry on small screens */
    @media only screen and (max-width: 767px) {
        .masonry {
            column-count: 1;
        }
    }

</style>
