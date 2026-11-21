<script setup>
import { Loader2 } from 'lucide-vue-next'
import { cn } from '@/lib/cn'

const props = defineProps({
  variant: { type: String, default: 'primary' },
  type: { type: String, default: 'button' },
  loading: { type: Boolean, default: false },
  disabled: { type: Boolean, default: false },
  class: { type: [String, Array, Object], default: '' },
})

const variants = {
  primary: 'bg-primary text-white border border-primary-border hover:bg-primary-hover',
  secondary: 'bg-surface text-ink border border-line hover:bg-page',
}
</script>

<template>
  <button
    :type="props.type"
    :disabled="props.disabled || props.loading"
    :aria-busy="props.loading || undefined"
    :class="
      cn(
        'inline-flex h-8 shrink-0 items-center justify-center gap-2 px-5 text-sm whitespace-nowrap transition-colors disabled:cursor-not-allowed disabled:opacity-60',
        variants[props.variant],
        props.class,
      )
    "
  >
    <Loader2 v-if="props.loading" class="size-4 animate-spin" aria-hidden="true" />
    <slot />
  </button>
</template>
