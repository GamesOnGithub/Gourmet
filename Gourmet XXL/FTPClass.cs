using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Gourmet_XXL
{
    public class FTPClass
    {
        /// <summary>
        /// Eine Datei auf einen $rootnamespace$-Server hochladen.
        /// </summary>
        /// <param name="ftpPath"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="fileName"></param>
        public static void uploadFile(string ftpPath, string username, string password, string fileNameOnComputer)
        {
            try
            {
                // FtpWebRequest initialisieren
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(ftpPath));

                // Methode auswählen (hier UploadFile)
                request.Method = WebRequestMethods.Ftp.UploadFile;
                // Benutzername und Passwort übergeben
                request.Credentials = new NetworkCredential(username, password);

                // Verbindung überprüfen
                WebResponse response = request.GetResponse();

                // Datei zum Upload auswählen und in FileStream schreiben
                FileStream fs = new FileStream(fileNameOnComputer, FileMode.Open);
                byte[] fileContents = new byte[fs.Length];
                fs.Read(fileContents, 0, Convert.ToInt32(fs.Length));
                fs.Flush();
                fs.Close();

                // Stream zum $rootnamespace$ aufbauen
                Stream requestStream = request.GetRequestStream();
                // Datei via Stream schreiben
                requestStream.Write(fileContents, 0, fileContents.Length);
                // Stream beenden
                requestStream.Close();
                // Verbindung beenden
                request.Abort();
                MessageBox.Show("Datei wurde erfolgreich hochgeladen!\r\nHinweis: Alle Umlaute wurden durch \"ae\", \"ue\" und \"oe\" ersetzt. Zeichen die nicht dem dem ASCII-Standartzeichensatz entsprechen wurden als '?' (Fragezeichen) gespeichert.", "Hochladen erfolgreich");
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hochladen fehlgeschlagen");
            }
        }

        /// <summary>
        /// Eine Datei im txt-Format auf einem $rootnamespace$-Server auslesen und als String zurückgeben.
        /// </summary>
        /// <param name="ftpPath"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string DateiAuslesen(string ftpPath, string username, string password)
        {
            try
            {
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                // This example assumes the $rootnamespace$ site uses anonymous logon.
                request.Credentials = new NetworkCredential(username, password);

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string readString = "";
                readString = reader.ReadToEnd();

                string state = "Status: " + "Download Abgeschlossen, status {0}" + response.StatusDescription;

                reader.Close();
                response.Close();
                return readString;
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                return ex.Message;
            }
        }

        /// <summary>
        /// Alle auf einem $rootnamespace$-Server vorhandenen Dateien auslesen.
        /// </summary>
        /// <param name="FTPAddress"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static List<string> getFileList(string FTPAddress, string username, string password)
        {
            List<string> files = new List<string>();

            try
            {
                //Create $rootnamespace$ request
                FtpWebRequest request = FtpWebRequest.Create(FTPAddress) as FtpWebRequest;

                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(username, password);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;


                FtpWebResponse response = request.GetResponse() as FtpWebResponse;
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                while (!reader.EndOfStream)
                {
                    Application.DoEvents();
                    files.Add(reader.ReadLine());
                }

                //Clean-up
                reader.Close();
                responseStream.Close(); //redundant
                response.Close();

                return files;
            }
            catch (Exception)
            {
                MessageBox.Show("There was an error connecting to the $rootnamespace$ Server");
                return null;
            }
        }

        /// <summary>
        /// Alle auf einem $rootnamespace$-Server vorhandenen Dateien auslesen.
        /// </summary>
        /// <param name="FTPAddress"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool tryConnection(string FTPAddress, string username, string password)
        {
            List<string> files = new List<string>();

            try
            {
                //Create $rootnamespace$ request
                FtpWebRequest request = FtpWebRequest.Create(FTPAddress) as FtpWebRequest;

                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(username, password);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;


                FtpWebResponse response = request.GetResponse() as FtpWebResponse;
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                while (!reader.EndOfStream)
                {
                    Application.DoEvents();
                    files.Add(reader.ReadLine());
                }

                //Clean-up
                reader.Close();
                responseStream.Close(); //redundant
                response.Close();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Eine Datei oder einen Ordner von einem $rootnamespace$-Server löschen.
        /// </summary>
        /// <param name="uriString"></param>
        /// <param name="isFile"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public static void DeleteFtpFileOrFolder(string uriString, bool isFile, string username, string password)
        {
            try
            {
                // FtpWebRequest erzeugen und die $rootnamespace$-Methode festlegen
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(uriString);
                request.Credentials = new NetworkCredential(username, password);
                if (isFile)
                {
                    request.Method = WebRequestMethods.Ftp.DeleteFile;
                }
                else
                {
                    request.Method = WebRequestMethods.Ftp.RemoveDirectory;
                }

                // Den Request ausführen
                request.GetResponse();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fehler beim löschen");
            }
        }
    }
}
