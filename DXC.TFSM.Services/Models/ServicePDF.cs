using DXC.TFSM.Business;
using DXC.TFSM.Business.Model;
using IronPdf;
using System;
using System.Globalization;
using System.IO;
using iTextSharp.text.pdf;
using WkHtmlToPdfDotNet;
using iTextSharp.text;
using ceTe.DynamicPDF.HtmlConverter;
using ExpertPdf.HtmlToPdf;
using System.Threading;

namespace DXC.TFSM.Services.Models
{
    public class ServicePDF
    {
        private static byte[] generatePdf(string html)
        {
            byte[] res = new byte[] { };

            string tempHtml = UtilsService.RandomString(32) + ".html";
            string tempHtmlPath = System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/temp/" + tempHtml);
            while (File.Exists(tempHtmlPath))
            {
                tempHtml = UtilsService.RandomString(32) + ".html";
                tempHtmlPath = System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/temp/" + tempHtml);
            }
            File.WriteAllText(tempHtmlPath, html);

            res = Converter.Convert(new Uri(tempHtmlPath));

            if (File.Exists(tempHtmlPath))
            {
                File.Delete(tempHtmlPath);
            }


            return res;
        }

        private static byte[] generatePdf2(string html)
        {
            byte[] res = new byte[] { };

            string tempHtml = UtilsService.RandomString(32) + ".html";
            string tempHtmlPath = System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/temp/" + tempHtml);
            while (File.Exists(tempHtmlPath))
            {
                tempHtml = UtilsService.RandomString(32) + ".html";
                tempHtmlPath = System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/temp/" + tempHtml);
            }
            File.WriteAllText(tempHtmlPath, html);
            PdfConverter PDF = new PdfConverter();
            PDF = new PdfConverter { LicenseKey = "99zF18/XwcXXxsXZx9fExtnGxdnOzs7O" };
            PDF.PdfSecurityOptions.OwnerPassword = "TF5WexiC0#";
            PDF.PdfDocumentInfo.AuthorName = "Toyota Financial Services México, S.A. de C.V.";
            PDF.PdfDocumentOptions.GenerateSelectablePdf = true;
            PDF.PdfDocumentOptions.JpegCompressionEnabled = false;
            PDF.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
            PDF.PdfDocumentOptions.AutoSizePdfPage = true;
            PDF.PdfDocumentOptions.TopMargin = 20;
            PDF.PdfDocumentOptions.LeftMargin = 20;
            PDF.PdfDocumentOptions.BottomMargin = 20;
            PDF.PdfDocumentOptions.RightMargin = 20;
            PDF.PdfStandardSubset = PdfStandardSubset.Full;
            //PDF.NavigationTimeout = 600;
            PDF.RenderOnTimeout = true;

            //if (ProxyActive)
            //{
            //    PDF.AuthenticationOptions.Username = (!string.IsNullOrEmpty(ProxyDom) ? ProxyDom + "\\" + ProxyUser : ProxyUser);
            //    PDF.AuthenticationOptions.Password = ProxyPass;
            //}
            try
            {

                EscribeDatos(html);
                //res = PDF.GetPdfBytesFromHtmlFile(tempHtmlPath);
                res= PDF.GetPdfBytesFromHtmlString(html);
                //res = new byte[] {1,6,7};
                //res = Converter.Convert(new Uri(tempHtmlPath));

                if (File.Exists(tempHtmlPath))
                {
                    File.Delete(tempHtmlPath);
                }
            }catch(Exception e)
            {
                String msjError = "e.Message:" + e.Message + "\n e.Source: " + e.Source + "\n e.StackTrace:" + e.StackTrace;
                EscribeDatos(msjError);
            }


            return res;
        }
        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        public static bool EscribeDatos(string lines, bool esError = false)
        {
            
        // Set Status to Locked
            _readWriteLock.EnterWriteLock();
            try
            {
                if (!lines.Contains("Thread was being aborted"))
                {
                    //var variables = HttpContext.Current.Request.ServerVariables;
                    //var taskSchdlr = System.Threading.Tasks.TaskScheduler.Current;

                    //var filePath = variables["APPL_PHYSICAL_PATH"] + "\\logs\\log" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
                    //var path = System.IO.Directory.GetCurrentDirectory();
                    //var otro = HttpRuntime.AppDomainAppVirtualPath;

                    //var path = System.Web.Hosting.HostingEnvironment.MapPath("/");
                    var path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                    //var path = "D:\\inetpub\\wwwroot\\wcfBuroCredito";
                    var filePath = path + "\\logs\\log" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt";

                    if (filePath.Length == 0)
                        return false;
                    if (lines.Length == 0)
                        return false;

                    // Append text to the file
                    //using (StreamWriter sw = File.AppendText(filePath)) {
                    using (StreamWriter sw = File.Exists(filePath) ? File.AppendText(filePath) : File.CreateText(filePath))
                    {
                        if (esError == true)
                        {
                            sw.WriteLine("MENSAJE: " + lines);
                        }
                        else
                        {
                            sw.WriteLine("ERROR: " + lines);
                        }
                        sw.WriteLine("FECHA: " + DateTime.Now);
                        sw.WriteLine("-------------------------------------------------------------------------------------------------------------");
                        sw.Close();
                    }
                }
            }
            finally
            {
                // Release lock
                _readWriteLock.ExitWriteLock();
            }
            return true;
        }

