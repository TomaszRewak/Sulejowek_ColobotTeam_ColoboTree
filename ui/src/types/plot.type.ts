import type { LatLngExpression } from 'leaflet'

export type Plot = {
  id: string
  polygon: LatLngExpression[]
}

export type PlotData = {
    totalArea: number,
    treeCoverage: number,
    co2Sequestration: number
}
