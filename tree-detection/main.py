import matplotlib.pyplot as plt
from db import save_to_db
from importer import import_data
from chunker import chunk_data
from clusterer import cluster_trees
import numpy as np


def segmentation_cmap():
    vals = np.linspace(0, 1, 256)
    np.random.shuffle(vals)
    return plt.cm.colors.ListedColormap(plt.cm.CMRmap(vals))


if __name__ == '__main__':
    trees_df = import_data()
    trees_chunked_df = chunk_data(trees_df)
    trees_clusters_df = cluster_trees(trees_chunked_df)

    save_to_db(trees_clusters_df)

    # plot
    plt.scatter(trees_clusters_df['x'], trees_clusters_df['y'], s=1, c=trees_clusters_df['cluster'], cmap=segmentation_cmap())
    plt.show()
