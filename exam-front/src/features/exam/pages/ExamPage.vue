<script setup>
import { computed, ref, watch } from 'vue'
import { useRouter } from 'vue-router'
import { BaseButton, FullPageMessage } from '@/components/ui'
import { ROUTES } from '@/config/routes'
import ExamSheet from '../components/ExamSheet.vue'
import { useQuestions } from '../composables/useQuestions'
import { useSubmitExam } from '../composables/useSubmitExam'
import { countUnanswered, toAnswerList } from '../exam'

/** IT 10-1: กรอกชื่อ เลือกคำตอบ แล้วส่งข้อสอบ. Every visit starts with an empty sheet. */
const router = useRouter()
const { data: questions, isPending, isError, error, refetch } = useQuestions()
const submit = useSubmitExam()

const fullName = ref('')
const answers = ref({})
const nameError = ref('')
watch(fullName, () => (nameError.value = ''))

const submitError = computed(() => {
  const e = submit.error.value
  return e && !e.details?.FullName ? e.message : ''
})

async function onSubmit() {
  nameError.value = fullName.value.trim() ? '' : 'กรุณากรอกชื่อ-สกุล'
  if (nameError.value) return

  const unanswered = countUnanswered(questions.value, answers.value)
  if (unanswered > 0 && !window.confirm(`ยังไม่ได้ตอบ ${unanswered} ข้อ ต้องการส่งข้อสอบเลยหรือไม่?`)) return

  try {
    const result = await submit.mutateAsync({ fullName: fullName.value.trim(), answers: toAnswerList(answers.value) })
    router.push(ROUTES.EXAM_RESULT(result.id))
  } catch (e) {
    nameError.value = e.details?.FullName?.[0] ?? ''
  }
}
</script>

<template>
  <FullPageMessage v-if="isPending" title="กำลังโหลดข้อสอบ..." />
  <FullPageMessage v-else-if="isError" title="โหลดข้อสอบไม่สำเร็จ" :message="error.message">
    <BaseButton variant="secondary" @click="refetch()">ลองอีกครั้ง</BaseButton>
  </FullPageMessage>

  <form v-else novalidate @submit.prevent="onSubmit">
    <ExamSheet v-model:full-name="fullName" v-model:answers="answers" :questions="questions" :name-error="nameError" />

    <p v-if="submitError" class="mb-3 bg-danger-light px-3 py-2 text-sm text-danger" role="alert">{{ submitError }}</p>

    <BaseButton type="submit" :loading="submit.isPending.value">ส่งข้อสอบ</BaseButton>
  </form>
</template>
