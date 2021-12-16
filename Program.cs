using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace COM2Mem
{
    internal class Program
    {
        static void Main( string[] args )
        {
            COM2Mem c2m = new COM2Mem();

            while( !c2m.isHalted )
            {
                c2m.loop();
            }

        }
    }

    class COM2Mem
    {
        private List<MemPort> ports;
        private int timeSleep;

        public bool isHalted { get; set; } = false;

        public COM2Mem()
        {
            ports = this.getPortList();
            timeSleep = 200;
        }

        public void loop()
        {
            displayHeader();

            MemPort.displayHeader();

            foreach( MemPort port in ports )
            {
                port.display();
            }

            Thread.Sleep( this.timeSleep );
            Console.Clear();
        }

        /**
         * Obtiene la lista de puertos
         */
        private List<MemPort> getPortList()
        {
            List<MemPort> spList = new List<MemPort>();
            string[] ports = SerialPort.GetPortNames();

            foreach( String port in ports )
            {
                spList.Add( new MemPort( port ) );
            }

            return spList;
        }

        private void displayHeader()
        {
            Console.WriteLine(
                $"Tasa de actualización: {timeSleep} ms"
            );
        }

    }

    class MemPort
    {
        public SerialPort port;
        private int toOpen = 0;
        private int toClose = 0;
        private int toSend = 0;
        private string toSendCmd;


        public MemPort( String portName )
        {
            port = new SerialPort( portName );
            toSendCmd = $"CMD#{portName}\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0";
        }

        public void display()
        {
            Console.WriteLine(
                $"{port.PortName}\t" +
                $"{( port.IsOpen ? "Abierto" : "Cerrado" )}\t" +
                $"[{toOpen}]\t" +
                $"[{toClose}]\t" +
                $"[{toSend}]\t" +
                $"[{toSendCmd}]"
            );
        }

        /**
         * Muestra la cabecera de información
         */
        public static void displayHeader()
        {
            Console.WriteLine(
                "Puerto\tEstado\tAbrir\tCerrar\tEnviar\tComando 32 chars"
            );
        }
    }

}
