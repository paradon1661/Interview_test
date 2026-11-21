<script setup>
import { cn } from '@/lib/cn'

const model = defineModel({ type: String, default: '' })
const props = defineProps({
  id: { type: String, required: true },
  label: { type: String, required: true },
  error: { type: String, default: '' },
  disabled: { type: Boolean, default: false },
  maxlength: { type: Number, default: undefined },
})
</script>

<template>
  <div>
    <div class="flex flex-wrap items-center gap-1.5">
      <label :for="props.id" class="text-sm">{{ props.label }}</label>
      <input
        :id="props.id"
        v-model="model"
        type="text"
        autocomplete="name"
        :maxlength="props.maxlength"
        :disabled="props.disabled"
        :aria-invalid="!!props.error || undefined"
        :aria-describedby="props.error ? `${props.id}-error` : undefined"
        :class="
          cn(
            'h-7 w-80 max-w-full border border-line bg-surface px-1.5 text-sm outline-none focus:ring-2 focus:ring-primary/40 disabled:text-ink',
            props.error && 'border-danger',
          )
        "
      />
    </div>
    <p v-if="props.error" :id="`${props.id}-error`" class="mt-1 text-xs text-danger">{{ props.error }}</p>
  </div>
</template>
