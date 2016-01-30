using NthsKeys.KeyCrawler;
using NthsKeys.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Net.Http;

namespace NthsKeys.Runner
{
    public struct Logging
    {
        public static void WriteLine(string str)
        {
            string log = string.Format("{0} ,{1:D3} ---{2}", DateTime.Now.ToString(), DateTime.Now.Millisecond, str);
            Console.WriteLine(log);
        }
    }
    class Program
    {
        static int taskLeftCount = 0;
        static EventWaitHandle taskProcessingHandle = new EventWaitHandle(true, EventResetMode.ManualReset);
        static HttpClient httpClient = new HttpClient();
        static void Main(string[] args)
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "bdbai crawler");

            var lastUpdate = LastUpdate;
            var latestUpdate = lastUpdate;
            var latestArchives = new List<KeyCrawler.Attachment>();
            // while (true)
            // {



            // Thread.Sleep(TimeSpan.FromHours(2f));


            bool newOne = false;
            string mailContent = string.Empty;
            var categoryPage = new CategoryPage();
            foreach (var bulletin in categoryPage.GetBulletins())
            {
                Logging.WriteLine(string.Format("Post fetched: {0}", bulletin.Title));
                if (bulletin.Date <= lastUpdate)
                {
                    continue;
                }
                else
                {
                    if (bulletin.Date > latestUpdate)
                    {
                        latestUpdate = bulletin.Date;
                    }
                }
                if (bulletin.Title.Contains("高二")
                    && bulletin.Title.Contains("答案"))
                {
                    Logging.WriteLine(string.Format("Post detected: {0}", bulletin.Title));
                    mailContent = string.Format("{0}\r\n{1}", bulletin.Title, bulletin.Url);
                    newOne = true;
                    foreach (var archive in bulletin.GetAttachments())
                    {
                        ProcessArchive(archive);
                    }
                }
            }
            if (newOne)
            {
                LastUpdate = lastUpdate = latestUpdate;
                // SendMail(mailContent);
                // continue;
            }
            // }
            while (taskLeftCount > 0)
            {
                Logging.WriteLine("正在等待 " + taskLeftCount + " 个任务完成...");
                taskProcessingHandle.WaitOne();
            }
            Logging.WriteLine("所有任务完成。按任意键退出。");
            Console.ReadKey();
        }
        //static void SendMail(string mailContent)
        //{







        //    return;














        //    Task.Run(new Action(() =>
        //    {
        //        using (SmtpClient sc = new SmtpClient("smtp-mail.outlook.com", 25))
        //        {
        //            MailMessage mm;
        //            sc.EnableSsl = true;
        //            sc.Credentials = new NetworkCredential("email@hotmail.com", "password");
        //            mm = new MailMessage(new MailAddress("email@hotmail.com", "包布丁"), new MailAddress("email@hotmail.com", "包布丁"));
        //            mm.Subject = "First answer published!";
        //            mm.Body = mailContent;
        //            try
        //            {
        //                Logging.WriteLine("Sending a mail.");
        //                sc.Send(mm);
        //                Logging.WriteLine("Mail sent.");
        //            }
        //            catch (Exception ex)
        //            {
        //                Logging.WriteLine("Error while sending a mail:" + ex.ToString());
        //            }
        //            Logging.WriteLine("Press enter to continue...");
        //            Console.ReadLine();
        //        }
        //    }));
        //}
        static void ProcessArchive(KeyCrawler.Attachment archive)
        {
            Interlocked.Increment(ref taskLeftCount);
            taskProcessingHandle.Reset();
            string path = Properties.Settings.Default.pathPrefix + @"\archive\" + archive.Name;
            httpClient.GetByteArrayAsync(archive.Url).ContinueWith(new Action<Task<byte[]>>(async (result) =>
            {
                System.IO.File.WriteAllBytes(path, await result);
                Logging.WriteLine("Download done: " + archive.Name);
                Logging.WriteLine("Saved at: " + path);
                Logging.WriteLine("Writing to db: " + archive.Name);
                using (Model m = new Model())
                {
                    m.archives.Add(new archive()
                    {
                        Path = path,
                        CreateTime = DateTime.Now
                    });
                    await m.SaveChangesAsync();
                }
                Logging.WriteLine("db written: " + archive.Name);
                if (Interlocked.Decrement(ref taskLeftCount) == 0)
                {
                    taskProcessingHandle.Set();
                }
            }));
            Logging.WriteLine("Download started: " + archive.Name);
        }

        static DateTime LastUpdate
        {
            get
            {
                using (Model m = new Model())
                {
                    var data = m.settings.Where(i => i.Key == "lastUpdate");
                    if (data.Count() == 0) return DateTime.Now - TimeSpan.FromDays(365);
                    var result = data.First();
                    string date = result.Value;
                    return DateTime.Parse(date);
                }
            }
            set
            {
                using (Model m = new Model())
                {
                    var data = m.settings.Where(i => i.Key == "lastUpdate");
                    if (data.Count() == 0)
                    {
                        setting setting = new setting();
                        setting.Key = "lastUpdate";
                        setting.Value = value.ToString();
                        m.settings.Add(setting);
                    }
                    else
                    {
                        var result = data.First();
                        result.Value = value.ToString();
                    }
                    m.SaveChanges();
                }
            }
        }
    }
}
