using System.Text;
using System;

namespace BaiTapLTM_Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            Server server = new Server();

            server.Start();

            Console.ReadLine();
        }
    }
}