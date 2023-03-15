using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float latitude;
    private float longitude;
    private List<Despesa> despesas;

    Fornecedor() {

    }
    public struct Despesa
    {
        float valor;
        string tipo;
        //DateTime data;
    }
}


