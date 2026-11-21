export class ApiError extends Error {
  /** `details`: field → messages, e.g. `{ FullName: ["กรุณากรอกชื่อ-สกุล"] }` for validation_failed. */
  constructor(status, code, message, details = null) {
    super(message)
    this.name = 'ApiError'
    this.status = status
    this.code = code
    this.details = details
  }
}

/**
 * The BE wraps every JSON response: `{ success: true, data }` or `{ success: false, error: { code, message } }`.
 * Returns `data`, so callers see the payload only.
 */
export function unwrap(envelope) {
  return envelope?.data ?? null
}

/**
 * The only way the FE talks to the BE. Relative /api paths (the Vite proxy / nginx forwards them).
 *
 * @param {string} path  must start with /api/
 * @param {object} [options]
 * @param {string} [options.method]
 * @param {unknown} [options.body]  sent as JSON
 * @param {AbortSignal} [options.signal]
 */
export async function apiFetch(path, { method = 'GET', body, signal } = {}) {
  if (!path.startsWith('/api/')) {
    throw new Error(`apiFetch only accepts relative /api/ paths, got "${path}"`)
  }

  const headers = { Accept: 'application/json' }
  if (body !== undefined) headers['Content-Type'] = 'application/json'

  let res
  try {
    res = await fetch(path, {
      method,
      headers,
      body: body === undefined ? undefined : JSON.stringify(body),
      credentials: 'same-origin',
      signal,
    })
  } catch (networkError) {
    if (networkError?.name === 'AbortError') throw networkError
    throw new ApiError(0, 'NETWORK_ERROR', 'ไม่สามารถเชื่อมต่อเซิร์ฟเวอร์ได้ กรุณาลองใหม่อีกครั้ง')
  }

  if (res.ok) {
    if (res.status === 204) return null
    return unwrap(await res.json())
  }

  const envelope = await res.json().catch(() => null)
  throw new ApiError(
    res.status,
    envelope?.error?.code ?? `HTTP_${res.status}`,
    envelope?.error?.message ?? `เกิดข้อผิดพลาด (${res.status})`,
    envelope?.error?.details ?? null,
  )
}
