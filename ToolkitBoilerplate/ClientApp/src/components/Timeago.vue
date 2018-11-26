<template>
    <span class="timeago">{{result}}</span>
</template>

<script lang="ts">
    import Vue from 'vue';
    import timeago from 'timeago.js';

    export default Vue.extend({
        name: 'Timeago',
        props: {
            showAgo: Boolean,
            date: String,
        },
        data() {
            return {
                timeago: timeago(),
            };
        },
        computed: {
            result: function() {
                return this.timeago.format(this.date)
                    .replace(' ago', this.showAgo ? ' ago' : '')
                    .replace('seconds', 's')
                    .replace('minute', 'min')
                    .replace('hour', 'hr')
                    .replace('year', 'yr')
                    .trim();
            }
        }
    });
</script>

<style scoped lang="scss">
</style>
