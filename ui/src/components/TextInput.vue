<template>
  <div class="mb-4 relative">
    <label class="block text-gray-700 text-sm font-bold mb-1">{{ title }}</label>

    <div v-if="leftIcon" class="absolute inset-y-0 left-0 flex items-center pl-2">
      <img class="w-4 h-4" src="../assets/icons/loupe.png" />
    </div>

    <input
      v-model="inputValue"
      :placeholder="placeholder"
      @input="handleInput"
      @focus="showSuggestions = true"
      @blur="showSuggestions = false"
      class="w-full pl-8 pr-12 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring focus:border-blue-500"
    />

    <div v-if="inputValue" class="absolute inset-y-0 right-0 flex items-center pr-2 cursor-pointer">
      <p @click="clearInput">X</p>
    </div>

    <transition name="fade">
      <ul
        v-if="showSuggestions && filteredSuggestions.length"
        class="absolute z-10 mt-2 w-full bg-white border border-gray-300 rounded-md shadow-md max-h-[32rem] overflow-auto"
      >
        <li
          v-for="suggestion in filteredSuggestions"
          :key="suggestion[displayKey]"
          @click="selectSuggestion(suggestion)"
          class="py-2 px-4 cursor-pointer hover:bg-gray-100 w-full"
        >
          {{ suggestion[props.displayKey] }}
        </li>
      </ul>
    </transition>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'

const props = defineProps({
  modelValue: { type: Object, required: true },
  placeholder: String,
  title: String,
  suggestions: { type: Array, required: true },
  searchKey: { type: String, required: true },
  displayKey: { type: String, required: true },
  leftIcon: Boolean
})

const emits = defineEmits(['update:modelValue'])

const inputValue = ref(props.modelValue[props.displayKey])
const showSuggestions = ref(false)

const handleInput = () => {
  emits('update:modelValue', inputValue.value)
}

const filteredSuggestions = computed(() => {
  if (inputValue.value) {
    const inputLowerCase = inputValue.value.toLowerCase()
    return props.suggestions.filter((suggestion: any) =>
      suggestion[props.searchKey].toLowerCase().includes(inputLowerCase)
    )
  }
  return props.suggestions
})

const selectSuggestion = (suggestion: any) => {
  inputValue.value = suggestion[props.displayKey]
  showSuggestions.value = false
  emits('update:modelValue', inputValue.value)
}

const clearInput = () => {
  inputValue.value = {}
  emits('update:modelValue', {})
}
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s;
}
.fade-enter,
.fade-leave-to {
  opacity: 0;
}
</style>