        public static byte[] asposePdf(RequestPDF dataPDF)
        {
            string input = System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/Toyota_cotiza_tradicional.html");
            string inputCss = System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/assets/css/ToyotaCotTrad.css");

            ResultPrice data = new ResultPrice();
            switch (dataPDF.Plazo)
            {
                case "24":
                    data = dataPDF.DatosCotizar[0];
                    break;
                case "36":
                    data = dataPDF.DatosCotizar[1];
                    break;
                case "48":
                    data = dataPDF.DatosCotizar[2];
                    break;
            }

            byte[] res = new byte[] { };
            // Open the file to read from.

            string html = File.ReadAllText(input);
            string css = File.ReadAllText(inputCss);

            html = html.Replace("{image_logo}", dataPDF.ImagenModelo);
            html = html.Replace("{image_auto}", dataPDF.ImagenAuto);
            html = html.Replace("{precio_total}", FormatNumber(data.PrecioTotal));
            html = html.Replace("{vehiculo}", data.Marca.ToUpper());
            html = html.Replace("{modelo}", data.Modelo);
            html = html.Replace("{version}", data.Vesion.ToUpper());
            html = html.Replace("{fecha_cotizacion}", DateTime.Now.ToString("dd/MM/yyyy"));
            html = html.Replace("{fecha_impresion}", DateTime.Now.ToString("dd/MM/yyyy"));
            html = html.Replace("{nombre_cliente}", data.Nombre.ToUpper() + ' ' + data.Apellido.ToUpper());
            html = html.Replace("{plazo}", dataPDF.Plazo);
            html = html.Replace("{mensualidad}", FormatNumber(data.Mensualidad));
            html = html.Replace("{enganche}", FormatNumber(data.Enganche));
            //html = html.Replace("{tasa_interes}", GetPorcentaje(tasa));
            html = html.Replace("{comision_apertura}", GetPorcentaje(data.PorcentajeComision));
            html = html.Replace("{estado}", data.Estado.ToUpper());
            html = html.Replace("{aseguradora}", data.Aseguradora.ToUpper());
            html = html.Replace("{cobertura}", data.Cobertura.ToUpper());
            html = html.Replace("{plazo24}", FormatNumber(dataPDF.DatosCotizar[0].Mensualidad));
            //html = html.Replace("{plazo36}", FormatNumber(dataPDF.DatosCotizar[1].Mensualidad));
            //html = html.Replace("{plazo48}", FormatNumber(dataPDF.DatosCotizar[2].Mensualidad));

            string tempFile = UtilsService.RandomString(32) + ".html";
            string tempPath = System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/temp/" + tempFile);
            while (File.Exists(tempPath))
            {
                tempFile = UtilsService.RandomString(32) + ".html";
                tempPath = System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/temp/" + tempFile);
            }
            File.WriteAllText(tempPath, html);

            string outputFile = UtilsService.RandomString(32) + ".pdf";
            string outputPath = System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/temp/" + outputFile);
            while (File.Exists(outputPath))
            {
                outputFile = UtilsService.RandomString(32) + ".pdf";
                outputPath = System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/temp/" + outputFile);
            }

            res = Converter.Convert(new Uri(tempPath));

            if (File.Exists(outputPath))
            {
                //res = File.ReadAllBytes(outputPath);
                File.Delete(outputPath);
            }
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }

            return res;
        }

