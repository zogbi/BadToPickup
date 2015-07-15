using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BadToPickup
{
    class Program
    {
        static void Main(string[] args)
        {
            string BadMailFolder = "C:\\Email\\badmail-Full";
            string PickupFolder = "C:\\Email\\pickup";
            string ReportFolder = "C:\\Email";

            Directory.SetCurrentDirectory(BadMailFolder);


            // Get an array of file names as strings rather than FileInfo objects. 
            // Use this method when storage space is an issue, and when you might 
            // hold on to the file name reference for a while before you try to access 
            // the file. 


            StringBuilder sbReport = new StringBuilder();
            int intRepeter = 0;
            string[] files = Directory.GetFiles(BadMailFolder, "*.BAD");
            foreach (string s in files)
            {
                // Create the FileInfo object only when needed to ensure 
                // the information is as current as possible.
                FileInfo fi = null;
                try
                {
                    fi = new FileInfo(s);
                }
                catch (FileNotFoundException e)
                {
                    // To inform the user and continue is 
                    // sufficient for this demonstration. 
                    // Your application may require different behavior.
                    Console.WriteLine(e.Message);
                    continue;
                }
                string strFile = BadMailFolder + "\\" + fi.Name;
                string strNewFile = Path.ChangeExtension(PickupFolder + "\\" + fi.Name, ".eml");
                intRepeter++;
                sbReport.AppendLine(" ");
                sbReport.AppendLine(" "); 
                sbReport.AppendLine("-----------------------------------------------------------------------------------------------------------------");
                sbReport.AppendLine("ID: " + intRepeter);
                sbReport.AppendLine("Origem: " + strFile);
                sbReport.AppendLine("Destino:" + strNewFile);

                bool blArquivo = false;
                bool blTo = false;
                
                StringBuilder sbContentFixed = new StringBuilder();

                foreach (var objline in File.ReadAllLines(strFile))
                {
                    
                    if (objline.Contains("From:") && blArquivo != true && !objline.Contains("postmaster"))
                    {
                        //Console.WriteLine(line);
                        sbReport.AppendLine(objline); 
                        blArquivo = true;
                        sbContentFixed.AppendLine(objline);

                    }
                    else if (blArquivo == true) {
                        sbContentFixed.AppendLine(objline);

                        if (objline.Contains("To:"))
                        {
                            blTo = true;
                            sbReport.AppendLine(objline);

                            
                        }
                        if (objline.Contains("Subject:"))
                        {
                            sbReport.AppendLine(objline);
                        }

                        if (objline.Contains("Date:"))
                        {
                            sbReport.AppendLine(objline);
                        }

                        
                    }

                    
                    
                }
                if (!File.Exists(strNewFile) && blArquivo == true && blTo == true)
                {
                    File.WriteAllText(strNewFile, sbContentFixed.ToString());
                }
                
                
                
            }
            File.WriteAllText(ReportFolder + "\\Report_Conversao.txt", sbReport.ToString());


            // Keep the console window open in debug mode.
           // Console.WriteLine("Press any key to exit.");
            //Console.ReadKey();

        }
    }
}
