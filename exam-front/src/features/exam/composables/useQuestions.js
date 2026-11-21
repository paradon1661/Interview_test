import { useQuery } from '@tanstack/vue-query'
import { examApi } from '../api/examApi'
import { examKeys } from './queryKeys'

/** Questions rarely change during a sitting, so both pages share one cached copy. */
export const useQuestions = () =>
  useQuery({
    queryKey: examKeys.questions,
    queryFn: examApi.questions,
    staleTime: 5 * 60_000,
  })
