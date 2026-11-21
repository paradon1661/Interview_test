<script setup>
import { RouterView, useRoute } from 'vue-router'

const route = useRoute()
</script>

<template>
  <div class="mx-auto my-4 max-w-4xl px-4 sm:my-8">
    <div class="border border-line bg-surface">
      <header class="border-b border-line bg-header px-4 py-1.5 text-center text-sm">
        <h1>{{ route.meta.title }}</h1>
      </header>
      <main class="px-5 pt-4 pb-10 sm:px-10">
        <RouterView />
      </main>
    </div>
  </div>
</template>
