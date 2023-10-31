using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Projeto_Web_Lh_Pets_versão_1
{
    class Banco
    {
        private readonly string connectionString;
        private List<Clientes> lista = new List<Clientes>();

        public Banco(string connectionString)
        {
            this.connectionString = connectionString;
            RetrieveDataFromDatabase();
        }

        private void RetrieveDataFromDatabase()
        {
            try
            {
                using (SqlConnection conexao = new SqlConnection(connectionString))
                {
                    string sql = "SELECT * FROM tblclientes";
                    using (SqlCommand comando = new SqlCommand(sql, conexao))
                    {
                        conexao.Open();
                        using (SqlDataReader tabela = comando.ExecuteReader())
                        {
                            while (tabela.Read())
                            {
                                lista.Add(new Clientes
                                {
                                    cpf_cnpj = tabela["cpf_cnpj"].ToString(),
                                    nome = tabela["nome"].ToString(),
                                    endereco = tabela["endereco"].ToString(),
                                    rg_ie = tabela["rg_ie"].ToString(),
                                    tipo = tabela["tipo"].ToString(),
                                    valor = (float)Convert.ToDecimal(tabela["valor"]),
                                    valor_imposto = (float)Convert.ToDecimal(tabela["valor_imposto"]),
                                    total = (float)Convert.ToDecimal(tabela["total"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao recuperar dados do banco de dados: " + e.Message);
            }
        }

        public List<Clientes> GetLista()
        {
            return lista;
        }

        public string GetListaString()
        {
            string html = "<!DOCTYPE html>\n<html>\n<head>\n<meta charset='utf-8' />\n" +
                          "<title>Cadastro de Clientes</title>\n</head>\n<body>";
            html += "<b>   CPF / CNPJ    -      Nome    -    Endereço    -   RG / IE   -   Tipo  -   Valor   - Valor Imposto -   Total  </b>";

            int i = 0;
            foreach (Clientes cli in GetLista())
            {
                string corfundo = (i % 2 == 0) ? "#6f47ff" : "#ffffff";
                string cortexto = (i % 2 == 0) ? "white" : "#6f47ff";
                i++;

                html += $"\n<br><div style='background-color:{corfundo};color:{cortexto}'>" +
                        $"{cli.cpf_cnpj} - {cli.nome} - {cli.endereco} - {cli.rg_ie} - " +
                        $"{cli.tipo} - {cli.valor.ToString("C")} - {cli.valor_imposto.ToString("C")} - {cli.total.ToString("C")}<br>" +
                        "</div>";
            }

            return html;
        }

        public void ImprimirListaConsole()
        {
            Console.WriteLine("   CPF / CNPJ   -   Nome   -   Endereço   -   RG / IE   -   Tipo   -   Valor   - Valor Imposto   -   Total");
            foreach (Clientes cli in GetLista())
            {
                Console.WriteLine($"{cli.cpf_cnpj} - {cli.nome} - {cli.endereco} - {cli.rg_ie} - {cli.tipo} - {cli.valor.ToString("C")} - {cli.valor_imposto.ToString("C")} - {cli.total.ToString("C")}");
            }
        }
    }
}
