using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsumoWS
{
    class Program
    {
        static void Main(string[] args)
        {

            var resultado = LlamadoServicio("4e25ce61-e6e2-457a-89f7-116404990967", "2017-01-01", "2017-03-01");
            
        }

        public static string LlamadoServicio(string Id, string startDate, string finishDate)
        {
            string startDateIni = startDate;
            string finishDateIni = finishDate;
            int CantFacturas = 0;
            int Llamdos = 0;
            DateTime UltimaIni = DateTime.Parse(startDate);
            DateTime UltimaFin = DateTime.Parse(finishDate);
            bool IndUltima = false;

            
            string res = string.Empty;
            while (!res.ToString().Contains("100 resultados") && IndUltima == false)
            {

                if(UltimaFin == DateTime.Parse(finishDate))
                    IndUltima = true;

                res = ConsumirServicio(Id, UltimaIni.ToString("yyyy-MM-dd"), UltimaFin.ToString("yyyy-MM-dd"));
                Llamdos++;

                ///Evaluo el resultado
                if (res.ToString().Contains("100 resultados")) 
                {
                    IndUltima = false;
                    var DifMes = MonthDifference(UltimaFin, UltimaIni);
                    if (DifMes > 1)
                    {
                        UltimaFin = UltimaFin.AddMonths(-int.Parse(DifMes.ToString()) + 1).AddDays(-1);
                    }
                       
                    else
                    {
                        UltimaFin = UltimaFin.AddDays(-1);
                    }
                         
                  res = string.Empty;
                }
                else
                {
                    CantFacturas = CantFacturas + int.Parse(res);
                    UltimaIni = UltimaFin.AddDays(1);
                    var DifMes = MonthDifference(DateTime.Parse(finishDate), UltimaIni);
                    UltimaFin = UltimaFin.AddMonths(1);
                    res = string.Empty;
                    if (DifMes < 1)
                    {
                        
                        UltimaIni = UltimaIni.AddDays(1);
                        UltimaFin = DateTime.Parse(finishDate);

                    }
                 }

            }

            Console.WriteLine("Cantidad Facturas: " + CantFacturas);
            Console.WriteLine("Número de LLamados: " + Llamdos);
            Console.Read();
            return string.Empty;
        }

        //Cónsumo del Servicio 
        public static string ConsumirServicio(string Id, string startDate, string finishDate)
        {
            var uri = "http://34.209.24.195/facturas"; 
            var data = string.Format("id={0}&start={1}&finish={2}", Id, startDate, finishDate);
            uri = string.Format(uri + "?{0}", data);
            var jsonResult = HttpRestRequest.CallRestMethod(uri);
            return jsonResult;
        }
        public static decimal MonthDifference(DateTime FechaFin, DateTime FechaInicio)
        {
            return Math.Abs((FechaFin.Month - FechaInicio.Month) + 12 * (FechaFin.Year - FechaInicio.Year));

        }


    }
}
