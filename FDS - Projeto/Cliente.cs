using Alba.CsConsoleFormat;
using Alba.CsConsoleFormat.Fluent;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using static System.ConsoleColor;
using Document = Alba.CsConsoleFormat.Document;

namespace FDS_ProjetoFinal
{
    public class Cliente
    {
        #region Propriedades


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        private int clienteID;

        public int ClienteID
        {
            get { return clienteID; }
            set { clienteID = value; }
        }

        [Required]
        [StringLength(100)]
        private string nome;

        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }


        [Required]
        [StringLength(9, MinimumLength = 9)]
        private string nif;

        public string Nif
        {
            get { return nif; }
            set { nif = value; }
        }


        [Required]
        private DateTime dataNascimento;

        public DateTime DataNascimento
        {
            get { return dataNascimento; }
            set { dataNascimento = value; }
        }


        [StringLength(200)]
        private string morada;

        public string Morada
        {
            get { return morada; }
            set { morada = value; }
        }


        [StringLength(9)]
        private string telemovel;

        public string Telemovel
        {
            get { return telemovel; }
            set { telemovel = value; }
        }


        [EmailAddress]
        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }


        private DateTime dataRegisto;

        public DateTime DataRegisto
        {
            get { return dataRegisto; }
            set { dataRegisto = DateTime.Now; }
        }

        #endregion

        #region Constrotures


        public Cliente()
        {

        }

        public Cliente(string nome, string nif, DateTime dataNascimento, string? morada = null, string? telemovel = null, string? email = null)
        {
            this.Nome = nome;
            this.nif = nif;
            this.DataNascimento = dataNascimento;
            this.Morada = morada;
            this.Telemovel = telemovel;
            this.Email = email;
            this.DataRegisto = DateTime.Now;
        }

        #endregion
        
        #region Metodos

        static string ConnectionString = "Password=Bdsql@123;Persist Security Info=True;User ID=Sa;Initial Catalog=FDS_Projeto;Data Source=localhost,1433;TrustServerCertificate=True;";

        public static void menuCliente()
        {
            string menuTexto =
                "   Menu do Cliente\n\n" +
                "   1. Adicionar Cliente(s).\n" +
                "   2. Listar Clientes.\n" +
                "   3. Remover um Cliente.\n" +
                "   4. Editar um Cliente\n\n" +
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

        public void CadastrarCliente()
        {

            using (SqlConnection conexao = new SqlConnection(ConnectionString))
            {
                conexao.Open();

                string query = "INSERT INTO Clientes (nome, nif, datanascimento, morada, telemovel, email) VALUES (@nome, @nif, @datanascimento, @morada, @telemovel, @email)";

                using (SqlCommand comando = new SqlCommand(query, conexao))
                {
                    comando.Parameters.AddWithValue("@nome", this.nome);
                    comando.Parameters.AddWithValue("@nif", this.nif);
                    comando.Parameters.AddWithValue("@datanascimento", this.dataNascimento);
                    comando.Parameters.AddWithValue("@morada", this.morada);
                    comando.Parameters.AddWithValue("@telemovel", this.telemovel);
                    comando.Parameters.AddWithValue("@email", this.email);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public static bool idClienteExiste(int idcliente)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT COUNT(*) FROM Clientes WHERE clienteID = @clienteID";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@clienteID", idcliente);

                    int count = (int)cmd.ExecuteScalar();

                    return count > 0;
                }
            }
        }

        public bool ApagarCliente()
        {
            if (this.clienteID == null)
                return false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "DELETE FROM Clientes WHERE clienteID = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", this.clienteID);
                    conn.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool editarCliente()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT COUNT(*) FROM Clientes WHERE clienteID = @clienteID";

                conn.Open();

                using (SqlCommand checkCmd = new SqlCommand(sql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@clienteID", this.clienteID);

                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        return false;
                    }
                }

                string nome;
                string nif;
                DateTime datanascimento;
                string morada;
                string telemovel;
                string email;


                Console.WriteLine($"Digite o nome do cliente: ");
                nome = Console.ReadLine();

                do
                {
                    Console.WriteLine($"Digite o nif do cliente: ");
                    nif = Console.ReadLine();
                } while (nif.Length != 9 || !nif.All(char.IsDigit));


                Console.WriteLine($"Digite a data de nascimento do cliente: ");
                DateTime.TryParse(Console.ReadLine(), out datanascimento);

                Console.WriteLine($"Digite a morada do cliente: ");
                morada = Console.ReadLine();

                do
                {
                    Console.WriteLine($"Digite o número telefônico: ");
                    telemovel = Console.ReadLine();
                } while (telemovel.Length != 9 || !telemovel.All(char.IsDigit));

                do
                {
                    Console.WriteLine($"Digite o email do cliente: ");
                    email = Console.ReadLine();

                } while (!email.Contains('@'));

                string editSql = "UPDATE Clientes SET nome = @nome, nif = @nif, datanascimento = @datanascimento, morada = @morada, telemovel = @telemovel, email = @email WHERE clienteID = @clienteID";
                using (SqlCommand editCmd = new SqlCommand(editSql, conn))
                {
                    editCmd.Parameters.AddWithValue("@clienteID", this.clienteID);
                    editCmd.Parameters.AddWithValue("@nome", nome);
                    editCmd.Parameters.AddWithValue("@nif", nif);
                    editCmd.Parameters.AddWithValue("@datanascimento", datanascimento);
                    editCmd.Parameters.AddWithValue("@morada", morada);
                    editCmd.Parameters.AddWithValue("@telemovel", telemovel);
                    editCmd.Parameters.AddWithValue("@email", email);

                    editCmd.ExecuteNonQuery();
                }

                return true;
            }
        }

        public static void listarCliente()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT clienteID, nome, nif, datanascimento, morada, telemovel, email FROM Clientes";

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
                            reader["clienteID"],
                            reader["nome"],
                            reader["nif"],
                            reader["datanascimento"],
                            reader["morada"],
                            reader["telemovel"],
                            reader["email"]
                            });
                        }

                        var doc = new Document(
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
                                new Column{ Width = GridLength.Auto },
                                new Column{ Width = GridLength.Auto }
                                },
                                Children =
                                {
                                new Cell("clienteID"){Color = ConsoleColor.Yellow},
                                new Cell("Nome"){Color = ConsoleColor.Yellow},
                                new Cell("nif"){Color = ConsoleColor.Yellow},
                                new Cell("datanascimento"){Color = ConsoleColor.Yellow},
                                new Cell("morada"){Color = ConsoleColor.Yellow},
                                new Cell("telemovel"){Color = ConsoleColor.Yellow},
                                new Cell("email"){Color = ConsoleColor.Yellow},


                                rows.ConvertAll(r => new[]
                                {
                                    new Cell(r[0]),
                                    new Cell(r[1]),
                                    new Cell(r[2]),
                                    new Cell(r[3]),
                                    new Cell(r[4]),
                                    new Cell(r[5]),
                                    new Cell(r[6])
                                })
                                }
                            }
                        );

                        ConsoleRenderer.RenderDocument(doc);
                    }
                }
            }
        }
        public static Cliente ObterClientePorId(int idcliente)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT clienteID, nome, nif, datanascimento, morada, telemovel, email FROM Clientes WHERE clienteID = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idcliente);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Cliente
                            {
                                clienteID = (int)reader["clienteID"],
                                Nome = reader["nome"].ToString(),
                                Nif = (string)reader["nif"],
                                DataNascimento = (DateTime)reader["datanascimento"],
                                Morada = reader["morada"].ToString(),
                                Telemovel = (string)reader["telemovel"],
                                Email = reader["email"].ToString()
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