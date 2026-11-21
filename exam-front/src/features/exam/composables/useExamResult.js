import { useQuery } from '@tanstack/vue-query'
import { computed, toValue } from 'vue'
import { examApi } from '../api/examApi'
import { examKeys } from './queryKeys'

/** A saved result never changes, so it is never refetched once loaded. */
export const useExamResult = (id) =>
  useQuery({
    queryKey: computed(() => examKeys.result(toValue(id))),
    queryFn: () => examApi.result(toValue(id)),
    staleTime: Infinity,
  })
