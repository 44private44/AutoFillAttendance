using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Threading.Tasks;

namespace YourWorkerServiceNamespace.Repositories
{
    public class AttendanceRepository
    {
        public async Task FillAttendance(string filePath)
        {

            // set the path of Crome Driver
            IWebDriver driver = new ChromeDriver(@"D:\important\122\Projects\chromedriver_win32");

            try
            {
                driver.Manage().Window.Maximize();

                // go to url directly
                driver.Navigate().GoToUrl("https://web2.anasource.com/workspace");

                // getting the input text and set to that in box
                IWebElement usernameField = driver.FindElement(By.Id("txtUserName"));
                IWebElement passwordField = driver.FindElement(By.Id("txtPassword"));

                string username = "Sohamkumar.Modi";
                string password = "$oh@m321";

                usernameField.SendKeys(username);
                passwordField.SendKeys(password);

                // login butn click
                IWebElement loginButton = driver.FindElement(By.Id("btnSave"));
                loginButton.Click();

                await Task.Delay(TimeSpan.FromSeconds(3));

                // my workspace tab click
                IWebElement myworkspaceTab = driver.FindElement(By.ClassName("ic18"));
                myworkspaceTab.Click();

                await Task.Delay(TimeSpan.FromSeconds(3));

                // attedance click tab
                IWebElement attendanceLink = driver.FindElement(By.CssSelector("a[data-menid='1344']"));
                attendanceLink.Click();

                await Task.Delay(TimeSpan.FromSeconds(3));

                // fetching the current date class
                try
                {
                    IWebElement attendancsEleDemo = driver.FindElement(By.CssSelector("td.calender-date-columns.CurrentDate"));
                }

                catch (Exception ex)
                {
                    // Week off or Holiday
                    driver.Quit();
                }

                IWebElement attendanceElement = driver.FindElement(By.CssSelector("td.calender-date-columns.CurrentDate"));
                string attendanceClass = attendanceElement.GetAttribute("class");

                // getting the currentdate class properties and after check 
                // Working days "initial-edit-div"
                if (attendanceClass.Contains("inline-edit-div"))
                {
                    attendanceElement.Click();
                    await Task.Delay(TimeSpan.FromSeconds(3));

                    IWebElement dropdown = attendanceElement.FindElement(By.CssSelector("select.inline-select.status"));
                    SelectElement select = new SelectElement(dropdown);
                    select.SelectByValue("P");
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    IWebElement saveButton = driver.FindElement(By.Id("btnSave"));
                    saveButton.Click();
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    string successMessage = $"Attendance fill at: {DateTimeOffset.Now}\n";
                    File.AppendAllText(filePath, successMessage);
                }

                // saturday-sunday WeekOff
                else if (attendanceClass.Contains("WeekOff"))
                {
                    string weekOffMessage = $"WeekOff: {DateTimeOffset.Now}\n";
                    File.AppendAllText(filePath, weekOffMessage);
                }

                // Holiday
                else if (attendanceClass.Contains("HolidayColor"))
                {
                    string holidayMessage = $"Holiday: {DateTimeOffset.Now}\n";
                    File.AppendAllText(filePath, holidayMessage);
                }
                else
                {
                    string attendanceFilledMessage = $"Attendance Already Filled.. Check at: {DateTimeOffset.Now}\n";
                    File.AppendAllText(filePath, attendanceFilledMessage);
                }

                driver.Quit();
            }
            catch (Exception ex)
            {
                driver.Quit();
                string errorMessage = $"Error to Fill the attendance at: {DateTimeOffset.Now}\n";
                File.AppendAllText(filePath, errorMessage);
                File.AppendAllText(filePath, ex.Message);
            }
        }
    }
}
