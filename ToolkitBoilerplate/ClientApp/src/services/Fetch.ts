import store from '@/store';
import 'nprogress/nprogress.css';
import NProgress from 'nprogress';

const baseUri = process.env.NODE_ENV === 'development' ? 'http://localhost:8080/api' : 'https://MY_SITE.com/api';

export default class Fetch {

    public static async Create(entityPath: string, entityModel: object): Promise<Response> {
        return await Fetch.Req(entityPath, {
            method: 'POST',
            body: JSON.stringify(entityModel),
        });
    }

    public static async Read(entityPath: string, params?: object, id?: number): Promise<Response> {
        let paramsQs = '';
        if (params !== undefined) {
            paramsQs = new URLSearchParams(Object.entries(params)).toString();
        }
        if (id === undefined) {
            return await Fetch.Req(`${entityPath}?${paramsQs}`);
        } else {
            return await Fetch.Req(`${entityPath}/${id}?${paramsQs}`);
        }
    }

    public static async Update(entityPath: string, entityModel: object, id: number): Promise<Response> {
        return await Fetch.Req(`${entityPath}/${id}`, {
            method: 'PUT',
            body: JSON.stringify(entityModel),
        });
    }

    public static async Delete(entityPath: string, id: number): Promise<Response> {
        return await Fetch.Req(`${entityPath}/${id}`, {
            method: 'DELETE',
        });
    }

    public static async Req(input: string,
        init: RequestInit = {},
        enableAlerts: boolean = true): Promise<Response> {

        if (enableAlerts && !store.getters.isAuthenticated && ['POST', 'PUT', 'DELETE'].includes(init.method || "PUT")) {
            store.commit('toggleLoginModal');
            this.unauthorizedAlert();
            return new Response();
        }

        NProgress.start();

        input = Fetch.addBaseUri(input);

        init.credentials = 'include'; // Incldue cookies
        init.headers = {
            'content-type': 'application/json',
        };

        const res = await fetch(input, init);

        NProgress.done();

        if (enableAlerts) {
            if (res.status === 401) {
                this.unauthorizedAlert();
            }

            if (res.status >= 300) {
                let type = 'warning';
                if (res.status >= 500) {
                    type = 'danger';
                }
                let body;
                try {
                    body = await res.json();
                } catch (e) {
                    body = res.statusText;
                }
                store.commit('addAlert', { type, text: body });
            }
        }
        return res;
    }

    private static unauthorizedAlert() {
        store.commit('addAlert', { type: 'warning', text: 'You have to be logged-in to do that.' });
    }

    private static addBaseUri(input: string) {
        if (!input.startsWith('/')) {
            input = `/${input}`;
        }
        input = `${baseUri}${input}`;
        return input;
    }
}
