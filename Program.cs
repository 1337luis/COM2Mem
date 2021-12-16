/**
 * COM2Mem
 * Programa de lanzamiento de instrucciones por puerto Serie
 * 
 * @author Luis Santos  <luis1337@outlook.es>
 * @version 0.0.3a
 */

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace COM2Mem
{
    internal class Program
    {
        static void Main( string[] args )
        {
            COM2Mem c2m = new COM2Mem();

            // Hasta que termine la ejecución
            while( !c2m.isHalted )
            {
                c2m.loop(); // Lanzamos el bucle
            }

        }
    }

    /**
     * Clase COM2Mem, conforma la interfaz principal
     */
    class COM2Mem
    {
        private List<MemPort> ports;    // Lista de puertos
        private int timeSleep;          // Tiempo de actualización

        // Indica si termina la ejecución del programa
        public bool isHalted { get; set; } = false;

        /**
         * Constructor principal de la clase
         */
        public COM2Mem()
        {
            ports = this.getPortList();
            timeSleep = 200;
        }

        /**
         * Bucle principal, se debe de lanzar contínuamente
         */
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

        /**
         * Muestra la cabecera de la interfaz
         */
        private void displayHeader()
        {
            Console.WriteLine(
                $"Tasa de actualización: {timeSleep} ms"
            );
        }

    }

    /**
     * Clase de abstracción de los puertos
     */
    class MemPort
    {
        public SerialPort port;     // Puerto real
        private int toOpen = 0;     // Comando de apertura
        private int toClose = 0;    // Comando de cierre
        private int toSend = 0;     // Comando de envío
        private string toSendCmd;   // String del comando

        /**
         * Inicializar cada puerto con su nombre
         * 
         * @param String portName Nombre del puerto
         */
        public MemPort( String portName )
        {
            port = new SerialPort( portName );
            toSendCmd = $"CMD#{portName}\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0";
        }

        /**
         * Muestra las opciones para cada puerto
         */
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
