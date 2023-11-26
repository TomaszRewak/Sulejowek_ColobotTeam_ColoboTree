<!-- Button.vue -->
<template>
  <button :class="buttonClass" @click="handleClick">
    <slot></slot>
  </button>
</template>

<script lang="ts">
import { ref, defineProps, defineEmits, computed } from 'vue';

export default {
  props: {
    variant: {
      type: String,
      default: 'primary',
    },
  },
  setup(props, { emit }) {
    const buttonClass = computed(() => `button ${props.variant}`);

    const handleClick = () => {
      emit('click');
    };

    return {
      buttonClass,
      handleClick,
    };
  },
};
</script>

<style scoped>
.button {
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 14px;
}

.primary {
  background-color: #3498db;
  color: #fff;
}

.secondary {
  background-color: #95a5a6;
  color: #fff;
}

/* Add more styles for different variants as needed */
</style>
