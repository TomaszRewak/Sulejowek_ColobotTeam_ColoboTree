import psycopg2

connection_string = "dbname='postgres' user='postgres' host='4.tcp.eu.ngrok.io' port='14858' password='password'"
chunk_size = 1000


def get_connection():
    return psycopg2.connect(connection_string)


def delete_all():
    with get_connection() as conn:
        with conn.cursor() as cur:
            cur.execute("DELETE FROM area_chunks")
            conn.commit()


def insert_chunk(chunk):
    with get_connection() as conn:
        with conn.cursor() as cur:
            query = """
                INSERT INTO area_chunks (center_point_2180, center_point_4326, tree_coverage_percentage, tree_classification, tree_id)
                VALUES (ST_SetSRID(ST_MakePoint(%s, %s), 2180), ST_SetSRID(ST_MakePoint(%s, %s), 4326), %s, %s, %s)
            """
            cur.executemany(query, chunk)
            conn.commit()


def map_params(params):
    return [
        (float(x), float(y), float(lon), float(lat), 100, 5, int(cluster))
        for x, y, lon, lat, cluster
        in zip(params['x'].values, params['y'].values, params['lon'].values, params['lat'].values, params['cluster'].values)
    ]


def split_to_chunks(params):
    return [
        params[i:i + chunk_size]
        for i
        in range(0, len(params), chunk_size)
    ]


def save_to_db(coordinates):
    delete_all()

    params = map_params(coordinates)
    params_chunks = split_to_chunks(params)

    for params_chunk in params_chunks:
        insert_chunk(params_chunk)
