import requests
import json

def get_company_info(cnpj):
    url = f"https://receitaws.com.br/v1/cnpj/{cnpj}"
    querystring = {"token":"XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX","cnpj":"06990590000123","plugin":"RF"}
    response = requests.request("GET", url, params=querystring)
    return json.loads(response.text)

def print_company_adress(info):
    print(f"{info['nome']}: {info['logradouro']}, {info['numero']} ({info['complemento']}) - CEP {info['cep']}")

GOOGLE_CNPJ = "06990590000123"
info = get_company_info(GOOGLE_CNPJ)
print(info)