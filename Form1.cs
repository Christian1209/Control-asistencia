using Microsoft.Office.Interop.Excel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

using Excel = Microsoft.Office.Interop.Excel;

namespace ProcesadorNominaas
{
    public partial class Form1 : Form
    {
        List<Empleado> empleados = new List<Empleado>();
        List<Empleado> noRegistrados = new List<Empleado>();
        string ruta;
        bool exito = false;
        int dia = 1;
        public Form1()
        {
            InitializeComponent();
         

        }

        public void estiloBotones()
        {
            btnProcesar.FlatStyle = FlatStyle.Flat; // Establece el estilo del botón en Flat para mostrar el borde personalizado
            btnProcesar.FlatAppearance.BorderColor = Color.DodgerBlue; // Establece el color del borde en rojo
            btnProcesar.FlatAppearance.BorderSize = 3; // Establece el grosor del borde en 2 píxeles
            btnUbicacion.FlatStyle = FlatStyle.Flat; // Establece el estilo del botón en Flat para mostrar el borde personalizado
            btnUbicacion.FlatAppearance.BorderColor = Color.DodgerBlue; // Establece el color del borde en rojo
            btnUbicacion.FlatAppearance.BorderSize = 3; // Establece el grosor del borde en 2 píxeles
        }


        public string[] ordenaPorFecha(string [] archivos)
        {
            string[] archivos2 = new string[7];

            for (int i = 0; i < archivos.Length; i++)
            {
                string nombreArchivo = Path.GetFileName(archivos[i]);
                if (nombreArchivo[nombreArchivo.Length - 1] != 'x')
                {
                    string nombreArchivo2 = nombreArchivo.Substring(0, nombreArchivo.Length - 4);
                    archivos2[i] = nombreArchivo2;
                }

            }

            string format = "ddMMyy";
            DateTime[] dates = new DateTime[archivos2.Length];
            for (int i = 0; i< archivos2.Length; i++)
            {
                if (DateTime.TryParseExact(archivos2[i], format, null, System.Globalization.DateTimeStyles.None, out DateTime date))
                {
                    dates[i] = date;
                }
                else
                {
                    Console.WriteLine("El formato de fecha no es válido para el elemento en el índice " + i);
                }
            }
            Array.Sort(dates);
            int a = 0;
            foreach (DateTime date in dates)
            {
               archivos2[a] = date.ToString("ddMMyy") + ".xls" ;
                a++;
            }
            return archivos2;
        }

        public void cargaArchivos()
        {
            string[] archivos2 = new string[7];
            string[] archivos = Directory.GetFiles(ruta);

            int posicion = 0;
            archivos2 = ordenaPorFecha(archivos);
            for (int i = 0; i < archivos2.Length; i++ )
            {
                string nombreArchivo =  ruta + @"\" + archivos2[i];
                cargaEntradaSalida(nombreArchivo);
                procesaEmpleado(posicion);
                limpiaEmpleados();
                posicion++;
            }
        }

        public void limpiaEmpleados()
        {
            for(int i = 0; i< empleados.Count; i++ )
            {
                empleados[i].Entrada = null;
                empleados[i].Salida = null;
            }
        }

