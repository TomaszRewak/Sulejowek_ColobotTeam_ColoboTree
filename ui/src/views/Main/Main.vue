<template>
  <div>
    <div v-show="state.isLoading" id="loader" class="fixed z-20 top-0 left-0 w-full h-full flex items-center justify-center bg-black bg-opacity-50 z-50">
        <div class="animate-spin rounded-full h-16 w-16 border-t-8 border-white-500"></div>
    </div>
    <div class="w-screen max-h-screen h-screen z-10" id="map"></div>
    <div class="absolute top-5 right-5 z-20">
      <ct-text-input
        v-model="state.searchInput"
        placeholder="Wpisz numer działki..."
        :suggestions="state.searchSuggestions"
        :leftIcon="true"
        searchKey="id"
        displayKey="id"
      >
      </ct-text-input>

      <ct-card :visible="state.picked.id" @close="onClose" class="mb-5">
        <template #header>
          <div class="flex justify-center items-center">
            <img class="w-6 h-6" src="../../assets/icons/location-pin.png" />
            {{ state.picked.id }}
          </div>
        </template>
        <div class="flex items-center py-4 border-b-2">
          <img class="w-6 h-6" src="../../assets/icons/co2.png" />
          <p class="ml-4 text-sm">Aktualny wpływ CO2: <span class="font-semibold">{{ state.plotData.co2Sequestration ?? '12'}}</span></p>
        </div>
        <div class="flex items-center py-4 border-b-2">
          <img class="w-6 h-6" src="../../assets/icons/nature.png" />
          <p class="ml-4 text-sm">
            Aktualny stopień zalesienia: <span class="font-semibold">{{ state.plotData.co2Sequestration ?? '30%' }}</span>
          </p>
        </div>
        <div class="flex items-center py-4 border-b-2">
          <img class="w-6 h-6" src="../../assets/icons/temperature.png" />
          <p class="ml-4 text-sm">Temperatura: <span class="font-semibold">30</span></p>
        </div>
        <div class="flex items-center pt-4">
          <img class="w-6 h-6" src="../../assets/icons/bird.png" />
          <p class="ml-4 text-sm">Okres lęgowy ptaków</p>
        </div>
      </ct-card>

      <ct-card :visible="state.pickedTree.id" @close="onClose">
        <template #header>
          <div class="flex justify-center items-center">
            <img class="w-6 h-6" src="../../assets/icons/tree.png" />
            {{ state.pickedTree.treeId }}
          </div>
        </template>
        <div class="flex items-center py-4 border-b-2">
          <img class="w-6 h-6" src="../../assets/icons/co2.png" />
          <p class="ml-4 text-sm">
            Stopień wpływu CO2: 10<span class="text-red-500 font-semibold">(-2)</span>
          </p>
        </div>
        <div class="flex items-center py-4 border-b-2">
          <img class="w-6 h-6" src="../../assets/icons/temperature.png" />
          <p class="ml-4 text-sm">
            Temperatura: <span class="text-red-500 font-semibold">35</span>
          </p>
        </div>
        <div class="flex items-center py-4">
          <img class="w-6 h-6" src="../../assets/icons/nature.png" />
          <p class="ml-4 text-sm">
            Stopień zalesienia po wycince: 27%<span class="text-red-500 font-semibold">(-3%)</span>
          </p>
        </div>
      </ct-card>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, reactive, watch } from 'vue'
import 'leaflet/dist/leaflet.css'
import L, { type LatLngExpression, type MapOptions } from 'leaflet'
import plotsData from '../../../../resources/plots.json'
import type { Plot, PlotData } from '@/types/plot.type'
import type { Chunk } from '@/types/chunk.type'
import { createDebouncedFunction, getRandomColor } from '../../utils'
import { postGetChunks, postGetPlot } from '../../services/chunk.service'


const map = ref<L.Map | null>(null)
const center: LatLngExpression = [52.24645266846282, 21.281910094983104]
const zoom = ref(15)
const plots: Plot[] = plotsData.plots as Plot[]

