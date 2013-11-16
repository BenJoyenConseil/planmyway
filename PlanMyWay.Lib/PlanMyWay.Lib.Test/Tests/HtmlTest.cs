using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlanMyWay.Lib.Manager;
using PlanMyWay.Lib.DataModel;
using PlanMyWay.Html;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Tasks;


namespace PlanMyWay.Lib.Test.Tests
{
    public class HtmlTest
    {

        static public void HtmlConstructTEST()
        {
            Element htmlFile = new Element();

            htmlFile.Tag = "html";
            Element head = new Element("head");
            head.Add(new Element("title") { Text = "Test html title" });
            Element body = new Element("body");
            body.Add(new Element("p") { Text = "<span color=\"Red\">Salut comment ça va??</span>" });
            htmlFile.Add(head);
            htmlFile.Add(body);
            try
            {
                IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();
                StreamWriter writeFile = new StreamWriter(new IsolatedStorageFileStream("rapport.html", FileMode.Create, file), Encoding.UTF8);
                writeFile.Write(htmlFile.ToString());
                writeFile.Flush();
            }
            catch (Exception e)
            {
            }
            EmailComposeTask emailtask = new EmailComposeTask();
            emailtask.Body = htmlFile.ToString();
            emailtask.Subject = "coucou";
            emailtask.Show();
        }
    }
}
