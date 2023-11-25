import pylas
import pandas as pd

tree_classification = 5

def import_data():
    with pylas.open('tree-detection/.data/example.las') as f:
        las = f.read()

    x = las.x
    y = las.y
    z = las.z
    classification = las.classification

    points_df = pd.DataFrame({'x': x, 'y': y, 'z': z, 'classification': classification})
    trees_df = points_df[points_df['classification'] == tree_classification]

    return trees_df