const state = reactive({
  chunks: [] as Chunk[],
  chunksLayer: L.layerGroup(),
  plotsLayer: L.layerGroup(),
  picked: {} as Plot,
  plotData: {} as PlotData,
  pickedTree: {} as Chunk,
  searchInput: {} as Plot,
  searchSuggestions: plots,
  isLoading: true
})

onMounted(() => {
  const mapOptions: MapOptions = {
    preferCanvas: true,
    maxZoom: 18,
    zoomAnimation: false,
    zoomAnimationThreshold: 4
  }
  map.value = L.map('map', mapOptions).setView(center, zoom.value)
  map.value.attributionControl.setPrefix(
    '<a href="https://leafletjs.com" title="A JS library for interactive maps">Leaflet</a> | ColobotTree | HackTheClimate 2023'
  )

  L.tileLayer('https://tiles.stadiamaps.com/tiles/osm_bright/{z}/{x}/{y}{r}.png').addTo(
    map.value as L.Map
  )
  map.value!.on('zoomend', handleZoomEnd)
  loadPlots()
  state.isLoading = false
})

const getDatasetChunks = async () => {
    state.isLoading = true 
  if (map.value instanceof L.Map) {
    const bounds = map.value.getBounds()
    const result = await postGetChunks(bounds.getNorthEast(), bounds.getSouthWest())
      state.chunks = result.chunks
      addTrees(state.chunks)
  }
    state.isLoading = false
  }

const clearChunks = () => {
  state.chunks = []
  map.value?.removeLayer(state.chunksLayer as L.LayerGroup)
}

const clearPlots = () => {
  map.value?.removeLayer(state.plotsLayer as L.LayerGroup)
}

const loadPlots = () => {
  addPlots(plots)
}

const handleZoomEnd = createDebouncedFunction(() => {
  getDatasetChunks()
}, 500)

const createSquare = (chunk: Chunk) => {
  const squareCoordinates = [
    [chunk.upperLeftVertex4326.coordinates[1], chunk.upperLeftVertex4326.coordinates[0]],
    [chunk.bottomRightVertex4326.coordinates[1], chunk.upperLeftVertex4326.coordinates[0]],
    [chunk.bottomRightVertex4326.coordinates[1], chunk.bottomRightVertex4326.coordinates[0]],
    [chunk.upperLeftVertex4326.coordinates[1], chunk.bottomRightVertex4326.coordinates[0]]
  ] as LatLngExpression[]

  return L.polygon(squareCoordinates, { color: getRandomColor(chunk.treeId ?? chunk.id) })
}

const addTrees = (chunks: Chunk[]) => {
  clearChunks()
  chunks.forEach((chunk) => {
    const square = createSquare(chunk)
    square.on('click', () => handleSquareClick(chunk))
    state.chunksLayer.addLayer(square)
  })
  state.chunksLayer.addTo(map.value as L.Map)
}

const addPlots = (plots: Plot[]) => {
  clearPlots()
  plots.forEach((plot: Plot) => {
    const polygon = L.polygon(plot.polygon, { color: '#7babca', fill: false, weight: 1 })
    polygon.on('click', () => handlePolygonClick(plot))
    polygon.on('mouseover', function (event) {
      event.target.setStyle({
        color: '#7bcac2',
        fill: true
      })
    })

    polygon.on('mouseout', function (event) {
      event.target.setStyle({
        color: '#7babca',
        fill: false
      })
    })
    state.plotsLayer.addLayer(polygon)
  })
  state.plotsLayer.addTo(map.value as L.Map)
}

const onClose = () => {
  state.picked = {} as Plot
}

const handleSquareClick = (chunk: any) => {
  state.pickedTree = chunk
}

const handlePolygonClick = async (plot: Plot) => {
  state.picked = plot
    const northEast = {lat: (plot.polygon[1] as any)[0], lng: (plot.polygon[1] as any)[1]}
    const southWest = {lat: (plot.polygon[2] as any)[0], lng: (plot.polygon[2] as any)[1]}
  const result = await postGetPlot(northEast, southWest )
  state.plotData = result

}

watch(
  () => state.searchInput,
  (newInput: any) => {
    if (newInput !== '') {
      const plot = plots.find((plot) => plot.id === newInput)
      if (plot) {
        map.value!.setView(plot.polygon[0], 18)
      }
    }
  }
)
</script>
