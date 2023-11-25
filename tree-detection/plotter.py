import matplotlib.pyplot as plt
import numpy as np


def segmentation_cmap():
    vals = np.linspace(0, 1, 256)
    np.random.shuffle(vals)
    return plt.cm.colors.ListedColormap(plt.cm.CMRmap(vals))


def plot_trees(data, plot_tree_ids):
    if plot_tree_ids:
        plt.scatter(data['x_1'], data['y_1'], s=1, c=data['tree_id'], cmap=segmentation_cmap())
    else:
        plt.scatter(data['x_1'], data['y_1'], s=1, c=data['coverage'], cmap='Greens')
    plt.show()
