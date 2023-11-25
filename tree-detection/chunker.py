import pyproj

inProj = pyproj.Proj(init='epsg:2180')
outProj = pyproj.Proj(init='epsg:4326')


def chunk_data(data):
    chunk_size = 1

    data['x'] = data['x'].apply(lambda x: int(x / chunk_size) * chunk_size)
    data['y'] = data['y'].apply(lambda y: int(y / chunk_size) * chunk_size)

    data = data.groupby(['x', 'y']).size().reset_index(name='count')
    data = data[data['count'] > 2]

    data['lon'], data['lat'] = pyproj.transform(inProj, outProj, data['x'].values, data['y'].values)

    return data
