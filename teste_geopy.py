import requests
import json
import time
from geopy.geocoders import ArcGIS

def get_company_info(cnpj, use_safe_access=True):
    url = f"https://receitaws.com.br/v1/cnpj/{cnpj}"

    querystring = {"token":"XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX","cnpj":"06990590000123","plugin":"RF"}
    if use_safe_access:
        time.sleep(20)
    response = requests.request("GET", url, params=querystring)
    if use_safe_access and not response.ok:
        time.sleep(60)
        response = requests.request("GET", url, params=querystring)
    return json.loads(response.text)

def get_lat_long(info):
    location = geolocator.geocode(f"{info['logradouro']}, {info['numero']}, {info['municipio']}, {info['uf']}")
    if location != None:
        return location.latitude, location.longitude
    print(f"Falha 1 {info['nome']}")
    location = geolocator.geocode(f"{info['logradouro']}, {info['municipio']}, {info['uf']}")
    if location != None:
        return location.latitude, location.longitude
    print(f"Falha 2 {info['nome']}")
    location = geolocator.geocode(f"{info['logradouro']}")
    if location != None:
        return location.latitude, location.longitude
    print(f"Falha 3 {info['nome']}")
    location = geolocator.geocode(f"{info['municipio']}, {info['uf']}")
    if location != None:
        return location.latitude, location.longitude
    print(f"Falha 4 {info['nome']}")
    location = geolocator.geocode("BRASIL")
    return location.latitude, location.longitude

GOOGLE_CNPJ = "06990590000123"
RIOT_CNPJ = "15409786000172"

geolocator = ArcGIS(user_agent="cnpj")

cnpjs = ["37064730000166", "00394502017209", "03696869000100", "38030169000167"]

for cnpj in cnpjs:
    info = get_company_info(cnpj)
    print(get_lat_long(info))