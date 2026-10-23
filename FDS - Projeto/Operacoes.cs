using Alba.CsConsoleFormat;
using FDS___Projeto;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FDS_ProjetoFinal
{
    public class Operacoes
    {

        #region Propriedades

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        private int operacaoID;

        public int OperacaoID
        {
            get { return operacaoID; }
            set { operacaoID = value; }
        }

        [Required]
        private int cartaoID;

        public int CartaoID
        {
            get { return cartaoID; }
            set { cartaoID = value; }
        }

        [Required]
        private int clienteID;

        public int ClienteID
        {
            get { return clienteID; }
            set { clienteID = value; }
        }

        [Required]
        private DateTime data_inicio;

        public DateTime Data_inicio
        {
            get { return data_inicio; }
            set { data_inicio = value; }
        }

        [Required]
        private DateTime data_fim;

        public DateTime Data_fim
        {
            get { return data_fim; }
            set { data_fim = value; }
        }

        [Required]
        private decimal preco_total;

        public decimal Preco_total
        {
            get { return preco_total; }
            set { preco_total = value; }
        }

        #endregion

        #region Construtores


        public Operacoes()
        {

        }

        public Operacoes( int? cartaoID, int? clienteID, DateTime data_inicio, DateTime data_fim, decimal preco_total)
        {
            this.cartaoID = (int)cartaoID;
            this.clienteID = (int)clienteID;
            this.data_inicio = data_inicio;
            this.data_fim = data_fim;
            this.preco_total = preco_total;
        }

        #endregion

        #region Metodos

        static string ConnectionString = "Password=Bdsql@123;Persist Security Info=True;User ID=Sa;Initial Catalog=FDS_Projeto;Data Source=localhost,1433;TrustServerCertificate=True;";

        public static void menuOperacoes()
        {
            string menuTexto =
                "   Menu do Operação\n\n" +
                "   1. Adicionar Operação(s).\n" +
                "   2. Listar Operações.\n" +
                "   3. Remover uma Operação.\n" +
                "   4. Editar uma Operação.\n\n" +
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


        public void InserirOperacoes()
        {


            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                conexao.Open();
                string query = "INSERT INTO Operacoes (cartaoID, clienteID , data_inicio, data_fim, preco_total) VALUES (@cartaoID, @clienteID, @data_inicio, @data_fim, @preco_total)";
                using (SqlCommand comando = new SqlCommand(query, conexao))
                {
                    comando.Parameters.AddWithValue("@cartaoID", this.cartaoID);
                    comando.Parameters.AddWithValue("@clienteID", this.clienteID);
                    comando.Parameters.AddWithValue("@data_inicio", this.data_inicio);
                    comando.Parameters.AddWithValue("@data_fim", this.data_fim);
                    comando.Parameters.AddWithValue("@preco_total", this.Preco_total);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public static bool operaçaoExistente(int idOperacao)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"SELECT COUNT(*) FROM dbo.Operacoes WHERE operacaoID = @operacaoID";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@operacaoID", SqlDbType.Int).Value = idOperacao;

                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();

                    return count > 0;
                }
            }
        }

        public bool ApagarOperaçao()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                string checkSql = "SELECT COUNT(*) FROM Operacoes WHERE operacaoID = @operacaoID";
                using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@operacaoID", this.operacaoID);

                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    { 
                        return false;
                    }
                }

                string deleteSql = "DELETE FROM Operacoes WHERE operacaoID = @operacaoID";
                using (SqlCommand deleteCmd = new SqlCommand(deleteSql, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@operacaoID", this.operacaoID);
                    int rowsAffected = deleteCmd.ExecuteNonQuery();
                }

                return true;
            }
        }

        public bool editarOperação()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT COUNT(*) FROM Operacoes WHERE operacaoID = @operacaoID";
                conn.Open();

                    
                using (SqlCommand checkCmd = new SqlCommand(sql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@operacaoID", this.operacaoID);

                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        return false;
                    }
                }

                int cartaoID;
                int clienteID;
                DateTime data_inicio;
                DateTime data_fim;
                decimal preco_total;

                Cliente.listarCliente();

                do
                {
                    Console.WriteLine($"Digite o id do cliente: ");
                    clienteID = int.Parse(Console.ReadLine());
                } while (!Cliente.idClienteExiste(clienteID));

                Cartao.ListarCartao();

                do
                {
                    Console.WriteLine($"Digite o id do cartão: ");
                    cartaoID = int.Parse(Console.ReadLine());
                } while (!Cliente.idClienteExiste(cartaoID));

                Console.WriteLine($"Digite a data início da operação: ");
                DateTime.TryParse(Console.ReadLine(), out data_inicio);

                Console.WriteLine($"Digite a data fim da operação: ");
                DateTime.TryParse(Console.ReadLine(), out data_fim);

                Console.WriteLine($"Digite a preço total da operação: ");
                preco_total = decimal.Parse(Console.ReadLine());

                string editSql = "UPDATE Operacoes SET cartaoID = @cartaoID, clienteID = @clienteID, data_inicio = @data_inicio, data_fim = @data_fim, preco_total = @preco_total WHERE operacaoID = @operacaoID";
                using (SqlCommand editCmd = new SqlCommand(editSql, conn))
                {
                    editCmd.Parameters.AddWithValue("@operacaoID", this.operacaoID);
                    editCmd.Parameters.AddWithValue("@cartaoID", cartaoID);
                    editCmd.Parameters.AddWithValue("@clienteID", clienteID);
                    editCmd.Parameters.AddWithValue("@data_inicio", data_inicio);
                    editCmd.Parameters.AddWithValue("@data_fim", data_fim);
                    editCmd.Parameters.AddWithValue("@preco_total", preco_total);

                    editCmd.ExecuteNonQuery();
                }

                return true;
            }
        }

        public static void listarOperacoes()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT operacaoID, cartaoID, clienteID, data_inicio, data_fim, preco_total FROM Operacoes";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        var rows = new List<object[]>();

                        while (reader.Read())
                        {
                            rows.Add(new object[]
                            {
                            reader["operacaoID"],
                            reader["cartaoID"],
                            reader["clienteID"],
                            reader["data_inicio"],
                            reader["data_fim"],
                            reader["preco_total"],
                            });
                        }

                        var doc = new Alba.CsConsoleFormat.Document(
                            new Grid
                            {
                                Stroke = LineThickness.Single,
                                Columns =
                                {
                                new Column{ Width = GridLength.Auto },
                                new Column{ Width = GridLength.Auto },
                                new Column{ Width = GridLength.Auto },
                                new Column{ Width = GridLength.Auto },
                                new Column{ Width = GridLength.Auto },
                                new Column{ Width = GridLength.Auto }
                                },
                                Children =
                                {
                                new Cell("operacaoID"){Color = ConsoleColor.Yellow},
                                new Cell("cartaoID"){Color = ConsoleColor.Yellow},
                                new Cell("clienteID"){Color = ConsoleColor.Yellow},
                                new Cell("data_inicio"){Color = ConsoleColor.Yellow},
                                new Cell("data_fim"){Color = ConsoleColor.Yellow},
                                new Cell("preco_total"){Color = ConsoleColor.Yellow},


                                rows.ConvertAll(r => new[]
                                {
                                    new Cell(r[0]),
                                    new Cell(r[1]),
                                    new Cell(r[2]),
                                    new Cell(r[3]),
                                    new Cell(r[4]),
                                    new Cell(r[5])
                                })
                                }
                            }
                        );

                        ConsoleRenderer.RenderDocument(doc);
                    }
                }
            }
        }

        public static Operacoes ObterOperacaoPorId(int idOperacao)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"SELECT operacaoID, cartaoID, clienteID, data_inicio, data_fim, preco_total FROM Operacoes WHERE operacaoID = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idOperacao);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Operacoes
                            {
                                OperacaoID = (int)reader["operacaoID"],
                                cartaoID = (int)reader["cartaoID"],
                                clienteID = (int)reader["clienteID"],
                                data_inicio = (DateTime)reader["data_inicio"],
                                data_fim = (DateTime)reader["data_fim"],
                                preco_total = (decimal)reader["preco_total"]
                            };
                        }
                    }
                }
            }

            return null;
        }

        #endregion
    }
}