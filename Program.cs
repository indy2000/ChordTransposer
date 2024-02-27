using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChordTransposer
{
    class Program
    {
        static List<string> notas = new List<string> {"A", "A#", "B", /*"B#",*/ "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };

        static void Main(string[] args)
        {
            LoadArquivo();

        }

        static void LoadArquivo()
        {
            string response = String.Empty;
            Console.WriteLine("Informe o caminho do arquivo .txt da cifra: ");
            response = Console.ReadLine();

            if (String.IsNullOrEmpty(response))
            {
                Console.WriteLine("Erro: Caminho do arquivo não pode estar em branco.");
                LoadArquivo();
            }

            try
            {
                if (!File.Exists(response))
                {
                    Console.WriteLine("Erro: Não foi possível localizar arquivo com o caminho informado.");
                    LoadArquivo();
                }

                var linhas = File.ReadAllLines(response);

                ConfigTranspose(linhas);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message} - {ex.StackTrace}");
                LoadArquivo();
            }            

        }

        static void ConfigTranspose(string[] linhas)
        {
            Console.WriteLine("Informe o tipo de transposição:");
            Console.WriteLine("1 - Descer o tom");
            Console.WriteLine("2 - Subir o tom");
            var operacao = Console.ReadLine();

            switch (operacao)
            {
                case "1":
                    Console.WriteLine("Informe quantos tons deve descer (Ex: 1 ou 1,5):");
                    var resultadoDescer = Console.ReadLine();
                    double doubleDescer = 0;
                    if (Double.TryParse(resultadoDescer, out doubleDescer) && doubleDescer > 0)
                    {
                        TransposeSong(linhas, 1, doubleDescer);
                    }
                    else
                    {
                        Console.WriteLine("Erro: Valor informado inválido.");
                        ConfigTranspose(linhas);
                    }
                    break;
                case "2":
                    Console.WriteLine("Informe quantos tons deve subir (Ex: 1 ou 1,5):");
                    var resultadoSubir = Console.ReadLine();
                    double doubleSubir = 0;
                    if (Double.TryParse(resultadoSubir, out doubleSubir) && doubleSubir > 0)
                    {
                        TransposeSong(linhas, 2, doubleSubir);
                    }
                    else
                    {
                        Console.WriteLine("Erro: Valor informado inválido.");
                        ConfigTranspose(linhas);
                    }
                    break;
                default:
                    Console.WriteLine("Erro: Não foi possível identificar o tipo de transposição.");
                    ConfigTranspose(linhas);
                    break;
            }
        }

        static void TransposeSong(string[] linhas, int tipo_operacao, double tom_transposicao)
        {
            //Lista com 3 oitavas para fazer a transposição
            var lista_oitavas = new List<string>();
            lista_oitavas.AddRange(notas);
            /*lista_oitavas.AddRange(notas);
            lista_oitavas.AddRange(notas);
            lista_oitavas.AddRange(notas);
            lista_oitavas.AddRange(notas);*/

            int qtde_notas = 0;
            /*if(tom_transposicao % 1 != 0)
            {
                if (tipo_operacao == 1)
                    qtde_tons = Convert.ToInt32(tom_transposicao) - 1;
                else
                    qtde_tons = Convert.ToInt32(tom_transposicao) + 1;
            }
            else
            {
                qtde_tons = Convert.ToInt32(tom_transposicao) * 2;
            }*/
            qtde_notas = Convert.ToInt32(tom_transposicao * 2) ;

            List<string> cifraTransposta = new List<string>();

            foreach (var linha in linhas)
            {
                var linha_temp = string.Empty;
                int index_linha = -1;
                foreach (var caractere in linha)
                {
                    index_linha++;

                    if (caractere == ' ')
                    {
                        linha_temp += caractere;
                        continue;
                    }
                        

                    string nota = linha.ElementAtOrDefault(index_linha + 1) == '#' ? caractere.ToString() + "#" : caractere.ToString();

                    if(lista_oitavas.Contains(nota))
                    {
                        if (tipo_operacao == 1) //Desce tom
                        {
                            var index = lista_oitavas.IndexOf(nota);
                            if((index - qtde_notas) < 0)
                            {
                                string nova_nota = lista_oitavas.ElementAtOrDefault(lista_oitavas.Count() - ((qtde_notas - index)));
                                //linha_temp = linha_temp.Replace(nota, nova_nota);
                                linha_temp += nova_nota;
                            }
                            else
                            {
                                string nova_nota = lista_oitavas.ElementAtOrDefault(index - qtde_notas);
                                //linha_temp = linha_temp.Replace(nota, nova_nota);
                                linha_temp += nova_nota;
                            }
                        }
                        else //Sobe tom
                        {
                            var index = lista_oitavas.IndexOf(nota);
                            if ((index + qtde_notas) >= lista_oitavas.Count())
                            {
                                string nova_nota = lista_oitavas.ElementAtOrDefault(0 + ((index - qtde_notas)));
                                //linha_temp = linha_temp.Replace(nota, nova_nota);
                                linha_temp += nova_nota;
                            }
                            else
                            {
                                string nova_nota = lista_oitavas.ElementAtOrDefault(index + qtde_notas);
                                //linha_temp = linha_temp.Replace(nota, nova_nota);
                                linha_temp += nova_nota;
                            }
                        }                        
                    }                        
                }
                cifraTransposta.Add(linha_temp);
            }

            if(cifraTransposta.Count() > 0)
            {
                Console.WriteLine("Transposição realizada com sucesso!");
                foreach (var linha in cifraTransposta)
                    Console.WriteLine(linha);
            }
            else
            {
                Console.WriteLine("Não foi possível realizar Transposição.");
            }
        }

    }
}
