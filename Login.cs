using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grupo_JCN___Pesagem_Offline_de_Fardinhos
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.Load += Login_Load;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // Define os textos iniciais e cor cinza
            txtUsuario.Text = "E-mail";
            txtUsuario.ForeColor = Color.Gray;

            txtSenha.Text = "Senha";
            txtSenha.ForeColor = Color.Gray;
            txtSenha.UseSystemPasswordChar = false;

            // Associa eventos
            txtUsuario.Enter += txtUsuario_Enter;
            txtUsuario.Leave += txtUsuario_Leave;

            txtSenha.Enter += txtSenha_Enter;
            txtSenha.Leave += txtSenha_Leave;
        }

        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "E-mail")
            {
                txtUsuario.Text = "";
                txtUsuario.ForeColor = Color.Black;
            }
        }

        private void txtUsuario_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                txtUsuario.Text = "E-mail";
                txtUsuario.ForeColor = Color.Gray;
            }
        }

        private void txtSenha_Enter(object sender, EventArgs e)
        {
            if (txtSenha.Text == "Senha")
            {
                txtSenha.Text = "";
                txtSenha.ForeColor = Color.Black;
                txtSenha.UseSystemPasswordChar = true;  // **oculta os caracteres com asterisco**
            }
        }

        private void txtSenha_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                txtSenha.UseSystemPasswordChar = false; // mostra o texto "Senha" sem asterisco
                txtSenha.Text = "Senha";
                txtSenha.ForeColor = Color.Gray;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        { 
            Application.Exit();  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "E-mail" || string.IsNullOrWhiteSpace(txtUsuario.Text) ||
        txtSenha.Text == "Senha" || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Por favor, preencha todos os campos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string caminhoCompleto = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DBFardinho.db");
            string caminhoBanco = $"Data Source={caminhoCompleto};Version=3;";
            using (SQLiteConnection conexao = new SQLiteConnection(caminhoBanco))
            {
                try
                {
                    conexao.Open();

                    string sql = "SELECT * FROM Operador WHERE Login = @login AND Senha = @senha";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@login", txtUsuario.Text);
                        cmd.Parameters.AddWithValue("@senha", txtSenha.Text);

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string nomeOperador = reader["Nome"].ToString();
                                MessageBox.Show($"Bem-vindo, {nomeOperador}!", "Login realizado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                Captura capturaForm = new Captura();
                                capturaForm.Show();

                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Usuário ou senha inválidos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao conectar no banco de dados:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Cria uma instância do formulário Cadastro
            Cadastro cadastroForm = new Cadastro();

            // Abre o formulário de cadastro como uma nova janela
            cadastroForm.Show();

            // Opcional: você pode esconder a janela de login se quiser
            // this.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            // Limpar campo E-mail
            txtUsuario.Text = "E-mail";
            txtUsuario.ForeColor = Color.Gray;

            // Limpar campo Senha
            txtSenha.Text = "Senha";
            txtSenha.ForeColor = Color.Gray;
            txtSenha.UseSystemPasswordChar = false;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            txtSenha.UseSystemPasswordChar = true;
            btnVerSenha.Visible = true;
            btnDesverSenha.Visible = false;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (txtSenha.UseSystemPasswordChar)
            {
                // Mostrar senha
                txtSenha.UseSystemPasswordChar = false;
                btnVerSenha.Text = "Ocultar";
            }
            else
            {
                // Ocultar senha
                if (txtSenha.Text != "Senha")  // só oculta se não for o placeholder
                    txtSenha.UseSystemPasswordChar = true;

                btnVerSenha.Text = "Mostrar";
            }
        }
    }
}
