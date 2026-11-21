<script setup>
import { RouterLink } from 'vue-router'
import { FullPageMessage } from '@/components/ui'
import { ROUTES } from '@/config/routes'
</script>

<template>
  <div class="mx-auto max-w-4xl px-4">
    <FullPageMessage title="ไม่พบหน้าที่ต้องการ">
      <RouterLink :to="ROUTES.EXAM" class="text-sm text-primary underline">กลับไปหน้าทำข้อสอบ</RouterLink>
    </FullPageMessage>
  </div>
</template>
