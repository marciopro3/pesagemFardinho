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
    public partial class Cadastro : Form
    {
        public Cadastro()
        {
            InitializeComponent();
            this.Load += Cadastro_Load;
        }

        private void Cadastro_Load(object sender, EventArgs e)
        {
            // Define os textos iniciais e cor cinza
            txtNome.Text = "Nome completo";
            txtNome.ForeColor = Color.Gray;

            txtUsuario.Text = "E-mail";
            txtUsuario.ForeColor = Color.Gray;

            txtSenha.Text = "Senha";
            txtSenha.ForeColor = Color.Gray;
            txtSenha.UseSystemPasswordChar = false;

            // Associa eventos
            txtNome.Enter += txtNome_Enter;
            txtNome.Leave += txtNome_Leave;

            txtUsuario.Enter += txtUsuario_Enter;
            txtUsuario.Leave += txtUsuario_Leave;

            txtSenha.Enter += txtSenha_Enter;
            txtSenha.Leave += txtSenha_Leave;
        }

        private void txtNome_Enter(object sender, EventArgs e)
        {
            if (txtNome.Text == "Nome completo")
            {
                txtNome.Text = "";
                txtNome.ForeColor = Color.Black;
            }
        }

        private void txtNome_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                txtNome.Text = "Nome completo";
                txtNome.ForeColor = Color.Gray;
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            // Cria uma nova instância do formulário Login
            Login loginForm = new Login();

            // Mostra o Login
            loginForm.Show();

            // Fecha o formulário atual (Cadastro)
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtUsuario_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            // Limpar campo Nome
            txtNome.Text = "Nome completo";
            txtNome.ForeColor = Color.Gray;

            // Limpar campo E-mail
            txtUsuario.Text = "E-mail";
            txtUsuario.ForeColor = Color.Gray;

            // Limpar campo Senha
            txtSenha.Text = "Senha";
            txtSenha.ForeColor = Color.Gray;
            txtSenha.UseSystemPasswordChar = false;
        }

        private void btnVerSenha_Click(object sender, EventArgs e)
        {
            txtSenha.UseSystemPasswordChar = false;
            btnVerSenha.Visible = false;
            btnDesverSenha.Visible = true;
        }

        private void btnDesverSenha_Click(object sender, EventArgs e)
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

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (txtNome.Text == "Nome completo" || string.IsNullOrWhiteSpace(txtNome.Text) ||
        txtUsuario.Text == "E-mail" || string.IsNullOrWhiteSpace(txtUsuario.Text) ||
        txtSenha.Text == "Senha" || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Preencha todos os campos!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nome = txtNome.Text.Trim();
            string login = txtUsuario.Text.Trim();
            string senha = txtSenha.Text.Trim();

            string caminhoCompleto = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DBFardinho.db");
            string caminhoBanco = $"Data Source={caminhoCompleto};Version=3;";

            using (SQLiteConnection conexao = new SQLiteConnection(caminhoBanco))
            {
                try
                {
                    conexao.Open();

                    string sql = "INSERT INTO Operador (Nome, Login, Senha) VALUES (@nome, @login, @senha)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conexao))
                    {
                        cmd.Parameters.AddWithValue("@nome", nome);
                        cmd.Parameters.AddWithValue("@login", login);
                        cmd.Parameters.AddWithValue("@senha", senha);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Usuário cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Abrir tela de Login
                    Login loginForm = new Login();
                    loginForm.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao cadastrar usuário: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
