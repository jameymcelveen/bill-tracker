// The one place the app talks HTTP. Response shapes come from ./generated/schema.d.ts,
// which is generated from contracts/openapi.yaml and never edited by hand.
import createClient from 'openapi-fetch'
import type { paths } from './generated/schema'

export const api = createClient<paths>({
  baseUrl: import.meta.env.VITE_API_URL ?? '',
})