        public void cargaEmpleados()
        {
            string rutaHorario =  ruta + @"\Horarios.xlsx";
            //try 
            try
            {
                FileStream test = new FileStream(rutaHorario, FileMode.Open, FileAccess.Read);
                test.Close();
                exito = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se logró abrir el archivo de Horarios en la carpeta, verifica la ruta, que el archivo exista, y que no este abierto en este momento.");
                exito = false;
                return;
            }

            FileStream excelStream = new FileStream(rutaHorario, FileMode.Open, FileAccess.Read);
            var book = new XSSFWorkbook(excelStream);

            excelStream.Close();

            var sheet = book.GetSheetAt(0);
            var headerRow = sheet.GetRow(0);
            //var cellCount = headerRow.LastCellNum;

            //auxiliar para evitar duplicados leyendo el id anterior.
            var rowCount = sheet.LastRowNum;
            int id = 0;
            for (int i = sheet.FirstRowNum + 1; i < rowCount + 1; i++)
            {
                var row = sheet.GetRow(i);
                if (row != null)
                {
                    int numero = 0;
                    string nombre = row.Cells[0].RichStringCellValue.String;

                    //numero
                    try
                    {
                        if (row.Cells[1].CellType == CellType.String && row.Cells[1].RichStringCellValue != null)
                        {
                            if (row.Cells[1].RichStringCellValue.String != "")
                                numero = int.Parse(row.Cells[1].RichStringCellValue.String);
                        }
                        else
                        {
                            numero = int.Parse(row.Cells[1].NumericCellValue.ToString());
                        }

                    }
                    catch (Exception ex)
                    {
                        numero = int.Parse(row.Cells[1].NumericCellValue.ToString());
                    }

                    //horario
                    string horario = row.Cells[2].DateCellValue.TimeOfDay.ToString();

                    //descanso
                    int descanso = -1;
                    try
                    {
                        if (row.Cells[3].CellType == CellType.String && row.Cells[3].RichStringCellValue != null)
                        {
                            if (row.Cells[3].RichStringCellValue.String != "")
                                descanso = int.Parse(row.Cells[3].RichStringCellValue.String);
                        }
                        else
                        {
                            descanso = int.Parse(row.Cells[3].NumericCellValue.ToString());
                        }

                    }
                    catch (Exception ex)
                    {
                        descanso = int.Parse(row.Cells[3].NumericCellValue.ToString());
                    }

                    Empleado auxiliar = new Empleado(nombre, numero, horario, descanso);
                    empleados.Add(auxiliar);
                }
            }
        }

        public void cargaEntradaSalida(string ruta)
        {
            /// <summary>
            /// Using Microsoft.Office.Interop to convert XLS to XLSX format, to work with EPPlus library
            /// </summary>
            /// <param name="filesFolder"></param>
            var app = new Microsoft.Office.Interop.Excel.Application();
            var xlsFile = ruta;
            var workbooks = app.Workbooks;
            var workbook = workbooks.Open(xlsFile);
            var xlsxFile = xlsFile + "x";
            
            workbook.SaveAs(Filename: xlsxFile, FileFormat: Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook);
            workbook.Close();
            app.Quit();

            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(workbooks);
            Marshal.ReleaseComObject(app);

            FileStream excelStream = new FileStream(xlsxFile, FileMode.Open, FileAccess.Read); 
            var book = new XSSFWorkbook(excelStream);

            excelStream.Close();

            var sheet = book.GetSheetAt(0);
            var headerRow = sheet.GetRow(0);
            //var cellCount = headerRow.LastCellNum;

            //auxiliar para evitar duplicados leyendo el id anterior.
            var rowCount = sheet.LastRowNum;
            int id = -1;
            bool nuevaSalida = true;
            for (int i = sheet.FirstRowNum + 1; i < rowCount + 1; i++)
            {

                var row = sheet.GetRow(i);
                if (row != null)
                {
                    int id2 = 0;
                    if (row.Cells.Count > 0)
                        id2 = int.Parse(row.Cells[0].RichStringCellValue.String);
                    //pruebas.
                    if (id2 == 220)
                    {
                        int a = 0;
                    }

                    if (id == -1)
                    {
                        id = int.Parse(row.Cells[0].RichStringCellValue.String);
                            nuevaSalida = true;

                            int numero = int.Parse(row.Cells[0].RichStringCellValue.String);
                        string nombre = row.Cells[1].RichStringCellValue.String;
                        string entrada = row.Cells[2].RichStringCellValue.String;
                        string estado = row.Cells[3].RichStringCellValue.String;
                        string dispositvos = row.Cells[4].RichStringCellValue.String;

                        Empleado auxiliar = new Empleado();
                        auxiliar = obtenerEmpleado(id, nombre);
                        if(estado == "Entrada")
                            auxiliar.Entrada = entrada;
                        if (estado == "Salida")
                            auxiliar.Salida = entrada; 

                    }
                    else
                    {
                        try
                        {

                            if (id != int.Parse(row.Cells[0].RichStringCellValue.String) && (row.Cells[3].RichStringCellValue.String == "Entrada") )
                            {
                                id = int.Parse(row.Cells[0].RichStringCellValue.String);

                                int numero = int.Parse(row.Cells[0].RichStringCellValue.String);
                                string nombre = row.Cells[1].RichStringCellValue.String;
                                string entrada = row.Cells[2].RichStringCellValue.String;
                                string dispositvos = row.Cells[4].RichStringCellValue.String;
                                string estado = row.Cells[3].RichStringCellValue.String;
                                Empleado auxiliar = new Empleado();
                                auxiliar = obtenerEmpleado(id, nombre);
                                if (estado == "Entrada")
                                {
                                    auxiliar.Entrada = entrada;
                                    nuevaSalida = true;
                                }
                                if (estado == "Salida")
                                    auxiliar.Salida = entrada;
                            }
                            else
                            {
                                if (id != int.Parse(row.Cells[0].RichStringCellValue.String))
                                {
                                    id = int.Parse(row.Cells[0].RichStringCellValue.String);
                                    nuevaSalida = true;
                                }

                                if (row.Cells[3].RichStringCellValue.String == "Salida" && nuevaSalida)
                                {
                                    int numero = int.Parse(row.Cells[0].RichStringCellValue.String);
                                    string nombre = row.Cells[1].RichStringCellValue.String;
                                    string salida = row.Cells[2].RichStringCellValue.String;
                                    nuevaSalida = false;

                                    Empleado auxiliar = new Empleado();
                                    auxiliar = obtenerEmpleado(id, nombre);
                                    auxiliar.Salida = salida;
                                }
                            }
                        }
                        catch
                        {

                        }

                    }

                }
            }

            File.Delete(xlsxFile);


        }

