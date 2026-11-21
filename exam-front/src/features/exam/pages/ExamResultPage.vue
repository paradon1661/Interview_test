<script setup>
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { BaseButton, FullPageMessage } from '@/components/ui'
import { ROUTES } from '@/config/routes'
import ExamSheet from '../components/ExamSheet.vue'
import { toAnswerMap } from '../exam'
import { useExamResult } from '../composables/useExamResult'
import { useQuestions } from '../composables/useQuestions'

/** IT 10-2: the submitted sheet (read-only) and the score, loaded from the saved result so a refresh keeps it. */
const props = defineProps({ id: { type: String, required: true } })

const router = useRouter()
const questionsQuery = useQuestions()
const resultQuery = useExamResult(() => props.id)

const isPending = computed(() => questionsQuery.isPending.value || resultQuery.isPending.value)
const loadError = computed(() => questionsQuery.error.value ?? resultQuery.error.value)
const result = computed(() => resultQuery.data.value)
const answers = computed(() => toAnswerMap(result.value?.answers))

/** สอบอีกครั้ง: back to IT 10-1, which always opens with a cleared sheet. */
const retake = () => router.push(ROUTES.EXAM)
</script>

<template>
  <FullPageMessage v-if="isPending" title="กำลังโหลดผลสอบ..." />
  <FullPageMessage v-else-if="loadError" title="เปิดผลสอบไม่สำเร็จ" :message="loadError.message">
    <BaseButton @click="retake">เริ่มทำข้อสอบ</BaseButton>
  </FullPageMessage>

  <template v-else>
    <ExamSheet :full-name="result.fullName" :answers="answers" :questions="questionsQuery.data.value" readonly />

    <div class="flex flex-wrap items-center gap-x-10 gap-y-3">
      <BaseButton @click="retake">สอบอีกครั้ง</BaseButton>
      <p class="text-sm" role="status">คุณ {{ result.fullName }} สอบได้คะแนน : {{ result.score }}/{{ result.totalQuestions }}</p>
    </div>
  </template>
</template>
