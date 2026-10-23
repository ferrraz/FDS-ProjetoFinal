
namespace FDS___Projeto
{
    using Alba.CsConsoleFormat;
    using Alba.CsConsoleFormat.Fluent;
    using FDS_ProjetoFinal;
    using Microsoft.Data.SqlClient;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Net.Http.Headers;
    using System.Reflection.PortableExecutable;
    using static System.ConsoleColor;

    internal class Program
    {

        public static void menu()
        {
            string menuTexto =
                "   Sistema do Banco\n\n" +
                "   1. Menu Clientes\n" +
                "   2. Menu Cartões\n" +
                "   3. Menu Operações\n\n" +
                "   0. Sair\n";

            var doc = new Document(
                new Grid
                {
                    Stroke = LineThickness.None,
                    Columns =
                    {
            new Column { Width = GridLength.Star(1) }
                        },
                    Children =
                        {
            new Cell(menuTexto)
            {
                Color = ConsoleColor.White,
                Align = Align.Left,
                Padding = new Thickness(4)
            }
                    }
                }
            );

            ConsoleRenderer.RenderDocument(doc);
        }

        static void Main(string[] args)
        {
            int escolha;

            do
            {
                Console.Clear();
                menu();
                Console.WriteLine("\n");
                escolha = int.Parse(Console.ReadLine());

                switch (escolha)
                {
                    case 1:
                        int escolhab;
                        Console.Clear();

                        do
                        {
                            Console.Clear();
                            Cliente.menuCliente();
                            Console.WriteLine("\n");
                            escolhab = int.Parse(Console.ReadLine());

                            switch (escolhab)
                            {
                                case 1:
                                    Console.Clear();
                                    string nome;
                                    string nif;
                                    DateTime datadenasci;
                                    string morada;
                                    string telemovel;
                                    string email;
                                    int contadorpes;

                                    do
                                    {
                                        Console.WriteLine("Digite quantos clientes quer inserir");
                                        contadorpes = int.Parse(Console.ReadLine());

                                    } while (contadorpes <= 0);

                                    for (int i = 1; i <= contadorpes; i++)
                                    {
                                        Console.WriteLine($"Digite o nome do cliente {i}: ");
                                        nome = Console.ReadLine();

                                        do
                                        {
                                            Console.WriteLine($"Digite o nif do cliente {i}: ");
                                            nif = Console.ReadLine();
                                        } while (nif.Length != 9 || !nif.All(char.IsDigit));


                                        Console.WriteLine($"Digite a data de nascimento do cliente {i}: ");
                                        DateTime.TryParse(Console.ReadLine(), out datadenasci);

                                        Console.WriteLine($"Digite o morada do cliente {i}: ");
                                        morada = Console.ReadLine();

                                        do
                                        {
                                            Console.WriteLine($"Digite o número do cliente {i}: ");
                                            telemovel = Console.ReadLine();
                                        } while (telemovel.Length != 9 || !telemovel.All(char.IsDigit));

                                        do
                                        {
                                            Console.WriteLine($"Digite o email do cliente {i}: ");
                                            email = Console.ReadLine();

                                        } while (!email.Contains('@'));

                                        Cliente cliente1 = new Cliente(nome, nif, datadenasci, morada, telemovel, email);
                                        cliente1.CadastrarCliente();
                                        cliente1 = null;
                                    }
                                    break;

                                case 2:
                                    Console.Clear();
                                    Cliente.listarCliente();
                                    Console.ReadLine();
                                    break;

                                case 3:
                                    int id;
                                    Console.Clear();

                                    Cliente.listarCliente();
                                    do
                                    {
                                        Console.WriteLine("Digite o id do cliente que quer remover: ");
                                        id = int.Parse(Console.ReadLine());
                                    } while (!Cliente.idClienteExiste(id));

                                    Cliente cliente2 = Cliente.ObterClientePorId(id);

                                    if (cliente2 == null)
                                    {
                                        Console.WriteLine("Cliente não encontrado.");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Cliente encontrado: {cliente2.Nome}");
                                        Console.Write("Tem a certeza que deseja apagar? (s/n): ");

                                        if (Console.ReadLine().ToLower() == "s")
                                        {
                                            if (cliente2.ApagarCliente())
                                            {
                                                Console.WriteLine("Cliente apagado com sucesso.");
                                                Console.ReadLine();
                                            }
                                            else
                                            {
                                                Console.WriteLine("Erro ao apagar cliente.");
                                                Console.ReadLine();
                                            }
                                        }
                                    }
                                    break;

                                case 4:
                                    int id2;
                                    Console.Clear();

                                    Cliente.listarCliente();
                                    do
                                    {
                                        Console.WriteLine("Digite o id do cliente que quer editar: ");
                                        id2 = int.Parse(Console.ReadLine());
                                    } while (!Cliente.idClienteExiste(id2));

                                    Cliente cliente3 = Cliente.ObterClientePorId(id2);

                                    if (cliente3 == null)
                                    {
                                        Console.WriteLine("Cliente não encontrado.");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Cliente encontrado: {cliente3.Nome}");
                                        Console.Write("Tem a certeza que deseja editar? (s/n): ");

                                        if (Console.ReadLine().ToLower() == "s")
                                        {
                                            if (cliente3.editarCliente())
                                            {
                                                Console.WriteLine("Cliente editado com sucesso.");
                                                Console.ReadLine();
                                            }
                                            else
                                            {
                                                Console.WriteLine("Erro ao editar cliente.");
                                                Console.ReadLine();
                                            }
                                        }
                                    }
                                    break;

                                case 0:
                                    Console.WriteLine("A sair.");
                                    break;

                                default:
                                    Console.WriteLine("Opção invalida.");
                                    break;
                            }
                        } while (escolhab != 0);

                    break;
 
                    case 2:
                        int escolhac;
                        Console.Clear();

                        do
                        {
                            Console.Clear();
                            Cartao.menuCartao();
                            Console.WriteLine("\n");
                            escolhac = int.Parse(Console.ReadLine());

                        switch (escolhac)
                        {
                            case 1:
                                Console.Clear();
                                int tipo;
                                int contadorpes;

                                do
                                {
                                    Console.WriteLine("Digite quantos cartões quer inserir");
                                    contadorpes = int.Parse(Console.ReadLine());
                                } while (contadorpes <= 0);

                                for (int i = 1; i <= contadorpes; i++)
                                {
                                    Console.WriteLine($"Escolha o tipo de cartão para o cartão {i}, 1 - Crédito 2 - Débito: ");
                                    tipo = int.Parse(Console.ReadLine());

                                    Cartao cartao1;

                                    if (tipo == 1)
                                    {

                                        int saldo;

                                        Console.WriteLine("Digite o saldo: ");
                                        saldo = int.Parse(Console.ReadLine());

                                        cartao1 = new CartaoCredito
                                        {
                                            Tipo = "Crédito",
                                            Saldo = saldo,
                                        };
                                    }
                                    else
                                    {

                                        int limite;
                                        decimal taxas;

                                        Console.WriteLine("Digite o limite: ");
                                        limite = int.Parse(Console.ReadLine());

                                        Console.WriteLine("Digite as taxas: ");
                                        taxas = decimal.Parse(Console.ReadLine());

                                        cartao1 = new CartaoDebito
                                        {
                                            Tipo = "Débito",
                                            Limite = limite,
                                            Taxas_Juros = taxas,
                                        };
                                    }

                                    cartao1.InserirCartao();
                                    cartao1 = null;

                                }

                                break;

                                case 2:
                                    Console.Clear();
                                    Cartao.ListarCartao();
                                    Console.ReadLine();
                                break;

                                case 3:
                                    int id;
                                    Console.Clear();

                                    Cartao.ListarCartao();
                                    do
                                    {
                                        Console.WriteLine("Digite o id do cartão que quer remover: ");
                                        id = int.Parse(Console.ReadLine());
                                    } while (!Cartao.CartaoExiste(id));

                                    Cartao cartao2 = Cartao.ObterCartaoPorId(id);

                                    if (cartao2 == null)
                                    {
                                        Console.WriteLine("Cartão não encontrado.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Cartão encontrado.");
                                        Console.Write("Tem a certeza que deseja apagar? (s/n): ");

                                        if (Console.ReadLine().ToLower() == "s")
                                        {
                                            if (cartao2.ApagarCartao())
                                            {
                                                Console.WriteLine("Cartão apagado com sucesso.");
                                                Console.ReadLine();
                                            }
                                            else
                                            {
                                                Console.WriteLine("Erro ao apagar cartão.");
                                                Console.ReadLine();

                                            }
                                        }
                                    }
                                break;

                                case 0:
                                    Console.WriteLine("A sair.");
                                    break;

                                default:
                                    Console.WriteLine("Opção invalida.");
                                break;
                            }
                        } while (escolhac != 0);

                    break;

                    case 3:
                        int escolhaf;
                        Console.Clear();

                        do
                        {
                            Console.Clear();
                            Operacoes.menuOperacoes();
                            Console.WriteLine("\n");
                            escolhaf = int.Parse(Console.ReadLine());

                            switch (escolhaf)
                            {
                                case 1:
                                    Console.Clear();
                                    int clienteID;
                                    int cartaoId;
                                    DateTime data_inicio;
                                    DateTime data_fim;
                                    decimal preco_total;
                                    int contadorpes;

                                    Console.WriteLine("Digite quantas operações quer inserir");
                                    contadorpes = int.Parse(Console.ReadLine());

                                    for (int i = 1; i <= contadorpes; i++)
                                    {
                                        Cliente.listarCliente();

                                        do
                                        {
                                            Console.WriteLine($"Digite o id do cliente {i}: ");
                                            clienteID = int.Parse(Console.ReadLine());
                                        } while (!Cliente.idClienteExiste(clienteID));

                                        Cartao.ListarCartao();

                                        do
                                        {
                                            Console.WriteLine($"Digite o id do cartão {i}: ");
                                            cartaoId = int.Parse(Console.ReadLine());
                                        } while (!Cliente.idClienteExiste(cartaoId));

                                        Console.WriteLine($"Digite a data início da operação {i}: ");
                                        DateTime.TryParse(Console.ReadLine(), out data_inicio);

                                        Console.WriteLine($"Digite a data fim da operação {i}: ");
                                        DateTime.TryParse(Console.ReadLine(), out data_fim);

                                        Console.WriteLine($"Digite a preço total da operação {i}: ");
                                        preco_total = decimal.Parse(Console.ReadLine());

                                        Operacoes operacao1 = new Operacoes(clienteID, cartaoId, data_inicio, data_fim, preco_total);
                                        operacao1.InserirOperacoes();
                                        operacao1 = null;
                                    }
                                break;

                                case 2:
                                    Console.Clear();
                                    Operacoes.listarOperacoes();
                                    Console.ReadLine();
                                break;

                                case 3:
                                    int id;
                                    Console.Clear();

                                    Operacoes.listarOperacoes();
                                    do
                                    {
                                        Console.WriteLine("Digite o id da operação que quer remover: ");
                                        id = int.Parse(Console.ReadLine());
                                    } while (!Operacoes.operaçaoExistente(id));

                                    Operacoes operacao2 = Operacoes.ObterOperacaoPorId(id);

                                    if (operacao2 == null)
                                    {
                                        Console.WriteLine("Operação não encontrado.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Operação encontrado.");
                                        Console.Write("Tem a certeza que deseja apagar? (s/n): ");

                                        if (Console.ReadLine().ToLower() == "s")
                                        {
                                            if (operacao2.ApagarOperaçao())
                                            {
                                                Console.WriteLine("Operação apagada com sucesso.");
                                                Console.ReadLine();
                                            }
                                            else
                                            {
                                                Console.WriteLine("Erro ao apagar operação.");
                                                Console.ReadLine();

                                            }
                                        }
                                    }
                                break;

                                case 4:
                                    int id2;
                                    Console.Clear();

                                    Operacoes.listarOperacoes();
                                    do
                                    {
                                        Console.WriteLine("Digite o id da operação que quer editar: ");
                                        id2 = int.Parse(Console.ReadLine());
                                    } while (!Operacoes.operaçaoExistente(id2));

                                    Operacoes operacao3 = Operacoes.ObterOperacaoPorId(id2);

                                    if (operacao3 == null)
                                    {
                                        Console.WriteLine("Operação não encontrado.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Operação encontrado.");
                                        Console.Write("Tem a certeza que deseja editar? (s/n): ");

                                        if (Console.ReadLine().ToLower() == "s")
                                        {
                                            if (operacao3.editarOperação())
                                            {
                                                Console.WriteLine("Operação editada com sucesso.");
                                                Console.ReadLine();
                                            }
                                            else
                                            {
                                                Console.WriteLine("Erro ao editar operação.");
                                                Console.ReadLine();
                                            }
                                        }
                                    }
                                    break;

                                case 0:
                                    Console.WriteLine("A sair.");
                                break;

                                default:
                                    Console.WriteLine("Opção invalida.");
                                break;
                            }
                        } while (escolhaf != 0);

                        break;

                    case 0:
                        Console.WriteLine("A desligar o sistema.");
                    break;

                    default:
                        Console.WriteLine("Opção invalida.");
                    break;
                }
            } while (escolha != 0);
        }
    }
}