        public Empleado obtenerEmpleado(int id, string nombre)
        {
            Empleado auxiliar = new Empleado();
            for(int i = 0; i< empleados.Count; i++)
            {
                if (empleados[i].Numero == id)
                {
                    auxiliar = empleados[i];
                    return auxiliar;
                }
            }

            if (!yaEraNoRegistrado(id))
            {
                auxiliar.Numero = id;
                auxiliar.Nombre = nombre;
                noRegistrados.Add(auxiliar);
                //AVISO
                //MessageBox.Show("El empleado: " + nombre + ". Con Numero: " + id + ". No se encuentra en el archivo de Horario, añade el empleado al archivo, o en caso de que ya exista revisa que el id coincida y corre de nuevo el programa. ");
                return auxiliar;
            }

            return auxiliar;
        }

        public bool yaEraNoRegistrado( int id )
        {
            for(int i = 0; i< noRegistrados.Count; i++)
            {
                if (noRegistrados[i].Numero == id)
                    return true;
            }
            return false;

        }

        public void procesaEmpleado(int posicion)
        {
            for (int i = 0; i < empleados.Count; i++)
            {
                trabajaronDescanso(empleados);
                obtenAsistencia(empleados[i], posicion);
                obtenRetardo(empleados[i], posicion);
                obtenHorasTrabajadas(empleados[i], posicion);
                obtenTurnoExtra(empleados[i], posicion);
                obtenSalida(empleados[i], posicion);
                empleados[i].Salida = null;
                empleados[i].Entrada = null;

            }
        }

        public void trabajaronDescanso(List<Empleado> empleados)
        {
            for (int i = 0; i < empleados.Count; i++)
            {
                int descanso = empleados[i].descanso;
                descanso = descanso - 1;
                if(descanso != -1 && descanso !=0 )
                {
                    if (empleados[i].matriz[descanso * 4] == 1 || empleados[i].matriz[descanso * 4 + 1] == 1 || empleados[i].matriz[descanso * 4 + 2] == 1 || empleados[i].matriz[descanso * 4 + 3] == 1)
                    {
                        empleados[i].trabajoDescanso = "SI";
                    }
                    else
                    {
                        empleados[i].trabajoDescanso = "NO";
                    }
                }

            }
        }

