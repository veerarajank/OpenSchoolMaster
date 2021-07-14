using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PdfGenerator
/// </summary>
public class PdfGenerator
{
	public PdfGenerator()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static string HtmlToPdf(string pdfOutputLocation, string outputFilenamePrefix, string[] inputhtml,
            string[] options = null)
    {

        
        string pdfHtmlToPdfExePath = HttpContext.Current.Server.MapPath("~/Bin") + "\\wkhtmltopdf.exe";
        string urlsSeparatedBySpaces = string.Empty;
        try
        {
            //Determine inputs
            if ((inputhtml == null) || (inputhtml.Length == 0))
                throw new Exception("No input File provided for HtmlToPdf");
            else
                urlsSeparatedBySpaces = String.Join(" ", inputhtml); //Concatenate URLs

            string outputFolder = pdfOutputLocation;
            urlsSeparatedBySpaces = " -T 5mm --header-html " + HttpContext.Current.Server.MapPath("~/Bin") + "\\Header.html "
                + " --footer-html " + HttpContext.Current.Server.MapPath("~/Bin") + "\\Footer.html --margin-bottom 20 "
                +  urlsSeparatedBySpaces;

            string outputFilename = outputFilenamePrefix + "_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-fff") + ".PDF"; // assemble destination PDF file name

            outputFilename = outputFilenamePrefix + ".pdf"; // assemble destination PDF file name

            /*
            ProcessStartInfo startInfo = new ProcessStartInfo(pdfHtmlToPdfExePath);
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.WorkingDirectory = outputFolder;
            startInfo.Arguments = urlsSeparatedBySpaces;

            Process.Start(startInfo);
             */

            string arg=" " + urlsSeparatedBySpaces + " " + outputFilename;



            var p = new System.Diagnostics.Process()
            {
                StartInfo =
                {
                    FileName = pdfHtmlToPdfExePath,
                    Arguments = arg,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false, // needs to be false in order to redirect output
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true, // redirect all 3, as it should be all 3 or none
                    WorkingDirectory = outputFolder
                }
            };

            p.Start();

            // read the output here...
            //var output = p.StandardOutput.ReadToEnd();
            var errorOutput = p.StandardError.ReadToEnd();

            // ...then wait n milliseconds for exit (as after exit, it can't read the output)
            p.WaitForExit(60000);

            // read the exit code, close process
            int returnCode = p.ExitCode;
            p.Close();

            // if 0 or 2, it worked so return path of pdf
            if ((returnCode == 0) || (returnCode == 2))
                return outputFolder + outputFilename;
            else
                throw new Exception(errorOutput);
        }
        catch (Exception exc)
        {
            throw new Exception("Problem generating PDF from HTML, URLs: " + urlsSeparatedBySpaces + ", outputFilename: " + outputFilenamePrefix, exc);
        }
    }
    public static string Invoicepdf(string pdfOutputLocation, string outputFilenamePrefix, string[] inputhtml,
            string[] options = null)
    {


        string pdfHtmlToPdfExePath = HttpContext.Current.Server.MapPath("~/Bin") + "\\wkhtmltopdf.exe";
        string urlsSeparatedBySpaces = string.Empty;
        try
        {
            //Determine inputs
            if ((inputhtml == null) || (inputhtml.Length == 0))
                throw new Exception("No input File provided for HtmlToPdf");
            else
                urlsSeparatedBySpaces = String.Join(" ", inputhtml); //Concatenate URLs

            string outputFolder = pdfOutputLocation;
            urlsSeparatedBySpaces = " -T 5mm --header-html " + HttpContext.Current.Server.MapPath("~/Format") + "\\Header.html "
                + " --footer-html " + HttpContext.Current.Server.MapPath("~/Format") + "\\Footer.html --margin-bottom 20 "
                + urlsSeparatedBySpaces;

            string outputFilename = outputFilenamePrefix + "_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-fff") + ".PDF"; // assemble destination PDF file name

            outputFilename = outputFilenamePrefix + ".pdf"; // assemble destination PDF file name

            /*
            ProcessStartInfo startInfo = new ProcessStartInfo(pdfHtmlToPdfExePath);
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.WorkingDirectory = outputFolder;
            startInfo.Arguments = urlsSeparatedBySpaces;

            Process.Start(startInfo);
             */

            string arg = " " + urlsSeparatedBySpaces + " " + outputFilename;



            var p = new System.Diagnostics.Process()
            {
                StartInfo =
                {
                    FileName = pdfHtmlToPdfExePath,
                    Arguments = arg,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false, // needs to be false in order to redirect output
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true, // redirect all 3, as it should be all 3 or none
                    WorkingDirectory = outputFolder
                }
            };

            p.Start();

            // read the output here...
            //var output = p.StandardOutput.ReadToEnd();
            var errorOutput = p.StandardError.ReadToEnd();

            // ...then wait n milliseconds for exit (as after exit, it can't read the output)
            p.WaitForExit(60000);

            // read the exit code, close process
            int returnCode = p.ExitCode;
            p.Close();

            // if 0 or 2, it worked so return path of pdf
            if ((returnCode == 0) || (returnCode == 2))
                return outputFolder + outputFilename;
            else
                throw new Exception(errorOutput);
        }
        catch (Exception exc)
        {
            throw new Exception("Problem generating PDF from HTML, URLs: " + urlsSeparatedBySpaces + ", outputFilename: " + outputFilenamePrefix, exc);
        }
    }

    public static string Company_HtmlToPdf(string pdfOutputLocation, string outputFilenamePrefix, string headerfilename, string[] inputhtml,
           string[] options = null)
    {


        string pdfHtmlToPdfExePath = HttpContext.Current.Server.MapPath("~/Bin") + "\\wkhtmltopdf.exe";
        string urlsSeparatedBySpaces = string.Empty;
        try
        {
            //Determine inputs
            if ((inputhtml == null) || (inputhtml.Length == 0))
                throw new Exception("No input File provided for HtmlToPdf");
            else
                urlsSeparatedBySpaces = String.Join(" ", inputhtml); //Concatenate URLs

            string outputFolder = pdfOutputLocation;
            urlsSeparatedBySpaces = " -T 5mm --header-html " + HttpContext.Current.Server.MapPath("~/Bin") + "\\" + headerfilename + " " 
                + " --footer-html " + HttpContext.Current.Server.MapPath("~/Bin") + "\\Footer.html --margin-bottom 20 "
                + urlsSeparatedBySpaces;

            string outputFilename = outputFilenamePrefix + "_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-fff") + ".PDF"; // assemble destination PDF file name

            outputFilename = outputFilenamePrefix + ".pdf"; // assemble destination PDF file name

            /*
            ProcessStartInfo startInfo = new ProcessStartInfo(pdfHtmlToPdfExePath);
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.WorkingDirectory = outputFolder;
            startInfo.Arguments = urlsSeparatedBySpaces;

            Process.Start(startInfo);
             */

            string arg = " " + urlsSeparatedBySpaces + " " + outputFilename;



            var p = new System.Diagnostics.Process()
            {
                StartInfo =
                {
                    FileName = pdfHtmlToPdfExePath,
                    Arguments = arg,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false, // needs to be false in order to redirect output
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true, // redirect all 3, as it should be all 3 or none
                    WorkingDirectory = outputFolder
                }
            };

            p.Start();

            // read the output here...
            //var output = p.StandardOutput.ReadToEnd();
            var errorOutput = p.StandardError.ReadToEnd();

            // ...then wait n milliseconds for exit (as after exit, it can't read the output)
            p.WaitForExit(60000);

            // read the exit code, close process
            int returnCode = p.ExitCode;
            p.Close();

            // if 0 or 2, it worked so return path of pdf
            if ((returnCode == 0) || (returnCode == 2))
                return outputFolder + outputFilename;
            else
                throw new Exception(errorOutput);
        }
        catch (Exception exc)
        {
            throw new Exception("Problem generating PDF from HTML, URLs: " + urlsSeparatedBySpaces + ", outputFilename: " + outputFilenamePrefix, exc);
        }
    }

    
}