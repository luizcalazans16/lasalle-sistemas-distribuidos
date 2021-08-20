using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPClient
{
    public partial class Form1 : Form
    {

        private static string nomeAbreviado = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnProcurar_Click(object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Envio de Arquivo";
            dialog.ShowDialog();
            txtArquivo.Text = dialog.FileName;
            nomeAbreviado = dialog.SafeFileName;
        }

        private void btnEnviarArquivo_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(txtEnderecoIP.Text) &&
                string.IsNullOrEmpty(txtPortaHost.Text) &&
                string.IsNullOrEmpty(txtArquivo.Text)) {
                MessageBox.Show("Dados inválidos...");
                return;
            }

            string enderecoIP = txtEnderecoIP.Text;
            int porta = int.Parse(txtPortaHost.Text);
            string nomeArquivo = txtArquivo.Text;

            try
            {
                Task.Factory.StartNew(() => EnviarArquivo(enderecoIP, porta, nomeArquivo, nomeAbreviado));
                MessageBox.Show("Arquivo enviado com sucesso");
            } catch(Exception ex)
            {

            }

        }

        private void EnviarArquivo(string IPHostRemoto, int portaHostRemoto, string nomeCaminhoArquivo, string nomeAbreviadoArquivo)
{
            try
            {
                if (!string.IsNullOrEmpty(IPHostRemoto))
                {
                    byte[] fileNameByte = Encoding.ASCII.GetBytes(nomeAbreviadoArquivo);
                    byte[] fileData = File.ReadAllBytes(nomeCaminhoArquivo);
                    byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];
                    byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);
                    //
                    fileNameLen.CopyTo(clientData, 0);
                    fileNameByte.CopyTo(clientData, 4);
                    fileData.CopyTo(clientData, 4 + fileNameByte.Length);
                    //
                    TcpClient clientSocket = new TcpClient(IPHostRemoto, portaHostRemoto);
                    NetworkStream networkStream = clientSocket.GetStream();
                    //
                    networkStream.Write(clientData, 0, clientData.GetLength(0));
                    networkStream.Close();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