        public void obtenRetardo(Empleado empleado, int posicion)
        {
            DateTime entrada = new DateTime();
            DateTime horario = new DateTime();
            if (empleado.Entrada != null)
            {
                entrada = DateTime.Parse(empleado.Entrada);
                if (empleado.Horario == "00:00:00")
                { 
                    
                    empleado.anotacionesGenerales = (empleado.Nombre + ". Num: " + empleado.Numero + ". No tiene horario, se le asignó un horario de 6:00 am");
                    empleado.Horario = "6:00:00";
                }
                horario = DateTime.Parse(empleado.Horario);

                TimeSpan diferencia = entrada.TimeOfDay - horario.TimeOfDay;
                if (diferencia.Minutes > 5)
                {
                    //empleado.anotaciones[posicion] = "el empleado: " + empleado.Nombre + ". Llegó tarde con: " + diferencia + " horas. el día 1";
                    empleado.matriz[posicion * 4 + 1] = 1;

                }
            }

        }

        public void obtenAsistencia(Empleado empleado, int posicion)
        {
            if (empleado.Entrada == null)
            {
                empleado.matriz[posicion * 4 ] = 0;
            }
            else
            {
                empleado.matriz[posicion * 4 ] = 1;
            }
        }

        public void obtenHorasTrabajadas(Empleado empleado, int posicion)
        {


            if (empleado.Entrada == null || empleado.Salida == null)
            {
                if (empleado.Entrada == null && empleado.Salida == null)
                {
                    if(empleado.descanso-1 != posicion)
                        empleado.anotaciones[posicion] = "EMPLEADO NO ASISTIÓ";
                }
                else
                {
                    if(posicion != 6)
                        empleado.anotaciones[posicion] = "EMPLEADO NO CHECÓ ENTRADA O SALIDA";
                }

            }

            else
            {
                DateTime entrada = DateTime.Parse(empleado.Entrada);
                DateTime salida = DateTime.Parse(empleado.Salida);
                TimeSpan diferencia = salida.TimeOfDay - entrada.TimeOfDay;
                if (diferencia.TotalHours < 8)
                    empleado.anotaciones[posicion] = "EMPLEADO NO COMPLETÓ TURNO: " + diferencia.ToString() + ". HORAS.";
                empleado.horas = empleado.horas + diferencia.TotalHours;
                //empleado.matriz[posicion * 5 + 2] = diferencia.Hours;
            }
        }

        public void obtenTurnoExtra(Empleado empleado , int posicion)
        {
            if (empleado.Entrada == null || empleado.Salida == null)
            {
                empleado.matriz[posicion * 4 + 3] = 0;
            }
            else
            {
                DateTime entrada = DateTime.Parse(empleado.Entrada);
                DateTime salida = DateTime.Parse(empleado.Salida);
                TimeSpan diferencia = salida.TimeOfDay - entrada.TimeOfDay;

                if (diferencia.TotalHours > 13.2)
                {
                    empleado.matriz[posicion * 4 + 3] = 1;
                    //MessageBox.Show("El usuario: " + empleado.Nombre + ". Dobló turno, con un total de " + diferencia + "horas.");
                }

            }

        }

        public void obtenSalida(Empleado empleado , int posicion)
        {
            if(empleado.Salida == null)
            {
                if (empleado.matriz[posicion * 4 ] == 1)
                {
                    empleado.matriz[posicion * 4 + 2] = 1;
                    empleado.matriz[posicion * 4] = 0;
                }
            }
            else
            {
                if(empleado.matriz[posicion * 4] == 0)
                {
                    empleado.matriz[posicion * 4 + 1] = 1;
                    empleado.matriz[posicion * 4] = 0;
                    empleado.matriz[posicion * 4 + 2] = 0;  
                }
                empleado.matriz[posicion * 4 + 2] = 0;
            }
        }

