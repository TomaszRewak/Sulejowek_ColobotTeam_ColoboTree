### Data pre-processing

1. Place the `.las` (containing the LiDAR data for a selected area) file in the `tree-detection/.data` folder as `example.las`.

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