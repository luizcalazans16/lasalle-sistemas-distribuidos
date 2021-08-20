using System;
using System.Net.Sockets;
using System.Threading;


namespace TCPClient
{
    class Cliente
    {
        public static void Enviar(Socket socket, byte[] buffer, int offset, int tamanho, int timeout)
        {
            int iniciaContagemTick = Environment.TickCount;

            int enviados = 0;
            do
            {
                if (Environment.TickCount > iniciaContagemTick + timeout)
                    throw new Exception("Tempo esgotado");
                try
                {
                    enviados += socket.Send(buffer, offset + enviados, tamanho - enviados, SocketFlags.None);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.WouldBlock ||
                        ex.SocketErrorCode == SocketError.IOPending ||
                        ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                    {
                        Thread.Sleep(30);
                    }
                    else
                    {
                        throw ex;
                    }
                }
            } while (enviados < tamanho);
        }
    }
}
