<script setup>
import { TextInput } from '@/components/ui'
import QuestionItem from './QuestionItem.vue'

/**
 * The answer sheet, shared by IT 10-1 (editable) and IT 10-2 (`readonly`, showing what was submitted).
 * `answers` is `{ [questionId]: choiceId }`.
 */
const fullName = defineModel('fullName', { type: String, default: '' })
const answers = defineModel('answers', { type: Object, default: () => ({}) })
const props = defineProps({
  questions: { type: Array, required: true },
  readonly: { type: Boolean, default: false },
  nameError: { type: String, default: '' },
})

const setAnswer = (questionId, choiceId) => {
  answers.value = { ...answers.value, [questionId]: choiceId }
}
</script>

<template>
  <div>
    <TextInput
      id="fullName"
      v-model="fullName"
      label="ชื่อ-สกุล"
      :maxlength="200"
      :disabled="props.readonly"
      :error="props.nameError"
      class="mb-2"
    />

    <QuestionItem
      v-for="question in props.questions"
      :key="question.id"
      :question="question"
      :model-value="answers[question.id] ?? null"
      :disabled="props.readonly"
      @update:model-value="setAnswer(question.id, $event)"
    />
  </div>
</template>
