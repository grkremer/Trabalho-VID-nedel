using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using System;
using System.Globalization;

public class MapPinner : MonoBehaviour
{
    public MapPinLayer pinLayer;
    public MapPin pin;
    public MapRenderer mapRenderer;

    private Dictionary<string, Fornecedor> dados;
    void Start()
    {
        //MapPin.UpdateScales(new List<MapPin>(){mapPin}, mapRenderer);
        dados = importaDados("Dados_processados_COMPLETO_CHEIO.csv");
        plotFornecedores();
    }
    void Update()
    {
        
    }

    private void plotFornecedores() {
        float min = 0f, max = float.MinValue;
        foreach(Fornecedor fornecedor in dados.Values) {
            float soma = fornecedor.getSomaDespesas();
            max = Math.Max(soma, max);
        }
        float tamanhoMinimo = 0.003f, tamanhoMaximo = 0.03f;
        foreach(Fornecedor fornecedor in dados.Values) {
            var mapPin = Instantiate(pin);
            float tamanho = Math.Clamp(normaliza(fornecedor.getSomaDespesas(), min, max)*tamanhoMaximo, tamanhoMinimo, tamanhoMaximo);
            mapPin.ScaleCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(tamanho, tamanho));
            mapPin.Location = new LatLon(fornecedor.latitude, fornecedor.longitude);
            pinLayer.MapPins.Add(mapPin);
        }
    }

    private float normaliza(float valor, float min, float max) {
        return (valor - min) / (max - min);
    }

    private Dictionary<string, Fornecedor> importaDados(string arquivo){
        Dictionary<string, Fornecedor> dados = new Dictionary<string,Fornecedor>();
        
        StreamReader strReader = new StreamReader("Assets/" + arquivo);
        string line = strReader.ReadLine();
        Dictionary<string, int> col = leNomesColuna(line);
        for(line = strReader.ReadLine(); line != null; line = strReader.ReadLine()) {
            string[] itens = line.Split('$');
            //Debug.Log(line);

            float valor = float.Parse(itens[col["VALOR"]]);
            string tipo = itens[col["TIPO"]];
            DateTime data = DateTime.Parse(itens[col["DATA PGTO"]]);
            Fornecedor.Despesa despesa = new Fornecedor.Despesa(valor, tipo, data);

            string cnpj = itens[col["CPF/CNPJ FORNECEDOR"]];
            if(!dados.ContainsKey(cnpj)) {
                string nome = itens[col["NOME FORNECEDOR"]];
                string atividadePrincipal = itens[col["ATIVIDADE_PRINCIPAL"]];
                bool isMatriz = "MATRIZ" == itens[col["MATRIZ"]];
                string naturezaJuridica = itens[col["NATUREZA_JURIDICA"]];
                string municipio = itens[col["MUNICIPIO"]];
                string uf = itens[col["UF"]];
                string endereco = itens[col["ENDEREÇO"]];
                float latitude = float.Parse(itens[col["Latitude"]], CultureInfo.InvariantCulture.NumberFormat);
                float longitude = float.Parse(itens[col["Longitude"]], CultureInfo.InvariantCulture.NumberFormat);
                Fornecedor fornecedor = new Fornecedor(
                    cnpj, 
                    nome, 
                    atividadePrincipal, 
                    isMatriz, 
                    naturezaJuridica, 
                    municipio, 
                    uf, 
                    endereco, 
                    latitude, 
                    longitude
                );
                dados[cnpj] = fornecedor;
            }
            dados[cnpj].addDespesa(despesa);
        }
        return dados;
    }
    
    Dictionary<string, int> leNomesColuna(string linha) {
        Dictionary<string, int> coluna = new Dictionary<string, int>();
        string[] itens = linha.Split('$');
        for(int i = 0; i < itens.Length; i++)
            coluna.Add(itens[i], i);
        return coluna;
    }
}
