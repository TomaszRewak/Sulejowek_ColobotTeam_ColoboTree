import pyproj

inProj = pyproj.Proj(init='epsg:2180')
outProj = pyproj.Proj(init='epsg:4326')


def chunk_data(data):
    chunk_size = 1

    data['x_1'] = data['x'].apply(lambda x: int(x / chunk_size) * chunk_size)
    data['y_1'] = data['y'].apply(lambda y: int(y / chunk_size) * chunk_size)

    data = data.groupby(['x_1', 'y_1']).size().reset_index(name='count')
    data = data[data['count'] > 2]

    data['x_2'] = data['x_1'].apply(lambda x: x + chunk_size)
    data['y_2'] = data['y_1'].apply(lambda y: y + chunk_size)

    data['lon_1'], data['lat_1'] = pyproj.transform(inProj, outProj, data['x_1'].values, data['y_1'].values)
    data['lon_2'], data['lat_2'] = pyproj.transform(inProj, outProj, data['x_2'].values, data['y_2'].values)

    data = data.drop(columns=['count'])

    data['coverage'] = 1.0
    data['tree_id'] = None

    return data
