import matplotlib.pyplot as plt
from db import save_to_db, clear_db
from importer import import_data
from chunker import chunk_data
from clusterer import cluster_trees
from aggregator import aggregate_trees
from plotter import plot_trees


if __name__ == '__main__':
    trees_df = import_data()
    trees_chunked_df = chunk_data(trees_df)
    trees_clusters_df = cluster_trees(trees_chunked_df)

    print("Number of tree points: ", len(trees_clusters_df))
    print("Number of unique clusters: ", trees_clusters_df['tree_id'].nunique())

    #trees_clusters_df.to_json('data.json', orient='records')

    clear_db()
    save_to_db(trees_clusters_df, 1)

    for resolution in [2, 5, 10, 20, 50]:
        aggregated_trees_df = aggregate_trees(trees_clusters_df, resolution)

        save_to_db(aggregated_trees_df, resolution)

    # plot
    plot_trees(trees_clusters_df, True)
    plot_trees(aggregate_trees(trees_clusters_df, 10), False)
