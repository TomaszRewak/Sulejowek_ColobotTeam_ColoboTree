import axios from 'axios'

export const getChunks = async (northEast: L.LatLng, southWest: L.LatLng) => {
  try {
    const response = await axios.get(`?${northEast}?${southWest}`)
    return response.data
  } catch (error) {
    console.error('Error fetching data:', error)
  }
}

export const postGetChunks = async (northEast: L.LatLng, southWest: L.LatLng) => {
  try {
    const response = await axios.post(`https://localhost:7178/chunks`, 
    {
      yMin: southWest.lat,
      xMin: southWest.lng,
      yMax: northEast.lat,
      xMax: northEast.lng,

    })
    return response.data
  } catch (error) {
    console.error('Error fetching data:', error)
  }
}

export const postGetPlot = async (northEast: L.LatLng, southWest: L.LatLng) => {
  try {
    const response = await axios.post(`https://localhost:7178/plot`, 
    {
      yMin: southWest.lat,
      xMin: southWest.lng,
      yMax: northEast.lat,
      xMax: northEast.lng,

    })
    return response.data
  } catch (error) {
    console.error('Error fetching data:', error)
  }
}