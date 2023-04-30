using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V111.IndexedDB;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace InstalingBOT
{
    public class Words
    {
        public string polish { get; set; }
        public string german { get; set; }
    }

    internal class Program
    {
        static string login = "337833376";
        static string password = "hafkt";
        static List<String> translateActive = new List<string> { };
        static List<Words> wordsList = new List<Words>();

        /*
         Loginy i hasła:

            //Grzehu
            static string login = "108977893";
            static string password = "exwhs";

            //Atur
            static string login = "165609099";
            static string password = "yunta";

            //Ja
            static string login = "178846898";
            static string password = "zsmjy";

            //losowe
            static string login = "337833376";
            static string password = "hafkt";
        */

        public static void InsertWords()
        {
            string fileName = "words.txt";

            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] pn = line.Split('|');

                    wordsList.Add(new Words { polish = pn[0], german = pn[1] });
                    translateActive.Add(pn[0]);
                }
            }
        }

        public static void GetNewWords()
        {
            string data = "";

            foreach(var el in wordsList)
            {
                if(el.german != "" && el.polish != "")
                {
                    data += el.polish + "|" + el.german + Environment.NewLine;
                }
            }

            File.WriteAllText("words.txt", data);
        }


        static void Main(string[] args)
        {
            InsertWords();
            Console.Clear();
            Console.WriteLine("Ropoczynam rozwiązywanie Instaling...");
            EdgeOptions options = new EdgeOptions();
            //options.AddArgument("--headless");
            //options.EnableMobileEmulation("iPhone X");
            options.AddAdditionalOption("userAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3 Edge/16.16299");
            IWebDriver webDriver = new EdgeDriver(options);
            webDriver.Navigate().GoToUrl("https://instaling.pl/teacher.php?page=login");

            IWebElement loginInput = webDriver.FindElement(By.Id("log_email"));
            loginInput.SendKeys(login);
            IWebElement passInput = webDriver.FindElement(By.Id("log_password"));
            passInput.SendKeys(password);

            Actions actions = new Actions(webDriver);
            actions.SendKeys(Keys.Enter).Perform();

            string[] linkElement = webDriver.Url.Split('=');
            string userId = linkElement[linkElement.Length - 1];

            Console.WriteLine("ID użytkownika: " + userId);

            webDriver.Navigate().GoToUrl("https://instaling.pl/ling2/html_app/app.php?child_id=" + userId);

            IWebElement checkIssetContinue = webDriver.FindElement(By.Id("continue_session_page"));
            IWebElement checkStart = webDriver.FindElement(By.Id("start_session_page"));

            if (checkIssetContinue.GetCssValue("display") != "none")
            {
                Console.WriteLine("Wykryto dokończenie sesji...");
                IWebElement continueSessionButton = webDriver.FindElement(By.Id("continue_session_button"));
                continueSessionButton.Click();
            }
            else if (checkStart.GetCssValue("display") != "none")
            {
                Console.WriteLine("Wykryto brak sesji...");
                IWebElement startSessionButton = webDriver.FindElement(By.Id("start_session_button"));
                startSessionButton.Click();
            }

            while(true)
            {
                Thread.Sleep(300);

                IWebElement translateCharacters = webDriver.FindElements(By.ClassName("translations"))[0];
                IWebElement inputDiv = webDriver.FindElement(By.Id("answer"));
                IWebElement nextButton = webDriver.FindElement(By.Id("check"));
                IWebElement nextwordButton = webDriver.FindElement(By.Id("nextword"));
                IWebElement trans = webDriver.FindElement(By.Id("word"));
                IWebElement green = null;
                IWebElement logout = null;

                string transNiemickie = trans.Text;               
                bool check = true;
                string translateCharactersVal = translateCharacters.Text;

                if (translateActive.Contains(translateCharactersVal) == false)
                {
                    if(nextButton.Displayed)
                    {
                        nextButton.Click();

                        try
                        {
                            green = webDriver.FindElement(By.Id("green"));
                            
                            if(green.Text != "")
                            {
                                check = false;
                            }
                        }
                        catch { }                      
                    }

                    if(check == false)
                    {
                        nextwordButton.Click();
                    }

                    Thread.Sleep(300);
                    trans = webDriver.FindElement(By.Id("word"));
                    
                    wordsList.Add(new Words { polish = translateCharactersVal, german = trans.Text });
                    translateActive.Add(translateCharactersVal);

                    actions.SendKeys(Keys.Enter).Perform();
                }
                else
                {
                    Thread.Sleep(300);

                    Words valuefromdatabase = wordsList.Find(x => x.polish == translateCharactersVal);
                    
                    if(inputDiv.Displayed)
                    {
                        inputDiv.SendKeys(valuefromdatabase.german);
                        actions.SendKeys(Keys.Enter).Perform();
                    }
                    else
                    {
                        actions.SendKeys(Keys.Enter).Perform();
                    }

                    try
                    {
                        logout = webDriver.FindElement(By.LinkText("Wyloguj"));

                        if(logout.Displayed)
                        {
                            logout.Click();
                            GetNewWords();
                            break;
                        }
                    }
                    catch { }
                }
                //Console.WriteLine(translateCharactersVal);
            }
        }
    }
}