<script setup>
/** A choice: grey dot, black when selected (as in the IT 10-1 / 10-2 mockup). Native radio underneath for a11y. */
const model = defineModel({ type: [Number, String, null], default: null })
const props = defineProps({
  name: { type: String, required: true },
  value: { type: [Number, String], required: true },
  label: { type: String, required: true },
  disabled: { type: Boolean, default: false },
})
</script>

<template>
  <label class="flex items-center gap-2.5 py-0.5 text-sm has-[:disabled]:cursor-default" :class="!props.disabled && 'cursor-pointer'">
    <input v-model="model" type="radio" class="peer sr-only" :name="props.name" :value="props.value" :disabled="props.disabled" />
    <span
      aria-hidden="true"
      class="size-[18px] shrink-0 rounded-full border border-radio-border bg-radio transition-colors peer-checked:border-radio-checked peer-checked:bg-radio-checked peer-focus-visible:outline-2 peer-focus-visible:outline-offset-2 peer-focus-visible:outline-primary"
    />
    <span>{{ props.label }}</span>
  </label>
</template>
