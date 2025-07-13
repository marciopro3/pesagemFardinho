using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Grupo_JCN___Pesagem_Offline_de_Fardinhos
{
    public partial class Configuracoes : Form
    {
        public Configuracoes()
        {
            InitializeComponent();
        }

        private void Configuracoes_Load(object sender, EventArgs e)
        {
            SetPlaceholder(txtLimiteInicial, "Peso Limite Inicial");
            SetPlaceholder(txtLimiteFinal, "Peso Limite Final");
            SetPlaceholder(textTara, "Peso Tara");
            SetPlaceholder(textPorta, "Porta (ex: COM3)");
            SetPlaceholder(textBaudRate, "Baud Rate (ex: 9600)");
            SetPlaceholder(textBits, "Data Bits");
            SetPlaceholder(textStopBits, "Stop Bits");
            SetPlaceholder(textParidade, "Paridade (None, Even...)");
            SetPlaceholder(textCinicial, "Caractere Inicial");
            SetPlaceholder(textCfinal, "Posição Inicial");
            SetPlaceholder(textTamanho, "Tamanho");

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox txt && txt.Tag != null)
                {
                    txt.Enter += TextBox_Enter;
                    txt.Leave += TextBox_Leave;
                }
            }

            CarregarConfiguracoes();
        }

        private void CarregarConfiguracoes()
        {
            var config = ConfigManager.LerConfiguracoes();
            if (config.Count > 0)
            {
                txtLimiteInicial.Text = ObterValorOuPadrao(config, "LimiteInicial", txtLimiteInicial.Tag.ToString());
                txtLimiteFinal.Text = ObterValorOuPadrao(config, "LimiteFinal", txtLimiteFinal.Tag.ToString());
                textTara.Text = ObterValorOuPadrao(config, "Tara", textTara.Tag.ToString());
                textPorta.Text = ObterValorOuPadrao(config, "Porta", textPorta.Tag.ToString());
                textBaudRate.Text = ObterValorOuPadrao(config, "BaudRate", textBaudRate.Tag.ToString());
                textBits.Text = ObterValorOuPadrao(config, "Bits", textBits.Tag.ToString());
                textStopBits.Text = ObterValorOuPadrao(config, "StopBits", textStopBits.Tag.ToString());
                textParidade.Text = ObterValorOuPadrao(config, "Paridade", textParidade.Tag.ToString());
                textCinicial.Text = ObterValorOuPadrao(config, "CaractereInicial", textCinicial.Tag.ToString());
                textCfinal.Text = ObterValorOuPadrao(config, "PosicaoInicial", textCfinal.Tag.ToString());
                textTamanho.Text = ObterValorOuPadrao(config, "Tamanho", textTamanho.Tag.ToString());
            }
        }

        private string ObterValorOuPadrao(Dictionary<string, string> dict, string chave, string padrao)
        {
            if (dict.ContainsKey(chave))
                return dict[chave];
            return padrao;
        }

        private void SetPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;
            textBox.Tag = placeholder;
        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt != null && txt.Text == (string)txt.Tag)
            {
                txt.Text = "";
                txt.ForeColor = Color.Black;
            }
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt != null && string.IsNullOrWhiteSpace(txt.Text))
            {
                txt.Text = (string)txt.Tag;
                txt.ForeColor = Color.Gray;
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            var config = new Dictionary<string, string>
            {
                ["LimiteInicial"] = txtLimiteInicial.Text,
                ["LimiteFinal"] = txtLimiteFinal.Text,
                ["Tara"] = textTara.Text,
                ["Porta"] = textPorta.Text,
                ["BaudRate"] = textBaudRate.Text,
                ["Bits"] = textBits.Text,
                ["StopBits"] = textStopBits.Text,
                ["Paridade"] = textParidade.Text,
                ["CaractereInicial"] = textCinicial.Text,
                ["PosicaoInicial"] = textCfinal.Text,
                ["Tamanho"] = textTamanho.Text
            };

            try
            {
                ConfigManager.SalvarConfiguracoes(config);
                MessageBox.Show("Configurações salvas com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar as configurações: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Métodos vazios para os eventos TextChanged que o Designer espera:
        private void textCfinal_TextChanged(object sender, EventArgs e) { }
        private void textCinicial_TextChanged(object sender, EventArgs e) { }
        private void textParidade_TextChanged(object sender, EventArgs e) { }
        private void textStopBits_TextChanged(object sender, EventArgs e) { }
        private void textBits_TextChanged(object sender, EventArgs e) { }
        private void textBaudRate_TextChanged(object sender, EventArgs e) { }
        private void textPorta_TextChanged(object sender, EventArgs e) { }
        private void textTara_TextChanged(object sender, EventArgs e) { }
        private void txtLimiteFinal_TextChanged(object sender, EventArgs e) { }
        private void txtLimiteInicial_TextChanged(object sender, EventArgs e) { }
        private void textTamanho_TextChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
    }
}