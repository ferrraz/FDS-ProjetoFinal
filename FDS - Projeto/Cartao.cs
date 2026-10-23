using Alba.CsConsoleFormat;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDS___Projeto
{
    public class Cartao
    {
        #region Propriedades
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        private int id_cartao;

        public int CartaoId
        {
            get { return id_cartao; }
            set { id_cartao = value; }
        }

        [Required]
        [StringLength(20)]
        private string tipo;

        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        #endregion

        #region Metodos

        static string ConnectionString = "Password=Bdsql@123;Persist Security Info=True;User ID=Sa;Initial Catalog=FDS_Projeto;Data Source=localhost,1433;TrustServerCertificate=True;";

        public static void menuCartao()
        {
            string menuTexto =
                "   Menu do Cartão\n\n" +
                "   1. Adicionar Cartão(s).\n" +
                "   2. Listar Cartões.\n" +
                "   3. Remover um Cartão.\n\n" +
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

        public virtual void InserirCartao()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"INSERT INTO Cartao (tipo, saldo, limite, taxa_Juros) VALUES (@tipo, @saldo, @limite, @taxaJuros)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@tipo", Tipo);
                    cmd.Parameters.AddWithValue("@saldo", DBNull.Value);
                    cmd.Parameters.AddWithValue("@limite", DBNull.Value);
                    cmd.Parameters.AddWithValue("@taxaJuros", DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void ListarCartao()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT id_Cartao, tipo, saldo, limite, taxa_Juros FROM Cartao";

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
                        reader["id_Cartao"],
                        reader["tipo"],
                        reader["saldo"] == DBNull.Value ? "-" : reader["saldo"],
                        reader["limite"] == DBNull.Value ? "-" : reader["limite"],
                        reader["taxa_Juros"] == DBNull.Value ? "-" : reader["taxa_Juros"]
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
                                },
                                Children =
                                {
                            new Cell("ID"){ Color = ConsoleColor.Yellow },
                            new Cell("Tipo"){ Color = ConsoleColor.Yellow },
                            new Cell("Saldo"){ Color = ConsoleColor.Yellow },
                            new Cell("Limite"){ Color = ConsoleColor.Yellow },
                            new Cell("Taxa Juros"){ Color = ConsoleColor.Yellow },

                            rows.ConvertAll(r => new[]
                            {
                                new Cell(r[0]),
                                new Cell(r[1]),
                                new Cell(r[2]),
                                new Cell(r[3]),
                                new Cell(r[4]),
                            })
                                }
                            }
                        );

                        ConsoleRenderer.RenderDocument(doc);
                    }
                }
            }
        }

        public bool ApagarCartao()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                string checkSql = "SELECT COUNT(*) FROM Cartao WHERE id_Cartao = @id";

                using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                {
                    checkCmd.Parameters.AddWithValue("@id", this.CartaoId);

                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        return false;
                    }
                }

                string deleteSql = "DELETE FROM Cartao WHERE id_Cartao = @id";

                using (SqlCommand deleteCmd = new SqlCommand(deleteSql, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@id", this.CartaoId);
                    deleteCmd.ExecuteNonQuery();
                }

                return true;
            }
        }

        public static bool CartaoExiste(int idCartao)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT COUNT(*) FROM Cartao WHERE id_Cartao = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@id", idCartao);

                    int count = (int)cmd.ExecuteScalar();

                    return count > 0;
                }
            }
        }

        public static Cartao ObterCartaoPorId(int idCartao)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"SELECT id_Cartao, tipo, saldo, limite, taxa_juros FROM Cartao WHERE id_Cartao = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idCartao);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string tipo = reader["tipo"].ToString();

                            Cartao cartao;

                            if (tipo == "Crédito")
                            {
                                cartao = new CartaoCredito
                                {
                                    Saldo = reader["saldo"] != DBNull.Value
                                        ? (decimal)reader["saldo"]
                                        : 0
                                };
                            }
                            else
                            {
                                cartao = new CartaoDebito
                                {
                                    Limite = reader["limite"] != DBNull.Value
                                        ? (decimal)reader["limite"]
                                        : 0,
                                    Taxas_Juros = reader["taxas_juros"] != DBNull.Value
                                        ? (decimal)reader["taxas_juros"]
                                        : 0
                                };
                            }

                            cartao.CartaoId = (int)reader["id_Cartao"];
                            cartao.Tipo = tipo;

                            return cartao;
                        }
                    }
                }
            }

            return null;
        }


        #endregion
    }

    public class CartaoCredito : Cartao
    {
        private decimal saldo;

        public decimal Saldo
        {
            get { return saldo; }
            set { saldo = value; }
        }

        static string ConnectionString = "Password=Bdsql@123;Persist Security Info=True;User ID=Sa;Initial Catalog=FDS_Projeto;Data Source=localhost,1433;TrustServerCertificate=True;";

        public override void InserirCartao()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"INSERT INTO Cartao (tipo, saldo, [limite], taxa_Juros) VALUES (@tipo, @saldo, @limite, @taxaJuros)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@tipo", SqlDbType.VarChar).Value = Tipo ?? "Credito";
                    cmd.Parameters.Add("@saldo", SqlDbType.Decimal).Value = Saldo;
                    cmd.Parameters.Add("@limite", SqlDbType.Decimal).Value = 0;
                    cmd.Parameters.Add("@taxaJuros", SqlDbType.Decimal).Value = 0;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    public class CartaoDebito : Cartao
    {
        private decimal limite;

        public decimal Limite
        {
            get { return limite; }
            set { limite = value; }
        }

        private decimal taxas_Juros;

        public decimal Taxas_Juros
        {
            get { return taxas_Juros; }
            set { taxas_Juros = value; }
        }

        static string ConnectionString = "Password=Bdsql@123;Persist Security Info=True;User ID=Sa;Initial Catalog=FDS_Projeto;Data Source=localhost,1433;TrustServerCertificate=True;";

        public override void InserirCartao()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = @"INSERT INTO dbo.Cartao (tipo, saldo, [limite], taxa_Juros) VALUES (@tipo, @saldo, @limite, @taxaJuros)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@tipo", SqlDbType.VarChar).Value = Tipo ?? "Debito";
                    cmd.Parameters.Add("@saldo", SqlDbType.Decimal).Value = 0;
                    cmd.Parameters.Add("@limite", SqlDbType.Decimal).Value = Limite;
                    cmd.Parameters.Add("@taxaJuros", SqlDbType.Decimal).Value = Taxas_Juros;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
