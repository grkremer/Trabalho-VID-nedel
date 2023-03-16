using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fornecedor
{
    private string cnpj;
    private string nome;
    private string atividadePrincipal;
    private bool isMatriz;
    private string naturezaJuridica;
    private string municipio;
    private string uf;
    private string endereco;
    public float latitude;
    public float longitude;
    private List<Despesa> despesas = new List<Despesa>();

    public Fornecedor(
        string cnpj, 
        string nome, 
        string atividadePrincipal, 
        bool isMatriz, 
        string naturezaJuridica, 
        string municipio, 
        string uf, 
        string endereco, 
        float latitude, 
        float longitude) {
        this.cnpj = cnpj;
        this.nome = nome;
        this.atividadePrincipal = atividadePrincipal;
        this.isMatriz = isMatriz;
        this.naturezaJuridica = naturezaJuridica;
        this.municipio = municipio;
        this.uf = uf;
        this.endereco = endereco;
        this.latitude = latitude;
        this.longitude = longitude;
    }

    public void addDespesa(Despesa despesa) {
        despesas.Add(despesa);
    }

    public struct Despesa
    {
        public float valor;
        public string tipo;
        public DateTime data;

        public Despesa(float valor, string tipo, DateTime data) {
            this.valor = valor;
            this.tipo = tipo;
            this.data = data;
        }
    }
}


