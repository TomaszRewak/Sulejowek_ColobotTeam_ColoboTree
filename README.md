
### Database setup

1. Run from `api`
```
docker-compose up
```

It will setup the Postgres database.

2. Run 
```
CREATE EXTENSION postgis;

CREATE TABLE public.area_chunks (
	id serial4 NOT NULL,
	upper_left_vertex_2180 public.geometry(point, 2180) NULL,
	upper_left_vertex_4326 public.geometry(point, 4326) NULL,
	tree_coverage_percentage numeric(10, 2) NULL,
	tree_classification int4 NULL,
	tree_id int4 NULL,
	bottom_right_vertex_2180 public.geometry(point, 2180) NULL,
	bottom_right_vertex_4326 public.geometry(point, 4326) NULL,
	resolution int4 NULL,
	CONSTRAINT area_chunks_pkey PRIMARY KEY (id)
);

CREATE INDEX idx_area_chunks_center_point_2180 ON public.area_chunks USING gist (upper_left_vertex_2180);
CREATE INDEX idx_area_chunks_center_point_4326 ON public.area_chunks USING gist (upper_left_vertext_4326);
CREATE INDEX idx_bottom_right_vertex_2180 ON public.area_chunks USING gist (bottom_right_vertex_2180);
CREATE INDEX idx_bottom_right_vertex_4326 ON public.area_chunks USING gist (bottom_right_vertex_4326);
```

on database to prepare structure


### Data pre-processing

1. Place the `.las` (containing the LiDAR data for a selected area) file in the `tree-detection/.data` folder as `example.las` (https://1drv.ms/u/s!AoCvYEtZOzNgjsks_gYzZBoPv_yepw?e=Lk1jnR)

2. Create and activate a virtual environment:

```bash
cd tree-detection
python -m venv .venv
source .venv/bin/activate
pip install -r requirements.txt
cd ..
```

3. Update the database connection string in the `tree-detection/db.py` file.

4. Run the data pre-processing script:

```bash
python tree-detection/main.py
```

5. Wait for the script to finish. The output will be saved in the database. You will see a data visualization once the process is complete.

### API

1. Run from `/api`
```
dotnet run
```

### Frontend

1. Run from `/ui`
```
npm install
npm run dev
```