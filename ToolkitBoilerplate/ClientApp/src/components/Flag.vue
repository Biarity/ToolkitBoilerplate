<template>
    <div class="login">
        <div class="modal" :class="$store.state.showFlagModal ? 'is-active' : ''">
            <div class="modal-background" @click="$store.commit('toggleFlagModal')"></div>
            <div class="modal-content">
                    <form class="card" @submit.prevent="submit">
                        <header class="card-header">
                            <p class="card-header-title">
                                Flag
                                {{$store.state.flagType}}
                            </p>
                            <span class="card-header-icon">
                                #{{$store.state.flagId}}
                            </span>
                        </header>
                        <div class="card-content">
                            <div class="content">
                                <textarea class="textarea" :placeholder="`Why should this ${$store.state.flagType} be taken down?`" v-model="body" required minlength="5"></textarea>
                            </div>
                        </div>
                        <footer class="card-footer">
                            <div class="card-footer-item">
                                <button type="submit" class="button">Submit</button>

                            </div>
                        </footer>
                    </form>
            </div>
            <button class="modal-close is-large" aria-label="close" @click="$store.commit('toggleFlagModal')"></button>
        </div>
    </div>
</template>
<script lang="ts">
    import Vue from 'vue';
    import Fetch from '@/services/Fetch';

    export default Vue.extend({
        name: 'Flag',
        data() {
            return {
                body: '',
            };
        },
        mounted() {
        },
        methods: {
            async submit() {
                const data = {
                    body: this.body,
                };

                if (this.$store.state.flagType === 'comment') {
                    data.commentId = this.$store.state.flagId;
                }

                const res = await Fetch.Create('flags', data);
                
                if (res.status === 200) {
                    this.$store.commit('addAlert', {
                        type: 'info',
                        text: `Your report was submitted (${this.$store.state.flagType} #${this.$store.state.flagId})`,
                    });
                    this.$store.commit('toggleFlagModal');
                    this.body = '';
                    this.$store.commit('addFlagLocal');
                }
            }
        },
    });
</script>
<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
</style>
