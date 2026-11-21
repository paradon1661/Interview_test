<script setup>
import { RadioOption } from '@/components/ui'

/** One question. `v-model` is the chosen choice id: a radio group, so only one answer per question. */
const model = defineModel({ type: Number, default: null })
const props = defineProps({
  question: { type: Object, required: true },
  disabled: { type: Boolean, default: false },
})
</script>

<template>
  <fieldset class="mb-6">
    <legend class="mb-1.5 text-sm">{{ props.question.no }}. {{ props.question.questionText }}</legend>
    <RadioOption
      v-for="choice in props.question.choices"
      :key="choice.id"
      v-model="model"
      :name="`question-${props.question.id}`"
      :value="choice.id"
      :label="choice.choiceText"
      :disabled="props.disabled"
    />
  </fieldset>
</template>
