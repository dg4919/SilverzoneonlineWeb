using ApsMind.Olympiad.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace ApsMind.Olympiad.Portal.Models
{
    public class AdminNotification
    {
        DatabaseContext objDB = new DatabaseContext();
        public String NotificationByPostAndriod(String Message, String ContactNo, String GsmId)
        {
            try
            {
                Common.LogError(this.ToString(), "Top of News Notification");

                //var applicationID = "AIzaSyBlDsDuAL58I65MV-95DH2sfp9I5816jS8";
                var applicationID = "AIzaSyCCEjqv0hq15-SI8lgZhFZyZdtP8xePq8Q";

                var SENDER_ID = "929753112377";
                WebRequest tRequest;
                tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
                //Data post to server 

                string postData =
                   "collapse_key=score_update&time_to_live=1084&delay_while_idle=5&data.message="
                    + HttpUtility.UrlEncode(Message)
                    + "&data.contactNumber=" + HttpUtility.UrlEncode(ContactNo)
                    + "&data.time=" + System.DateTime.Now.ToString()
                     + "&registration_id=" + GsmId;


                Console.WriteLine(postData);
                Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                tRequest.ContentLength = byteArray.Length;
                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse tResponse = tRequest.GetResponse();
                dataStream = tResponse.GetResponseStream();
                StreamReader tReader = new StreamReader(dataStream);
                String sResponseFromServer = tReader.ReadToEnd();   //Get response from GCM server.
                //ViewBag.msg = sResponseFromServer;      //Assigning GCM response to Label text
                tReader.Close();
                dataStream.Close();

                tResponse.Close();
                Common.LogError(this.ToString(), postData);
                Common.LogError(this.ToString(), sResponseFromServer);

                return "Success Notification";

            }
            catch (Exception ex)
            {

                //Common.LogError("Inside Notification",ex);   //Assigning GCM response to Label text
                Common.LogError(this.ToString(), ex);
                return "Error In Notification";

            }

        }

        public String NotificationByPostIPhone(String Message, String ContactNo, String GsmId)
        {
            try
            {
                int port = 2195;
                String deviceID = "705384ca 92088770 f1f75679 3122f64c da64e314 7df77593 d8afeeff ef8d201a";
                String hostname = "gateway.push.apple.com";     // TEST
                //String hostname = "gateway.push.apple.com";           // REAL

                //        @"cert.p12";C:\Users\APSMIND1\Desktop\Ashvini\ApsMind.Olympiad.Portal\ApsMind.Olympiad.Portal\Content\InfoZone\ck.pem
                String certificatePath = HttpContext.Current.Server.MapPath("~/Content/InfoZone/ck.pem");
                //X509Certificate2 clientCertificate = new X509Certificate2(certificatePath, "");

                X509Certificate2 clientCertificate = new X509Certificate2(System.IO.File.ReadAllBytes(certificatePath), "", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
                X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);
                TcpClient client = new TcpClient(hostname, port);

                //SslStream _apnsStream = new SslStream(_apnsClient.GetStream(), false, validateServerCertificate, SelectLocalCertificate);
                //SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

                //SslStream sslStream = new SslStream(client.GetStream(), false, validateServerCertificate, SelectLocalCertificate);
                SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

                //SslStream sslStream = new SslStream(client.GetStream(), false, null, null);
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                    sslStream.AuthenticateAsClient(hostname, certificatesCollection, SslProtocols.Default, false);
                }
                catch (Exception e)
                {
                    throw (e);
                    client.Close();
                    //return;
                }

                MemoryStream memoryStream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(memoryStream);
                writer.Write((byte)0);  //The command
                writer.Write((byte)0);  //The first byte of the deviceId length (big-endian first byte)
                writer.Write((byte)32); //The deviceId length (big-endian second byte)

                writer.Write(HexStringToByteArray(deviceID.ToUpper()));
                String payload = "{\"aps\":{\"alert\":\"hello\",\"badge\":0,\"sound\":\"default\"}}";
                writer.Write((byte)0);
                writer.Write((byte)payload.Length);
                byte[] b1 = System.Text.Encoding.UTF8.GetBytes(payload);
                writer.Write(b1);
                writer.Flush();
                byte[] array = memoryStream.ToArray();
                sslStream.Write(array);
                sslStream.Flush();
                client.Close();
                return "Success Notification";

            }
            catch (Exception ex)
            {

                //Common.LogError("Inside Notification",ex);   //Assigning GCM response to Label text
                Common.LogError(this.ToString(), ex);
                return "Error In Notification";

            }

        }

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            throw new NotImplementedException();
        }

        private string HexStringToByteArray(string p)
        {
            throw new NotImplementedException();
        }
    }
}