        public void generaExcel(List<Empleado> empleados)
        {
            //
            var app = new Microsoft.Office.Interop.Excel.Application();
            var workbooks = app.Workbooks;
            var workbook = workbooks.Add("");
            var excelWS = workbook.ActiveSheet;

            //cabecera

            excelWS.Cells[1, 3] = "NOMBRE";
            Excel.Range cell = excelWS.Cells[1, 3];
            cell.ColumnWidth = 40;

            Excel.Range header = excelWS.Range["D1:G1"];
            header.Merge();
            header.Interior.Color = Excel.XlRgbColor.rgbLightSkyBlue;
            header.Value = "LUNES";
            header.ColumnWidth = 4;

            header = excelWS.Range["H1:K1"];
            header.Merge();
            header.Interior.Color = Excel.XlRgbColor.rgbDarkSeaGreen;
            header.Value = "MARTES";
            header.ColumnWidth = 4;

            header = excelWS.Range["L1:O1"];
            header.Merge();
            header.Interior.Color = Excel.XlRgbColor.rgbPeachPuff;
            header.Value = "MIERCOLES";
            header.ColumnWidth = 4;

            header = excelWS.Range["P1:S1"];
            header.Merge();
            header.Interior.Color = Excel.XlRgbColor.rgbDarkSalmon;
            header.Value = "JUEVES";
            header.ColumnWidth = 4;

            header = excelWS.Range["T1:W1"];
            header.Merge();
            header.Interior.Color = Excel.XlRgbColor.rgbTan;
            header.Value = "VIERNES";
            header.ColumnWidth = 4;

            header = excelWS.Range["X1:AA1"];
            header.Merge();
            header.Interior.Color = Excel.XlRgbColor.rgbSilver;
            header.Value = "SABADO";
            header.ColumnWidth = 4;

            header = excelWS.Range["AB1:AE1"];
            header.Merge();
            header.Interior.Color = Excel.XlRgbColor.rgbThistle;
            header.Value = "DOMINGO";
            header.ColumnWidth = 4;

            excelWS.Cells[1, 33] = "HORAS POR SEMANA";
            cell = excelWS.Cells[1, 33];
            cell.ColumnWidth = 20;
            cell.Interior.Color = Excel.XlRgbColor.rgbLightCoral;

            excelWS.Cells[1, 34] = "TRABAJO DESCANSO";
            cell = excelWS.Cells[1, 34];
            cell.ColumnWidth = 20;
            cell.Interior.Color = Excel.XlRgbColor.rgbLightGreen;

            excelWS.Cells[1, 32] = "DIA DESCANSO";
            cell = excelWS.Cells[1, 32];
            cell.ColumnWidth = 15;
            cell.Interior.Color = Excel.XlRgbColor.rgbPaleGoldenrod;

            header = excelWS.Range["AK1:AQ1"];
            header.Merge();
            header.Interior.Color = Excel.XlRgbColor.rgbCornflowerBlue;
            header.Value = "ANOTACIONES POR DIA";
            header.ColumnWidth = 49;

            //FORMATO ANOTACIONES
            excelWS.Cells[2, 37] = "LUNES";
            excelWS.Cells[2, 38] = "MARTES";
            excelWS.Cells[2, 39] = "MIERCOLES";
            excelWS.Cells[2, 40] = "JUEVES";
            excelWS.Cells[2, 41] = "VIERNES";
            excelWS.Cells[2,42] = "SABADO";
            excelWS.Cells[2, 43] = "DOMINGO";

            //SUBCABECERA
            for (int i = 4; i < 32; i = i + 4)
            {
                excelWS.Cells[2, i] = "DIA";
                excelWS.Cells[2, i + 1] = "RET";
                excelWS.Cells[2, i + 2] = "SAL";
                excelWS.Cells[2, i + 3] = "T/E";
                
            }

            excelWS.Cells[1, 35] = "ANOTACIONES GENERALES";
            cell = excelWS.Cells[1, 35];
            cell.ColumnWidth = 85;
            cell.Interior.Color = Excel.XlRgbColor.rgbLightBlue;



            for(int i = 0; i < empleados.Count; i++) {
                excelWS.Cells[i + 3, 3] = empleados[i].Nombre;
                excelWS.Cells[i + 3, 32] = transformaDia(empleados[i].descanso);
                excelWS.Cells[i + 3, 35] = empleados[i].anotacionesGenerales;
                excelWS.Cells[i + 3, 34] = empleados[i].trabajoDescanso;
                if (empleados[i].trabajoDescanso == "SI")
                {
                    cell = excelWS.Cells[i+3, 34];
                    cell.Interior.Color = Excel.XlRgbColor.rgbTomato;
                }
                //empleados
                for (int j = 0; j < 28; j++)
                {
                    // 
                    excelWS.Cells[i + 3, j+4] = empleados[i].matriz[j];
                    if(j == 26)
                    {
                        if (empleados[i].matriz[j] == 1)
                        {
                            excelWS.Cells[i + 3, j + 4] = 0;
                            excelWS.Cells[i + 3, j + 4 - 2] = 1;
                        }
                    }
                }

                for(int j = 37  ; j < 44 ; j++) {
                    excelWS.Cells[i + 3, j] = empleados[i].anotaciones[j - 37];
                }
                excelWS.Cells[i + 3, 33] = empleados[i].horas;

                //asigna dia de descanso.
                if (empleados[i].descanso != 0 && empleados[i].descanso != -1)
                    excelWS.Cells[i + 3, ((empleados[i].descanso - 1) * 4) + 4] = "D";

            }

            //NO REGISTRADOS
            excelWS.Cells[empleados.Count + 4, 2] = "EMPLEADO AÚN NO REGISTRADOS";
            cell = excelWS.Cells[empleados.Count + 4, 2];
            cell.ColumnWidth = 40;
            cell.Interior.Color = Excel.XlRgbColor.rgbLightCoral;
            cell.Borders.Value = true;

            excelWS.Cells[empleados.Count + 4, 3] = "NÚMERO";
            cell = excelWS.Cells[empleados.Count + 4, 3];
            cell.ColumnWidth = 30;
            cell.Interior.Color = Excel.XlRgbColor.rgbLightCoral;
            cell.Borders.Value = true;


            for (int i = 0; i < noRegistrados.Count; i++)
            {
                excelWS.Cells[i + empleados.Count + 5, 2] = noRegistrados[i].Nombre;
                cell = excelWS.Cells[i + empleados.Count + 5, 2];
                cell.ColumnWidth = 40;
                cell.Borders.Value = true;
                excelWS.Cells[i + empleados.Count + 5, 3] = noRegistrados[i].Numero;
                cell = excelWS.Cells[i + empleados.Count + 5, 3];
                cell.ColumnWidth = 40;
                cell.Borders.Value = true;

            }

            cell = excelWS.Cells;
            cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            try
            {
                workbook.SaveAs(@"C:\Users\misa_\OneDrive\Escritorio\Procesador nominas\nominas\Matriz.xlsx");
            }
            catch
            {
                MessageBox.Show("No se logró guardar, revisa que el archivo Matriz no se encuentre abierto e intenta de nuevo.");
            }
            Marshal.ReleaseComObject(excelWS);
            Marshal.ReleaseComObject(cell);
            Marshal.ReleaseComObject(header);
            header = null;
            cell = null;
            excelWS = null;
            workbook.Close();
            app.Quit();


            Marshal.ReleaseComObject(workbooks);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(app);

            GC.Collect();
            GC.WaitForPendingFinalizers();
 
            
        }

