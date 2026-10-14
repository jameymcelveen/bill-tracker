import { describe, expect, it } from 'vitest'
import { toLocalIsoDate } from './localDate'

describe('toLocalIsoDate', () => {
  it('pads single-digit months and days', () => {
    expect(toLocalIsoDate(new Date(2026, 0, 5, 12, 0))).toBe('2026-01-05')
  })

  it('uses the local calendar day, even late at night', () => {
    expect(toLocalIsoDate(new Date(2026, 9, 10, 23, 59))).toBe('2026-10-10')
  })
})
