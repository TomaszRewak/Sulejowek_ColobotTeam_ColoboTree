export type Shape = {
  type: string
  coordinates: number[]
}

export type Chunk = {
  id: number
  upperLeftVertex4326: Shape
  bottomRightVertex4326: Shape
  treeId: number
}

export type Dataset = {
  totalArea: number
  treeCoverage: number
  chunks: Chunk[]
}
