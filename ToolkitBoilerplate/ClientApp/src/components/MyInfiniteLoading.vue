<template>
    <div>
        <slot name="content" :data="data">
        </slot>
        <infinite-loading @infinite="loadData" spinner="waveDots" ref="iL">
            <div slot="no-results">
                <div style="text-align: center;">
                    Nothing here yet!
                </div>
            </div>
            <div slot="no-more">
                <div>
                    That's all.
                </div>
            </div>
        </infinite-loading>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import InfiniteLoading from 'vue-infinite-loading';
    import Fetch from '@/services/Fetch';

    export default Vue.extend({
        name: 'MyInfiniteLoading',
        props: {
            entityPath: String,
            filters: String,
            sorts: String,
            startAtPage: Number,
            pageSize: Number
        },
        data() {
            return {
                page: 1,
                data: new Array,
                metadata: {}
            };
        },
        watch: {
            filters() {
                this.reset();
            },
            sorts() {
                this.reset();
            },
        },
        mounted() {
            this.page = this.startAtPage || 1;
            // this.reset();
            // if (this.startAtPage) {
            //     this.$refs.iL.stateChanger.loaded();
            // }
        },
        methods: {
            reset() {
                this.$refs.iL.stateChanger.reset();
                this.page = this.startAtPage || 1;
                this.data = [];
                this.metadata = {};
            },
            async loadData($state: any) {
                const res = await Fetch.Read(this.entityPath, {
                    pageSize: this.pageSize || 10,
                    page: this.page,
                    filters: this.filters || '',
                    sorts: this.sorts || '',
                });

                if (res.status === 200) {
                    const json = await res.json();

                    if (json.data.length) {
                        this.data = [...this.data, ...json.data];
                        this.metadata = json.metadata || this.metadata;
                        this.page++;

                        $state.loaded();
                        if (this.data.length < this.pageSize) {
                            $state.complete();
                        }
                    } else {
                        $state.complete();
                    }
                }
            },
        },
        components: {
            InfiniteLoading,
        },
    });
</script>