        public string transformaDia(int dia)
        {
            if(dia == 1)
                return "Lunes";
            if (dia == 2)
                return "Martes";
            if (dia == 3)
                return "Miercoles";
            if (dia == 4)
                return "Jueves";
            if (dia == 5)
                return "Viernes";
            if (dia == 6)
                return "Sabado";
            if (dia == 7)
                return "Domingo";
            return "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnUbicacion_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                ruta = folderBrowserDialog.SelectedPath;
                txtRuta.Text = ruta;
            }
        }

        private async void btnProcesar_Click(object sender, EventArgs e)
        {
            if (ruta == null)
            {
                MessageBox.Show("Asigna ruta primero");
                return;
            }
            pbxLoading.Visible = true;
            pbxSuccess.Visible = false;
            Task task1 = new Task(() => cargaEmpleados());
            task1.Start();
            await task1;
            if (exito)
            {
                Task task2 = new Task(() => cargaArchivos());
                task2.Start();
                await task2;
                Task task3 = new Task(() => generaExcel(empleados));
                task3.Start();
                await task3;
                pbxLoading.Visible = false;
                reseteaTodo();
                pbxSuccess.Visible = true;
            }
            else
            {
                reseteaTodo();
                return;
            }
        }

        public void reseteaTodo()
        {
            pbxLoading.Visible = false;
            pbxSuccess.Visible = false;
            empleados.Clear();
            noRegistrados.Clear();
            dia = 1;
        }

    }



}
