using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcesadorNominaas
{
    public class Empleado
    {
        private int numero;
        private string nombre;
        private string entrada;
        private string salida;
        private string dispositivos;
        private string horario;
        public string[] anotaciones = new string[7];
        public int descanso = 0;
        public string trabajoDescanso;
        public string anotacionesGenerales;
        public int[] matriz = new int[35];
        public double horas = 0;


        public int Numero { get => numero; set => numero = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Entrada { get => entrada; set => entrada = value; }
        public string Salida { get => salida; set => salida = value; }
        public string Dispositivos { get => dispositivos; set => dispositivos = value; }
        public string Horario { get => horario; set => horario = value; }


        public Empleado( int numero, string nombre, string entrada, string salida, string dispositivos) {
            this.numero = numero;
            this.nombre = nombre;
            this.entrada = entrada;
            this.salida = salida;
            this.dispositivos = dispositivos;
        }

        public Empleado(string nombre, int numero, string horario, int descanso)
        {
            this.nombre = nombre;
            this.numero = numero;
            this.horario = horario;
            this.descanso = descanso;
        }

       public Empleado() { }



    }
}
