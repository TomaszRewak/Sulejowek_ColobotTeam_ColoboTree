import axios from 'axios';

export const getChunks = async (northEast: L.LatLng, southWest: L.LatLng) => {
    try {
        const response = await axios.get(`?${northEast}?${southWest}`);
        return response.data;

    }catch(error) {
        console.error('Error fetching data:', error)
    }
}