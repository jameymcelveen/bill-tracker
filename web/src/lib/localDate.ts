// The calendar date where the user is, as YYYY-MM-DD.
// Never use toISOString() for this: it converts to UTC and can return tomorrow or yesterday.
export function toLocalIsoDate(moment: Date): string {
  const year = moment.getFullYear()
  const month = String(moment.getMonth() + 1).padStart(2, '0')
  const day = String(moment.getDate()).padStart(2, '0')
  return `${year}-${month}-${day}`
}
