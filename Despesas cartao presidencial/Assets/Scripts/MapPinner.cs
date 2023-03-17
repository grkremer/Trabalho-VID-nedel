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
    private Double lat;
    private Double lon;
    private Single zoom;
    private Single zoomAlvo;
    private Double latAlvo;
    private Double lonAlvo;

    void Start()
    {
        //MapPin.UpdateScales(new List<MapPin>(){mapPin}, mapRenderer);
        dados = importaDados("Dados_processados_COMPLETO_CHEIO.csv");
        plotFornecedores();
        lat = mapRenderer.Center.LatitudeInDegrees;
        lon = mapRenderer.Center.LongitudeInDegrees;
        zoom = mapRenderer.ZoomLevel;
        zoomAlvo = zoom;
        latAlvo = lat;
        lonAlvo = lon;
    }
    void Update()
    {   

        if(Input.GetKeyDown(KeyCode.N))
            zoomAlvo = zoom - 0.4f;
        if(Input.GetKeyDown(KeyCode.M))
            zoomAlvo = zoom + 0.4f;
        mapRenderer.ZoomLevel = zoom;
        if(Input.GetKeyDown(KeyCode.I))
            latAlvo =  lat + 100/(zoom*zoom*zoom);
        if(Input.GetKeyDown(KeyCode.J))
            lonAlvo = lon - 100/(zoom*zoom*zoom);
        if(Input.GetKeyDown(KeyCode.K))
            latAlvo = lat - 100/(zoom*zoom*zoom);
        if(Input.GetKeyDown(KeyCode.L))
            lonAlvo = lon + 100/(zoom*zoom*zoom);

        if (Math.Abs(zoom - zoomAlvo) < 0.005f + (zoom - zoomAlvo)/4)
            zoom = zoomAlvo;
        else
            if (zoom > zoomAlvo)
                zoom -= 0.005f + (zoom - zoomAlvo)/4;
            else if (zoom < zoomAlvo)
                zoom += 0.005f + ( zoomAlvo - zoom)/4;
        
        if (Math.Abs(lat - latAlvo) < 5/(zoom*zoom*zoom))
            lat = latAlvo;
        else
            if (lat > latAlvo)
                lat -= 5/(zoom*zoom*zoom);
            else if (lat < latAlvo)
                lat += 5/(zoom*zoom*zoom);
        if (Math.Abs(lon - lonAlvo) < 5/(zoom*zoom*zoom))
            lon = lonAlvo;
        else
            if (lon > lonAlvo)
                lon -= 5/(zoom*zoom*zoom);
            else if (lon < lonAlvo)
                lon += 5/(zoom*zoom*zoom);
        mapRenderer.Center = new LatLon(lat, lon);
    }

    private void plotFornecedores() {
        float min = 0f, max = float.MinValue;
        foreach(Fornecedor fornecedor in dados.Values) {
            float soma = fornecedor.getSomaDespesas();
            max = Math.Max(soma, max);
        }
        float tamanhoMinimo = 0.02f, tamanhoMaximo = 0.2f;
        foreach(Fornecedor fornecedor in dados.Values) {
            var mapPin = Instantiate(pin);
            float tamanho = Math.Clamp(normaliza(fornecedor.getSomaDespesas(), min, max)*tamanhoMaximo, tamanhoMinimo, tamanhoMaximo);
            mapPin.ScaleCurve = new AnimationCurve(new Keyframe(tamanho, tamanho), new Keyframe(tamanho, tamanho));
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
