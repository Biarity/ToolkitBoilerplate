import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
    state: {
        alerts: new Array,
        grecaptchaRendered: false,
        showLoginModal: false,
        showFlagModal: false,
        flagType: '',
        flagId: -1,
        flagLocals: new Array
    },
    mutations: {
        addAlert(state, { type, text }) {
            let tagline = '';
            switch (type) {
                case 'success':
                    tagline = '';
                    break;
                case 'info':
                    tagline = '';
                    break;
                case 'warning':
                    tagline = 'Warning!';
                    break;
                case 'error':
                    type = 'danger';
                    tagline = 'Error!';
                    break;
            }

            const id = Date.now();
            state.alerts.push({
                type: type,
                text: text,
                tagline,
                id: id,
            });

            const _this = this;
            setTimeout(() => {
                _this.commit('removeAlert' as any, id);
            }, 10000);

            // Display max 1 error, can change
            if (state.alerts.length > 3) {
                state.alerts.shift();
            }
        },
        removeAlert(state, alertId) {
            state.alerts.filter((a: any) => {
                return a.id === alertId;
            }).forEach((a: any, i: any) => {
                state.alerts.splice(i, 1);
            });
        },
        grecaptchaRendered(state) {
            state.grecaptchaRendered = true;
        },
        toggleLoginModal(state) {
            state.showLoginModal = !state.showLoginModal;
        },
        toggleFlagModal(state, payload) {
            state.showFlagModal = !state.showFlagModal;

            if (payload !== undefined) {
                state.flagType = payload.type;
                state.flagId = payload.id;
            }
        },
        addFlagLocal(state) {
            state.flagLocals.push(state.flagId);
            state.flagType = '';
            state.flagId = -1;
        }
    },
    actions: {

    },
    getters: {
        isAuthenticated(state) {
            return document.cookie.indexOf('User=') !== -1;
        },
        userName(state) {
            const start = document.cookie.split('User=')[1];
            if (start && start.includes(';')) {
                return start.substr(0, start.indexOf(';'));
            } else {
                return start;
            }
        },

    }
})
