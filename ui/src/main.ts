import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import Card from './components/Card.vue'
import TextInput from './components/TextInput.vue'
import Button from './components/Button.vue'

const app = createApp(App)

app.component('ct-card', Card)
app.component('ct-text-input', TextInput)
app.component('ct-button', Button)

app.use(createPinia())
app.use(router)

app.mount('#app')
