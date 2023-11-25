import matplotlib.pyplot as plt
from db import save_to_db
from importer import import_data
from chunker import chunk_data


if __name__ == '__main__':
    trees_df = import_data()
    trees_chunked_df = chunk_data(trees_df)

    #save_to_db(trees_chunked_df)

    print('Chunked Trees:', len(trees_chunked_df))

    # plot
    plt.scatter(trees_chunked_df['x'], trees_chunked_df['y'], s=1)
    plt.show()
