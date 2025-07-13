using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;
using System.IO;

namespace Grupo_JCN___Pesagem_Offline_de_Fardinhos
{
    public partial class Captura : Form
    {
        private int segundos = 0;
        private int minutos = 0;
        private SerialPort serialPort;
        private string pesoAtual = "0";

        // Variáveis para configurações
        private string porta;
        private int baudRate;
        private int dataBits;
        private StopBits stopBits;
        private Parity paridade;

        private char caractereInicial;
        private int posicaoInicial;
        private int tamanho;

        // Caminho do banco SQLite (corrigido)
        private string caminhoDB = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DBFardinho.db");

        public Captura()
        {
            InitializeComponent();
            this.FormClosed += Captura_FormClosed;
        }

        private void Captura_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Start();

            // Ler configurações do arquivo config.txt
            var config = ConfigManager.LerConfiguracoes();

            // Atribuir valores com tratamento e conversão
            porta = config.ContainsKey("Porta") ? config["Porta"] : "COM3";
            baudRate = config.ContainsKey("BaudRate") && int.TryParse(config["BaudRate"], out int br) ? br : 9600;
            dataBits = config.ContainsKey("Bits") && int.TryParse(config["Bits"], out int db) ? db : 8;
            stopBits = config.ContainsKey("StopBits") && int.TryParse(config["StopBits"], out int sb) ? (StopBits)sb : StopBits.One;
            paridade = config.ContainsKey("Paridade") ? ConverterParidade(config["Paridade"]) : Parity.None;

            caractereInicial = config.ContainsKey("CaractereInicial") && !string.IsNullOrEmpty(config["CaractereInicial"]) ? config["CaractereInicial"][0] : '1';
            posicaoInicial = config.ContainsKey("PosicaoInicial") && int.TryParse(config["PosicaoInicial"], out int pi) ? pi : 1;
            tamanho = config.ContainsKey("Tamanho") && int.TryParse(config["Tamanho"], out int t) ? t : 5;

            // Configurar e abrir porta serial
            serialPort = new SerialPort(porta, baudRate, paridade, dataBits, stopBits);
            serialPort.DataReceived += SerialPort_DataReceived;

            try
            {
                serialPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir a porta serial: " + ex.Message);
            }

            // Carregar dados do banco no DataGridView
            CarregarDadosPesagem();
        }

        private Parity ConverterParidade(string p)
        {
            switch (p.ToLower())
            {
                case "none": return Parity.None;
                case "even": return Parity.Even;
                case "odd": return Parity.Odd;
                case "mark": return Parity.Mark;
                case "space": return Parity.Space;
                default: return Parity.None;
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string data = serialPort.ReadLine().Trim();
                int posIni = posicaoInicial > 0 ? posicaoInicial - 1 : 0;

                if (data.Length >= posIni + tamanho && data[posIni] == caractereInicial)
                {
                    pesoAtual = data.Substring(posIni, tamanho);
                }
                else
                {
                    pesoAtual = data;
                }

                this.Invoke(new MethodInvoker(delegate {
                    lblPeso.Text = pesoAtual;

                    // Inserir no banco
                    InserirPesagemNoBanco(pesoAtual, $"{minutos:D2}:{segundos:D2}", 1); // OperadorId = 1 (ajuste se necessário)
                    CarregarDadosPesagem();
                }));
            }
            catch
            {
                // erro silencioso
            }
        }

        private void InserirPesagemNoBanco(string peso, string tempo, int operadorId)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={caminhoDB};Version=3;"))
                {
                    conn.Open();

                    string codigoBarras = Guid.NewGuid().ToString().Substring(0, 8); // Simulação
                    string numeroOperacional = new Random().Next(1000, 9999).ToString(); // Simulação
                    string dataHora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    string sql = "INSERT INTO Pesagem (DataHora, CodigoDeBarras, NumeroOperacional, PesoBruto, Tempo, OperadorId) " +
                                 "VALUES (@DataHora, @CodigoDeBarras, @NumeroOperacional, @PesoBruto, @Tempo, @OperadorId)";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@DataHora", dataHora);
                        cmd.Parameters.AddWithValue("@CodigoDeBarras", codigoBarras);
                        cmd.Parameters.AddWithValue("@NumeroOperacional", numeroOperacional);
                        cmd.Parameters.AddWithValue("@PesoBruto", peso);
                        cmd.Parameters.AddWithValue("@Tempo", tempo);
                        cmd.Parameters.AddWithValue("@OperadorId", operadorId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao inserir no banco: " + ex.Message);
            }
        }

        private void CarregarDadosPesagem()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection($"Data Source={caminhoDB};Version=3;"))
                {
                    conn.Open();
                    string sql = "SELECT * FROM Pesagem ORDER BY Id DESC";

                    SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message);
            }
        }

        private void Captura_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort != null && serialPort.IsOpen)
                serialPort.Close();

            Login loginForm = new Login();
            loginForm.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Configuracoes configuracoesForm = new Configuracoes();
            configuracoesForm.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            segundos++;

            if (segundos == 60)
            {
                segundos = 0;
                minutos++;
            }

            lblTempo.Text = $"{minutos:D2}:{segundos:D2}";
        }

        private void lblTempo_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
