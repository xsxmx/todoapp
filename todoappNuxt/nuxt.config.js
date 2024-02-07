export default {
  ssr: false,

  head: {
    title: 'To:do',
    htmlAttrs: {
      lang: 'en'
    },
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      { hid: 'description', name: 'description', content: '' },
      { name: 'format-detection', content: 'telephone=no' }
    ],
    link: [
      { rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' },
      { rel: 'stylesheet', href: 'https://cdn.jsdelivr.net/npm/bootstrap-icons@1.5.0/font/bootstrap-icons.css' }
    ]
  },

  css: [
    "~/styles/index.scss",
    "~/styles/layout.scss",
    "~/styles/note.scss",
    "~/styles/variables.scss",
    "~/styles/media.scss",
    "~/styles/components.scss",
    "~/styles/animations.scss"
  ],

  plugins: [],

  components: true,

  buildModules: [
    '@nuxt/typescript-build',
  ],

  modules: [
    'bootstrap-vue/nuxt',
    '@nuxtjs/axios',
    '@nuxtjs/proxy'
  ],

  axios: {
    proxy: true 
  },

  proxy: {
    '/api/': { target: 'http://localhost:5087', pathRewrite: {'^/api/': ''} },
  },

  build: {}
}
