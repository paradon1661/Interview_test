<script setup>
defineProps({
  title: { type: String, required: true },
  message: { type: String, default: '' },
})
</script>

<template>
  <div class="py-10 text-center" role="status">
    <p class="font-medium">{{ title }}</p>
    <p v-if="message" class="mt-1 text-sm text-ink-secondary">{{ message }}</p>
    <div v-if="$slots.default" class="mt-4 flex justify-center"><slot /></div>
  </div>
</template>
