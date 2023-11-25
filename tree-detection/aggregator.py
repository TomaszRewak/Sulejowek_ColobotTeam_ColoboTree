import pyproj

inProj = pyproj.Proj(init='epsg:2180')
outProj = pyproj.Proj(init='epsg:4326')

def aggregate_trees(data, resolution):
    data = data.copy()

    data['x_1'] = data['x_1'].apply(lambda x: int(x / resolution) * resolution)
    data['y_1'] = data['y_1'].apply(lambda y: int(y / resolution) * resolution)

    data = data.groupby(['x_1', 'y_1']).size().reset_index(name='count')

    data['x_2'] = data['x_1'].apply(lambda x: x + resolution)
    data['y_2'] = data['y_1'].apply(lambda y: y + resolution)

    data['lon_1'], data['lat_1'] = pyproj.transform(inProj, outProj, data['x_1'].values, data['y_1'].values)
    data['lon_2'], data['lat_2'] = pyproj.transform(inProj, outProj, data['x_2'].values, data['y_2'].values)

    data['coverage'] = data['count'] / (resolution * resolution)
    data['tree_id'] = None
    
    data = data.drop(columns=['count'])

    return data
