using System;
using System.Net;
using System.Timers;
using System.Net.Mail;

namespace IncendiosAlgoritmo

{
    class Program
    {

        static Timer timer;

        static void Main(string[] args)
        {
            Console.WriteLine("### Scheduled Task Started ### \n\n");

            Algoritmo a = new Algoritmo();
            String c = a.downloadFile();
            int r = a.loadIncendiosDiarios(c);
            if (r > 0) { a.alteraNivelCritico(); }

            Console.WriteLine("### Task Finished ### \n\n");

        }

        static void Timer_incendiosAnual()
        {
            Console.WriteLine("### Timer Started ###");

            DateTime nowTime = DateTime.Now;
            DateTime scheduledTime = new DateTime(nowTime.Year, nowTime.Month, nowTime.Day, 23, 59, 0, 0); //Specify your scheduled time HH,MM,SS [11pm]
            if (nowTime > scheduledTime)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }

            double tickTime = (double)(scheduledTime - DateTime.Now).TotalMilliseconds;
            timer = new Timer(tickTime);
            timer.Elapsed += new ElapsedEventHandler(timer_action);
            timer.Start();
        }

        static void timer_action(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("### Timer Stopped ### \n");
            timer.Stop();
            Console.WriteLine("### Scheduled Task Started ### \n\n");
            
            Algoritmo a = new Algoritmo();
            String c = a.downloadFile();
            int r = a.loadIncendiosDiarios(c);
            if (r > 0) { a.alteraNivelCritico(); }
            
            Console.WriteLine("### Task Finished ### \n\n");
            Timer_incendiosAnual();
        }






        public static void email()
        {
            

                SmtpClient SmtpServer = new SmtpClient("smtp.live.com");

                MailAddress addressFrom = new MailAddress("brancojse@hotmail.com");
                MailAddress addressTo = new MailAddress("aemiranda7@gmail.com");
                MailMessage message = new MailMessage(addressFrom, addressTo);

                message.Subject = "Sending Email with HTML Body";
                message.IsBodyHtml = true;
                string htmlString = @"<html>
                      <body>
                      <p>Dear Ms. Susan,</p>
                      <p>Thank you for your letter of yesterday inviting me to come for an interview on Friday afternoon, 5th July, at 2:30.
                              I shall be happy to be there as requested and will bring my diploma and other papers with me.</p>
                      <p>Sincerely,<br>-Jack</br></p>
                      </body>
                      </html>
                     ";
                message.Body = htmlString;

                SmtpClient client = new SmtpClient();
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("", "");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(message);


            
            
        }
    }
}
