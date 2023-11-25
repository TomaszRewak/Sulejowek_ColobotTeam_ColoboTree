import psycopg2

connection_string = "dbname='postgres' user='postgres' host='localhost' port='2137' password='password'"
chunk_size = 1000


def get_connection():
    return psycopg2.connect(connection_string)


def clear_db():
    with get_connection() as conn:
        with conn.cursor() as cur:
            cur.execute("DELETE FROM area_chunks")
            conn.commit()


def insert_chunk(chunk):
    with get_connection() as conn:
        with conn.cursor() as cur:
            query = """
                INSERT INTO area_chunks (
                    upper_left_vertex_2180,
                    bottom_right_vertex_2180,
                    upper_left_vertex_4326,
                    bottom_right_vertex_4326,
                    tree_coverage_percentage,
                    tree_classification,
                    tree_id,
                    resolution
                )
                VALUES (
                    ST_SetSRID(ST_MakePoint(%s, %s), 2180),
                    ST_SetSRID(ST_MakePoint(%s, %s), 2180),
                    ST_SetSRID(ST_MakePoint(%s, %s), 4326),
                    ST_SetSRID(ST_MakePoint(%s, %s), 4326),
                    %s,
                    %s,
                    %s,
                    %s
                )
            """
            cur.executemany(query, chunk)
            conn.commit()


def map_params(data, resolution):
    return [
        (
            float(x_1),
            float(y_1),
            float(x_2),
            float(y_2),
            float(lon_1),
            float(lat_1),
            float(lon_2),
            float(lat_2),
            float(coverage) if coverage is not None else None,
            5,
            int(tree_id) if tree_id is not None else None,
            int(resolution)
        )
        for x_1, y_1, x_2, y_2, lon_1, lat_1, lon_2, lat_2, coverage, tree_id
        in zip(
            data['x_1'],
            data['y_1'],
            data['x_2'],
            data['y_2'],
            data['lon_1'],
            data['lat_1'],
            data['lon_2'],
            data['lat_2'],
            data['coverage'],
            data['tree_id']
        )
    ]


def split_to_chunks(params):
    return [
        params[i:i + chunk_size]
        for i
        in range(0, len(params), chunk_size)
    ]


def save_to_db(data, resolution):
    params = map_params(data, resolution)
    params_chunks = split_to_chunks(params)

    for params_chunk in params_chunks:
        insert_chunk(params_chunk)
