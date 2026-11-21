/** `{ [questionId]: choiceId }` → the API's `[{ questionId, choiceId }]`, unanswered questions left out. */
export const toAnswerList = (answers) =>
  Object.entries(answers)
    .filter(([, choiceId]) => choiceId != null)
    .map(([questionId, choiceId]) => ({ questionId: Number(questionId), choiceId }))

/** The API's answer list → `{ [questionId]: choiceId }` for the sheet. */
export const toAnswerMap = (list = []) => Object.fromEntries(list.map((a) => [a.questionId, a.choiceId]))

export const countUnanswered = (questions, answers) => questions.filter((q) => answers[q.id] == null).length
