import os
import pandas as pd
import pyodbc

SERVER_NAME = 'DESKTOP-5U8J0TT\SQLEXPRESS'
DATABASE_NAME = 'AromaDom'

conn_str = (
    f"DRIVER={{ODBC Driver 17 for SQL Server}};"
    f"SERVER={SERVER_NAME};"
    f"DATABASE={DATABASE_NAME};"
    f"Trusted_Connection=yes;"
)

conn = pyodbc.connect(conn_str)
cursor = conn.cursor()

def insert_many(table, columns, data):
    """Вставка множества записей в таблицу"""
    placeholders = ', '.join(['?' for _ in columns])
    cols = ', '.join(columns)
    sql = f"INSERT INTO {table} ({cols}) VALUES ({placeholders})"
    cursor.executemany(sql, data)
    conn.commit()
    return len(data)

SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))

df_mt = pd.read_excel(os.path.join(SCRIPT_DIR, 'АД_Material_type_import.xlsx'))
df_mt.columns = ['name', 'loss_percent']

df_mt['loss_percent'] = (
    df_mt['loss_percent']
    .astype(str)
    .str.replace('%', '', regex=False)
    .str.replace(',', '.', regex=False)
    .astype(float)
)

data_mt = [tuple(row) for row in df_mt.values]
count = insert_many('MaterialTypes', ['name', 'loss_percent'], data_mt)

df_pt = pd.read_excel(os.path.join(SCRIPT_DIR, 'АД_Product_type_import.xlsx'))
df_pt.columns = ['name', 'coefficient']

data_pt = [tuple(row) for row in df_pt.values]
count = insert_many('ProductTypes', ['name', 'coefficient'], data_pt)

df_mat = pd.read_excel(os.path.join(SCRIPT_DIR, 'АД_Materials_import.xlsx'))
df_mat.columns = ['name', 'material_type_name', 'price', 'stock_quantity',
                  'min_quantity', 'package_quantity', 'unit']

cursor.execute("SELECT id, name FROM MaterialTypes")
types_map = {row[1]: row[0] for row in cursor.fetchall()}

df_mat['material_type_id'] = df_mat['material_type_name'].map(types_map)

data_mat = [tuple(row) for row in df_mat[['material_type_id', 'name', 'price',
                                           'stock_quantity', 'min_quantity',
                                           'package_quantity', 'unit']].values]
count = insert_many('Materials',
                    ['material_type_id', 'name', 'price', 'stock_quantity',
                     'min_quantity', 'package_quantity', 'unit'], data_mat)

df_prod = pd.read_excel(os.path.join(SCRIPT_DIR, 'АД_Products_import.xlsx'))
df_prod.columns = ['article', 'product_type_name', 'name', 'min_partner_price']

cursor.execute("SELECT id, name FROM ProductTypes")
ptypes_map = {row[1]: row[0] for row in cursor.fetchall()}

df_prod['product_type_id'] = df_prod['product_type_name'].map(ptypes_map)

data_prod = [tuple(row) for row in df_prod[['product_type_id', 'article', 'name',
                                             'min_partner_price']].values]
count = insert_many('Products',
                    ['product_type_id', 'article', 'name', 'min_partner_price'], data_prod)

df_mp = pd.read_excel(os.path.join(SCRIPT_DIR, 'АД_Material_products_import.xlsx'))
df_mp.columns = ['material_name', 'product_name', 'quantity_per_unit']

cursor.execute("SELECT id, name FROM Materials")
mat_map = {row[1]: row[0] for row in cursor.fetchall()}

cursor.execute("SELECT id, name FROM Products")
prod_map = {row[1]: row[0] for row in cursor.fetchall()}

df_mp['material_id'] = df_mp['material_name'].map(mat_map)
df_mp['product_id'] = df_mp['product_name'].map(prod_map)

unresolved = df_mp[df_mp['material_id'].isna() | df_mp['product_id'].isna()]
if len(unresolved) > 0:
    print(f"Не найдено связей: {len(unresolved)}")
    print(unresolved[['material_name', 'product_name']])

data_mp = [tuple(row) for row in df_mp[['material_id', 'product_id',
                                         'quantity_per_unit']].values]
count = insert_many('MaterialProducts',
                    ['material_id', 'product_id', 'quantity_per_unit'], data_mp)

cursor.close()
conn.close()
print("\nИмпорт завершён успешно")