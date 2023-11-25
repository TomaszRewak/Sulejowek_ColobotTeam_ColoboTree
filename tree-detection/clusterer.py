import pandas as pd
from sklearn.cluster import AgglomerativeClustering
from sklearn.preprocessing import StandardScaler


def cluster_trees(data):
    # split data into 100x100 blocks
    chunk_size = 100
    data['chunk_x'] = data['x_1'].apply(lambda x: int(x / chunk_size) * chunk_size)
    data['chunk_y'] = data['y_1'].apply(lambda y: int(y / chunk_size) * chunk_size)

    result = pd.DataFrame()
    chunk_offset = 0

    for chunk in data.groupby(['chunk_x', 'chunk_y']):
        # cluster using AgglomerativeClustering
        chunk_df = chunk[1]

        # scale data
        scaler = StandardScaler()
        scaled_data = scaler.fit_transform(chunk_df[['x_1', 'y_1']])

        # cluster
        clusterer = AgglomerativeClustering(n_clusters=None, distance_threshold=0.5)
        clusterer.fit(scaled_data)

        # add results
        chunk_df['tree_id'] = clusterer.labels_
        chunk_df['tree_id'] = chunk_df['tree_id'] + chunk_offset

        result = pd.concat([result, chunk_df])
        chunk_offset = max(chunk_df['tree_id'].max(), 0) + 1

    result = result.drop(columns=['chunk_x', 'chunk_y'])

    return result
