<template>
    <div class="login">
        <div class="modal" :class="$store.state.showLoginModal ? 'is-active' : ''">
            <div class="modal-background" @click="$store.commit('toggleLoginModal')"></div>
            <div class="modal-content">
                <article class="media box">
                    <!--
    <div class="media-left">
        <i class="fas fa-laugh" style="font-size: 3rem;"></i>
    </div>
        -->

                    <div id="sendCodeRecaptcha">
                    </div>
                    <div class="media-content">
                        <div class="content">
                            <div v-if="!$store.getters.isAuthenticated">
                                <span class="label">
                                    <span v-if="isRegistering">Almost there! Fill this out to create an account.</span>
                                    <span v-else-if="codeSent">Code sent to <code>{{this.email}}</code></span>
                                    <span v-else>Register or Login</span>
                                </span>
                                <form @submit.prevent="validateSendCodeRecaptcha" v-if="!codeSent">
                                    <div class="field has-addons">
                                        <div class="control has-icons-left">
                                            <input class="input" name="email" type="email" placeholder="Email" v-model="email" required autocomplete="email">
                                            <span class="icon is-small is-left">
                                                <i class="fas fa-envelope"></i>
                                            </span>
                                        </div>
                                        <div class="control">
                                            <input type="submit" class="button" value="Get Code">
                                        </div>
                                    </div>
                                </form>

                                <form @submit.prevent="submitCode" v-if="codeSent && !isRegistering">
                                    <div class="field has-addons">
                                        <div class="control">
                                            <a class="button is-info" @click="codeSent = false;">
                                                <span class="icon">
                                                    <i class="fas fa-angle-left"></i>
                                                </span>
                                            </a>
                                        </div>
                                        <div class="control has-icons-left">
                                            <input class="input" type="text" v-model.trim="code" placeholder="Code" required pattern="[0-9]{6}" minlength="6" maxlength="6">
                                            <span class="icon is-small is-left">
                                                <i class="fas fa-key"></i>
                                            </span>
                                        </div>
                                        <div class="control">
                                            <input type="submit" class="button" value="Login">
                                        </div>
                                    </div>
                                    <label class="checkbox">
                                        <input type="checkbox" v-model="rememberMe">
                                        Remember me
                                    </label>
                                </form>

                                <form @submit.prevent="submitCode" v-if="isRegistering">
                                    <div class="field">
                                        <label class="label">Username</label>
                                        <div class="control has-icons-left">
                                            <input class="input" type="text" v-model="user.userName" placeholder="No spaces" required pattern="[A-Za-z0-9]{3,10}" minlength="3" maxlength="10">
                                            <span class="icon is-small is-left">
                                                <i class="fas fa-user"></i>
                                            </span>
                                        </div>
                                    </div>

                                    <div class="control">
                                        <input type="submit" class="button" value="Login">
                                    </div>
                                </form>
                            </div>
                            <div v-else>
                                Logged-in as <em>{{ $store.getters.userName }}</em>
                            </div>
                        </div>
                    </div>
                </article>
            </div>
            <button class="modal-close is-large" aria-label="close" @click="$store.commit('toggleLoginModal')"></button>
        </div>

    </div>
</template>
<script lang="ts">
    import Vue from 'vue';
    import Fetch from '@/services/Fetch';

    export default Vue.extend({
        name: 'Login',
        data() {
            return {
                email: '',
                rememberMe: true,
                code: '',
                codeSent: false,
                isRegistering: false,
                user: {
                    userName: '',
                },
            };
        },
        mounted() {
            this.initReCaptcha();
        },
        methods: {
            initReCaptcha() {
                try {
                    if (!this.$store.state.grecaptchaRendered) {
                        grecaptcha.render('sendCodeRecaptcha', {
                            sitekey: '6Lda_TwUAAAAAJmEYd7pN_Ywp-yVWcX8a1z5yJRg',
                            size: 'invisible',
                            callback: this.sendCode,
                        });
                        this.$store.commit('grecaptchaRendered');
                    }
                } catch (e) {
                    setTimeout(this.initReCaptcha, 100);
                }
            },
            validateSendCodeRecaptcha(e) {
                const res = grecaptcha.getResponse();
                if (res) {
                    this.sendCode(res);
                } else {
                    grecaptcha.execute();
                }
            },
            async sendCode(recaptchaResponse) {
                this.codeSent = true;
                const formData = new FormData();
                formData.append('email', this.email);
                formData.append('g-recaptcha-response', recaptchaResponse);
                const res = await fetch('/api/account/sendtoken', {
                    method: 'POST',
                    body: formData,
                });
            },
            async submitCode(e) {
                const res = await Fetch.Req('account/login', {
                    method: 'POST',
                    body: JSON.stringify({
                        email: this.email,
                        token: this.code,
                        rememberMe: this.rememberMe,
                        isRegistering: this.isRegistering,
                        ...this.user,
                    }),
                }, false);

                if (res.status === 200) {
                    const resJson = await res.json();
                    localStorage.setItem('User.Id', resJson.id);
                    localStorage.setItem('User.UserName', resJson.userName);
                    localStorage.setItem('User.Email', resJson.email);
                    window.location.reload();
                }

                if (res.status === 202) {
                    this.isRegistering = true;
                }

                if ((res.status === 400 && this.isRegistering) || res.status === 500) {
                    this.$store.commit('addAlert',
                        { type: 'warning', text: await res.json() });
                }

                if (res.status === 400 && !this.isRegistering) {
                    this.$store.commit('addAlert',
                        { type: 'warning', text: 'Already logged-in.' });
                }

                if (res.status === 401) {
                    this.$store.commit('addAlert',
                        { type: 'warning', text: 'Code invalid or expired. Please try again.' });
                }
            },
        },
    });
</script>
<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
</style>
