import { useMutation, useQueryClient } from '@tanstack/vue-query'
import { examApi } from '../api/examApi'
import { examKeys } from './queryKeys'

/** Submit → the server grades and saves. The answer is cached so the result page opens without another request. */
export function useSubmitExam() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: examApi.submit,
    onSuccess: (result) => queryClient.setQueryData(examKeys.result(result.id), result),
  })
}
