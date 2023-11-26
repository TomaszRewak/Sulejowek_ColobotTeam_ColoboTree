<template>
  <div
    v-if="state.isVisible"
    :class="['w-screen', `max-w-sm`, 'relative', 'bg-white', 'rounded-md', 'p-6', 'shadow-lg']"
  >
    <button v-if="closable" @click="closeCard" class="absolute top-0 right-0 p-2 cursor-pointer">
      X
    </button>
    <header class="text-xl font-semibold mb-2 text-center">
      <slot name="header"></slot>
    </header>
    <main class="text-gray-700 text-base">
      <slot></slot>
    </main>
    <footer class="flex justify-center">
      <slot name="footer"></slot>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { defineProps, onMounted, reactive, watch } from 'vue'

const props = defineProps({
  visible: Boolean,
  closable: {
    type: Boolean,
    default: true
  }
})
const emits = defineEmits(['close'])
const state = reactive({ isVisible: false })
onMounted(() => {
  state.isVisible = props.visible
})
watch(
  () => props.visible,
  (newVisible) => {
    if (newVisible !== state.isVisible) {
      state.isVisible = newVisible
    }
  }
)

const closeCard = () => {
  emits('close')
  state.isVisible = false
}
</script>
