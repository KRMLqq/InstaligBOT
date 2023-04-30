//IWebElement finishPage = webDriver.FindElement(By.Id("finish_page"));
/*
if(finishPage.GetCssValue("display") == "block")
{
    break;
} */
//try
//{

//try
//{
//wait.Until(ExpectedConditions.ElementIsVisible(By.Id("answer")));
//}
//catch
//{
//    wait.Until(ExpectedConditions.ElementIsVisible(By.Id("answer_translations")));
//}


//try
//{
//    wait.Until(ExpectedConditions.ElementIsVisible(By.Id("check")));
//}
//catch
//{
//    wait.Until(ExpectedConditions.ElementIsVisible(By.Id("nextword")));
//}

//wait.Until(ExpectedConditions.ElementIsVisible(By.Id("nextword")));
//Thread.Sleep(300);
//nextwordButton.Click();


//string transl = Regex.Matches(input, pattern, options)[0].Value;

//wait.Until(ExpectedConditions.ElementIsVisible(By.Id("answer")));

//Console.WriteLine("Przetłumaczone: " + trans.Text);

//string pattern = @"<div id=.word.>([\w ]+)</div>";
//string input = webDriver.PageSource;

//RegexOptions options = RegexOptions.Multiline;

//foreach (Match m in Regex.Matches(input, pattern, options))
//{
//    Console.WriteLine("'{0}' found at index {1}.", m.Value, m.Index);
//}

//WebDriverWait wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));