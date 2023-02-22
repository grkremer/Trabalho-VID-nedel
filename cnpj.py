import requests
import json
import time
from geopy.geocoders import Nominatim

def get_company_info(cnpj):
    url = f"https://receitaws.com.br/v1/cnpj/{cnpj}"
    querystring = {"token":"XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX","cnpj":"06990590000123","plugin":"RF"}
    time.sleep(20)
    response = requests.request("GET", url, params=querystring)
    while not response.ok:
        time.sleep(20)
        response = requests.request("GET", url, params=querystring)
    return json.loads(response.text)

def get_company_adress(info):
    return f"{info['logradouro']}, {info['numero']}, {info['uf']}"

GOOGLE_CNPJ = "06990590000123"
RIOT_CNPJ = "15409786000172"

geolocator = Nominatim(user_agent="cnpj")

cnpjs = [GOOGLE_CNPJ, RIOT_CNPJ]
for cnpj in cnpjs:
    info = get_company_info(cnpj)
    location = geolocator.geocode(get_company_adress(info))
    print(f"{info['nome']}: {get_company_adress(info)} ({location.latitude}, {location.longitude})")
    #time.sleep(20)