        public static byte[] getPlanTradicionalPdf(RequestPDF dataPDF)
        {
            BssPrice bssPrice = new BssPrice();

            ResultPrice data = new ResultPrice();
            switch (dataPDF.Plazo)
            {
                case "24":
                    data = dataPDF.DatosCotizar[0];
                    break;
                case "36":
                    data = dataPDF.DatosCotizar[1];
                    break;
                case "48":
                    data = dataPDF.DatosCotizar[2];
                    break;
            }

            try
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/Toyota_cotiza_tradicional.html");

                if (File.Exists(path))
                {
                    byte[] res = new byte[] { };
                    // Open the file to read from.
                    string html = File.ReadAllText(path);
                    double tasa = bssPrice.GetParametro("TasaFijaSinIVA");

                    html = html.Replace("{image_auto}", dataPDF.ImagenAuto);
                    html = html.Replace("{precio_auto}", FormatNumber(dataPDF.PrecioAuto));
                    html = html.Replace("{precio_total}", FormatNumber(data.PrecioTotal));
                    html = html.Replace("{vehiculo}", data.Marca.ToUpper());
                    html = html.Replace("{modelo}", data.Vesion.ToUpper());
                    html = html.Replace("{version}", data.Modelo);
                    html = html.Replace("{fecha_cotizacion}", String.IsNullOrEmpty(dataPDF.FechaCotizacion) == true ? DateTime.Now.ToString("dd/MM/yyyy") : dataPDF.FechaCotizacion);
                    html = html.Replace("{fecha_impresion}", DateTime.Now.ToString("dd/MM/yyyy"));
                    html = html.Replace("{nombre_cliente}", data.Nombre.ToUpper() + ' ' + data.Apellido.ToUpper());
                    html = html.Replace("{plazo}", dataPDF.Plazo);
                    html = html.Replace("{mensualidad}", FormatNumber(data.Mensualidad));
                    html = html.Replace("{enganche}", FormatNumber(data.Enganche));
                    html = html.Replace("{tasa_interes}", GetPorcentaje(tasa));
                    html = html.Replace("{comision_apertura}", GetPorcentaje(data.PorcentajeComision));
                    html = html.Replace("{estado}", data.Estado.ToUpper());
                    html = html.Replace("{aseguradora}", data.Aseguradora.ToUpper());
                    html = html.Replace("{cobertura}", data.Cobertura.ToUpper());
                    html = html.Replace("{plazo24}", FormatNumber(dataPDF.DatosCotizar[0].Mensualidad));
                    html = html.Replace("{plazo36}", FormatNumber(dataPDF.DatosCotizar[1].Mensualidad));
                    html = html.Replace("{plazo48}", FormatNumber(dataPDF.DatosCotizar[2].Mensualidad));

                    if (!string.IsNullOrEmpty(dataPDF.ImagenModelo))
                    {
                        html = html.Replace("{image_logo}", dataPDF.ImagenModelo);
                    }
                    else
                    {
                        html = html.Replace("{image_logo}", System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/assets/images/transparent.png"));
                    }
                    if ((data.Nombre + data.Apellido).Length < 31)
                    {
                        html = html.Replace("{nombre_completo}", data.Nombre.ToUpper() + ' ' + data.Apellido.ToUpper());
                    }
                    else
                    {
                        html = html.Replace("{nombre_completo}", data.Nombre.ToUpper() + "<br/>" + data.Apellido.ToUpper());
                    }

                    res = generatePdf(html);

                    return res;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static byte[] getPlanTradicionalPdf2(RequestPDF dataPDF)
        {
            BssPrice bssPrice = new BssPrice();

            ResultPrice data = new ResultPrice();
            switch (dataPDF.Plazo)
            {
                case "24":
                    data = dataPDF.DatosCotizar[0];
                    break;
                case "36":
                    data = dataPDF.DatosCotizar[1];
                    break;
                case "48":
                    data = dataPDF.DatosCotizar[2];
                    break;
            }
            /*
            try
            {
            */
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/Toyota_cotiza_tradicional.html");

                if (File.Exists(path))
                {
                    byte[] res = new byte[] { };
                    // Open the file to read from.
                    string html = File.ReadAllText(path);
                    double tasa = 15.99;

                    html = html.Replace("{image_auto}", dataPDF.ImagenAuto);
                    html = html.Replace("{precio_auto}", FormatNumber(dataPDF.PrecioAuto));
                    html = html.Replace("{precio_total}", FormatNumber(data.PrecioTotal));
                    html = html.Replace("{vehiculo}", data.Marca.ToUpper());
                    html = html.Replace("{modelo}", data.Vesion.ToUpper());
                    html = html.Replace("{version}", data.Modelo);
                    html = html.Replace("{fecha_cotizacion}", String.IsNullOrEmpty(dataPDF.FechaCotizacion) == true ? DateTime.Now.ToString("dd/MM/yyyy") : dataPDF.FechaCotizacion);
                    html = html.Replace("{fecha_impresion}", DateTime.Now.ToString("dd/MM/yyyy"));
                    html = html.Replace("{nombre_cliente}", data.Nombre.ToUpper() + ' ' + data.Apellido.ToUpper());
                    html = html.Replace("{plazo}", dataPDF.Plazo);
                    html = html.Replace("{mensualidad}", FormatNumber(data.Mensualidad));
                    html = html.Replace("{enganche}", FormatNumber(data.Enganche));
                    html = html.Replace("{tasa_interes}", GetPorcentaje(tasa));
                    html = html.Replace("{comision_apertura}", GetPorcentaje(data.PorcentajeComision));
                    html = html.Replace("{estado}", data.Estado.ToUpper());
                    html = html.Replace("{aseguradora}", data.Aseguradora.ToUpper());
                    html = html.Replace("{cobertura}", data.Cobertura.ToUpper());
                    html = html.Replace("{plazo24}", FormatNumber(dataPDF.DatosCotizar[0].Mensualidad));
                    html = html.Replace("{plazo36}", FormatNumber(dataPDF.DatosCotizar[1].Mensualidad));
                    html = html.Replace("{plazo48}", FormatNumber(dataPDF.DatosCotizar[2].Mensualidad));

                    if (!string.IsNullOrEmpty(dataPDF.ImagenModelo))
                    {
                        html = html.Replace("{image_logo}", dataPDF.ImagenModelo);
                    }
                    else
                    {
                        html = html.Replace("{image_logo}", System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/assets/images/transparent.png"));
                    }
                    if ((data.Nombre + data.Apellido).Length < 31)
                    {
                        html = html.Replace("{nombre_completo}", data.Nombre.ToUpper() + ' ' + data.Apellido.ToUpper());
                    }
                    else
                    {
                        html = html.Replace("{nombre_completo}", data.Nombre.ToUpper() + "<br/>" + data.Apellido.ToUpper());
                    }
                String htmlFalso = "<!DOCTYPE html><html lang=\"es\"><body><div>HOLA MUNDO</div></body></html>";
                    res = generatePdf2(htmlFalso);

                    return res;
                }
                else
                {
                    return null;
                }

        /*
            }
            catch (Exception e)
            {
                throw e;
            }
        */
        }


        public static byte[] getPlanBalloonPdf(RequestPDF dataPDF)
        {
            BssPrice bssPrice = new BssPrice();

            ResultPrice data = new ResultPrice();
            switch (dataPDF.Plazo)
            {
                case "24":
                    data = dataPDF.DatosCotizar[0];
                    break;
                case "36":
                    data = dataPDF.DatosCotizar[1];
                    break;
                case "48":
                    data = dataPDF.DatosCotizar[2];
                    break;
            }

            try
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/Toyota_cotiza_balloon.html");

                if (File.Exists(path))
                {
                    byte[] res = new byte[] { };
                    // Open the file to read from.
                    string html = File.ReadAllText(path);
                    double tasa = bssPrice.GetParametro("TasaFijaSinIVA");

                    html = html.Replace("{image_auto}", dataPDF.ImagenAuto);
                    html = html.Replace("{precio_auto}", FormatNumber(dataPDF.PrecioAuto));
                    html = html.Replace("{precio_total}", FormatNumber(data.PrecioTotal));
                    html = html.Replace("{modelo}", data.Vesion.ToUpper());
                    html = html.Replace("{version}", data.Modelo);
                    html = html.Replace("{version}", data.Vesion.ToUpper());
                    html = html.Replace("{vehiculo}", data.Marca.ToUpper());
                    html = html.Replace("{fecha_cotizacion}", DateTime.Now.ToString("dd/MM/yyyy"));
                    html = html.Replace("{fecha_impresion}", DateTime.Now.ToString("dd/MM/yyyy"));
                    html = html.Replace("{nombre_cliente}", data.Nombre.ToUpper() + ' ' + data.Apellido.ToUpper());
                    html = html.Replace("{plazo}", dataPDF.Plazo);
                    html = html.Replace("{mensualidad}", FormatNumber(data.Mensualidad));
                    html = html.Replace("{enganche}", FormatNumber(data.Enganche));
                    html = html.Replace("{tasa_interes}", GetPorcentaje(tasa));
                    html = html.Replace("{comision_apertura}", GetPorcentaje(data.PorcentajeComision));
                    html = html.Replace("{monto_balloon}", FormatNumber(data.Ballon));
                    html = html.Replace("{porcentaje_balloon}", GetPorcentaje(data.PBallon));
                    html = html.Replace("{estado}", data.Estado.ToUpper());
                    html = html.Replace("{aseguradora}", data.Aseguradora.ToUpper());
                    html = html.Replace("{cobertura}", data.Cobertura.ToUpper());
                    html = html.Replace("{plazo24}", FormatNumber(dataPDF.DatosCotizar[0].Mensualidad));
                    html = html.Replace("{plazo36}", FormatNumber(dataPDF.DatosCotizar[1].Mensualidad));
                    html = html.Replace("{plazo48}", FormatNumber(dataPDF.DatosCotizar[2].Mensualidad));

                    if (!string.IsNullOrEmpty(dataPDF.ImagenModelo))
                    {
                        html = html.Replace("{image_logo}", dataPDF.ImagenModelo);
                    }
                    else
                    {
                        html = html.Replace("{image_logo}", System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/assets/images/transparent.png"));
                    }
                    if ((data.Nombre + data.Apellido).Length < 31)
                    {
                        html = html.Replace("{nombre_completo}", data.Nombre.ToUpper() + ' ' + data.Apellido.ToUpper());
                    }
                    else
                    {
                        html = html.Replace("{nombre_completo}", data.Nombre.ToUpper() + "<br/>" + data.Apellido.ToUpper());
                    }

                    res = generatePdf(html);

                    return res;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static byte[] getPlanAnualidadesPdf(RequestPDF dataPDF)
        {
            BssPrice bssPrice = new BssPrice();

            ResultPrice data = new ResultPrice();
            switch (dataPDF.Plazo)
            {
                case "24":
                    data = dataPDF.DatosCotizar[0];
                    break;
                case "36":
                    data = dataPDF.DatosCotizar[1];
                    break;
                case "48":
                    data = dataPDF.DatosCotizar[2];
                    break;
            }

            try
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/Toyota_cotiza_anualidades.html");

                if (File.Exists(path))
                {
                    byte[] res = new byte[] { };
                    // Open the file to read from.
                    string html = File.ReadAllText(path);
                    double tasa = bssPrice.GetParametro("TasaFijaSinIVA");

                    html = html.Replace("{image_auto}", dataPDF.ImagenAuto);
                    html = html.Replace("{precio_auto}", FormatNumber(dataPDF.PrecioAuto));
                    html = html.Replace("{precio_total}", FormatNumber(data.PrecioTotal));
                    html = html.Replace("{vehiculo}", data.Marca.ToUpper());
                    html = html.Replace("{modelo}", data.Vesion.ToUpper());
                    html = html.Replace("{version}", data.Modelo);
                    html = html.Replace("{fecha_cotizacion}", DateTime.Now.ToString("dd/MM/yyyy"));
                    html = html.Replace("{fecha_impresion}", DateTime.Now.ToString("dd/MM/yyyy"));
                    html = html.Replace("{nombre_cliente}", data.Nombre.ToUpper() + ' ' + data.Apellido.ToUpper());
                    html = html.Replace("{plazo}", dataPDF.Plazo);
                    html = html.Replace("{mensualidad}", FormatNumber(data.Mensualidad));
                    html = html.Replace("{enganche}", FormatNumber(data.Enganche));
                    html = html.Replace("{tasa_interes}", GetPorcentaje(tasa));
                    html = html.Replace("{comision_apertura}", GetPorcentaje(data.PorcentajeComision));
                    html = html.Replace("{monto_anualidad}", FormatNumber(data.Anualidad));
                    html = html.Replace("{estado}", data.Estado.ToUpper());
                    html = html.Replace("{aseguradora}", data.Aseguradora.ToUpper());
                    html = html.Replace("{cobertura}", data.Cobertura.ToUpper());
                    html = html.Replace("{plazo24}", FormatNumber(dataPDF.DatosCotizar[0].Mensualidad));
                    html = html.Replace("{plazo36}", FormatNumber(dataPDF.DatosCotizar[1].Mensualidad));
                    html = html.Replace("{plazo48}", FormatNumber(dataPDF.DatosCotizar[2].Mensualidad));

                    if (!string.IsNullOrEmpty(dataPDF.ImagenModelo))
                    {
                        html = html.Replace("{image_logo}", dataPDF.ImagenModelo);
                    }
                    else
                    {
                        html = html.Replace("{image_logo}", System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/assets/images/transparent.png"));
                    }
                    if ((data.Nombre + data.Apellido).Length < 31)
                    {
                        html = html.Replace("{nombre_completo}", data.Nombre.ToUpper() + ' ' + data.Apellido.ToUpper());
                    }
                    else
                    {
                        html = html.Replace("{nombre_completo}", data.Nombre.ToUpper() + "<br/>" + data.Apellido.ToUpper());
                    }

                    res = generatePdf(html);

                    return res;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static byte[] getFinancieroPdf(RequestPDF dataPDF)
        {
            ResultPrice data = new ResultPrice();
            switch (dataPDF.Plazo)
            {
                case "24":
                    data = dataPDF.DatosCotizar[0];
                    break;
                case "36":
                    data = dataPDF.DatosCotizar[1];
                    break;
                case "48":
                    data = dataPDF.DatosCotizar[2];
                    break;
            }

            try
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/Toyota_cotiza_arrendamiento.html");

                if (File.Exists(path))
                {
                    byte[] res = new byte[] { };
                    // Open the file to read from.
                    string html = File.ReadAllText(path);

                    html = html.Replace("{image_auto}", dataPDF.ImagenAuto);
                    html = html.Replace("{precio_auto}", FormatNumber(dataPDF.PrecioAuto));
                    html = html.Replace("{precio_total}", FormatNumber(data.PrecioTotal));
                    html = html.Replace("{vehiculo}", data.Marca.ToUpper());
                    html = html.Replace("{modelo}", data.Vesion.ToUpper());
                    html = html.Replace("{version}", data.Modelo);
                    html = html.Replace("{fecha_cotizacion}", DateTime.Now.ToString("dd/MM/yyyy"));
                    html = html.Replace("{fecha_impresion}", DateTime.Now.ToString("dd/MM/yyyy"));
                    html = html.Replace("{nombre_cliente}", data.Nombre.ToUpper() + ' ' + data.Apellido.ToUpper());
                    html = html.Replace("{plazo}", dataPDF.Plazo);
                    html = html.Replace("{mensualidad}", FormatNumber(data.Mensualidad));
                    html = html.Replace("{cant_depositos}", data.DepositoGarantia.ToString());
                    html = html.Replace("{monto_deposito}", FormatNumber(data.Enganche));
                    html = html.Replace("{monto_arrendar}", FormatNumber(data.PrecioTotal));
                    html = html.Replace("{comision_apertura}", GetPorcentaje(data.PorcentajeComision));
                    html = html.Replace("{monto_comision}", FormatNumber(data.Comision));
                    html = html.Replace("{estado}", data.Estado.ToUpper());
                    html = html.Replace("{aseguradora}", data.Aseguradora.ToUpper());
                    html = html.Replace("{cobertura}", data.Cobertura.ToUpper());
                    html = html.Replace("{plazo24}", FormatNumber(dataPDF.DatosCotizar[0].Mensualidad));
                    html = html.Replace("{plazo36}", FormatNumber(dataPDF.DatosCotizar[1].Mensualidad));
                    html = html.Replace("{plazo48}", FormatNumber(dataPDF.DatosCotizar[2].Mensualidad));

                    if (!string.IsNullOrEmpty(dataPDF.ImagenModelo))
                    {
                        html = html.Replace("{image_logo}", dataPDF.ImagenModelo);
                    }
                    else
                    {
                        html = html.Replace("{image_logo}", System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/assets/images/transparent.png"));
                    }
                    if ((data.Nombre + data.Apellido).Length < 31)
                    {
                        html = html.Replace("{nombre_completo}", data.Nombre.ToUpper() + ' ' + data.Apellido.ToUpper());
                    }
                    else
                    {
                        html = html.Replace("{nombre_completo}", data.Nombre.ToUpper() + "<br/>" + data.Apellido.ToUpper());
                    }

                    res = generatePdf(html);

                    return res;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static byte[] getPuroPdf(RequestPDF dataPDF)
        {
            ResultPrice data = new ResultPrice();
            switch (dataPDF.Plazo)
            {
                case "24":
                    data = dataPDF.DatosCotizar[0];
                    break;
                case "36":
                    data = dataPDF.DatosCotizar[1];
                    break;
                case "48":
                    data = dataPDF.DatosCotizar[2];
                    break;
            }

            try
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/Toyota_cotiza_arrendamiento_puro.html");

                if (File.Exists(path))
                {
                    byte[] res = new byte[] { };
                    // Open the file to read from.
                    string html = File.ReadAllText(path);

                    html = html.Replace("{image_auto}", dataPDF.ImagenAuto);
                    html = html.Replace("{precio_auto}", FormatNumber(dataPDF.PrecioAuto));
                    html = html.Replace("{precio_total}", FormatNumber(data.PrecioTotal));
                    html = html.Replace("{vehiculo}", data.Marca.ToUpper());
                    html = html.Replace("{modelo}", data.Vesion.ToUpper());
                    html = html.Replace("{version}", data.Modelo);
                    html = html.Replace("{fecha_cotizacion}", DateTime.Now.ToString("dd/MM/yyyy"));
                    html = html.Replace("{fecha_impresion}", DateTime.Now.ToString("dd/MM/yyyy"));
                    html = html.Replace("{nombre_cliente}", data.Nombre.ToUpper() + ' ' + data.Apellido.ToUpper());
                    html = html.Replace("{plazo}", dataPDF.Plazo);
                    html = html.Replace("{mensualidad}", FormatNumber(data.Mensualidad));
                    html = html.Replace("{cant_depositos}", data.DepositoGarantia.ToString());
                    html = html.Replace("{monto_deposito}", FormatNumber(data.Enganche));
                    html = html.Replace("{monto_arrendar}", FormatNumber(data.PrecioTotal));
                    html = html.Replace("{comision_apertura}", GetPorcentaje(data.PorcentajeComision));
                    html = html.Replace("{monto_comision}", FormatNumber(data.Comision));
                    html = html.Replace("{estado}", data.Estado.ToUpper());
                    html = html.Replace("{aseguradora}", data.Aseguradora.ToUpper());
                    html = html.Replace("{cobertura}", data.Cobertura.ToUpper());
                    html = html.Replace("{plazo24}", FormatNumber(dataPDF.DatosCotizar[0].Mensualidad));
                    html = html.Replace("{plazo36}", FormatNumber(dataPDF.DatosCotizar[1].Mensualidad));
                    html = html.Replace("{plazo48}", FormatNumber(dataPDF.DatosCotizar[2].Mensualidad));

                    if (!string.IsNullOrEmpty(dataPDF.ImagenModelo))
                    {
                        html = html.Replace("{image_logo}", dataPDF.ImagenModelo);
                    }
                    else
                    {
                        html = html.Replace("{image_logo}", System.Web.Hosting.HostingEnvironment.MapPath("~/HTML Templates/Planes/assets/images/transparent.png"));
                    }
                    if ((data.Nombre + data.Apellido).Length < 31)
                    {
                        html = html.Replace("{nombre_completo}", data.Nombre.ToUpper() + ' ' + data.Apellido.ToUpper());
                    }
                    else
                    {
                        html = html.Replace("{nombre_completo}", data.Nombre.ToUpper() + "<br/>" + data.Apellido.ToUpper());
                    }

                    res = generatePdf(html);

                    return res;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static string FormatNumberString(string number)
        {
            double num = double.Parse(number, CultureInfo.InvariantCulture);
            string res = string.Format("{0:#,0.00}", num);

            return res;
        }

        private static string FormatNumber(double number)
        {
            string res = string.Format("{0:#,0.00}", number);

            return res;
        }

        private static string GetPorcentajeString(string number)
        {
            double num = double.Parse(number, CultureInfo.InvariantCulture);
            num = num * 100;
            string res = string.Format("{0:#,0.##}", num);

            return res;
        }

        private static string GetPorcentaje(double number)
        {
            double num = number * 100;
            string res = string.Format("{0:#,0.##}", num);

            return res;
        }
    }
}