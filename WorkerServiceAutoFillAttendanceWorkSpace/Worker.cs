using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace WorkerServiceAutoFillAttendanceWorkSpace
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string filePath = "D:\\important\\122\\Projects\\Attendance.txt";
            int intervalSeconds = 20;

            while (!stoppingToken.IsCancellationRequested)
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--start-maximized"); // Open Chrome in maximized window
                // options.AddArgument("--headless"); 

                IWebDriver driver = new ChromeDriver(options);

                try
                {
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

                    // my workspace tab click
                    IWebElement myworkspaceTab = driver.FindElement(By.ClassName("ic18"));
                    myworkspaceTab.Click();

                    // attedance click tab
                    IWebElement attendanceLink = driver.FindElement(By.CssSelector("a[data-menid='1344']"));
                    attendanceLink.Click();

                    // fetching the current date class
                    IWebElement attendanceElement = driver.FindElement(By.CssSelector("td.calender-date-columns.CurrentDate"));
                    string attendanceClass = attendanceElement.GetAttribute("class");

                    // getting the currentdate class properties and after check 
                    // Working days "initial-edit-div"
                    if (attendanceClass.Contains("inline-edit-div"))
                    {
                        attendanceElement.Click();

                        IWebElement dropdown = attendanceElement.FindElement(By.CssSelector("select.inline-select.status"));
                        SelectElement select = new SelectElement(dropdown);
                        select.SelectByValue("P");

                        IWebElement saveButton = driver.FindElement(By.Id("btnSave"));
                        saveButton.Click();
                        string successMessage = $"Attendance fill at: {DateTimeOffset.Now}\n";
                        File.AppendAllText(filePath, successMessage);
                    }

                    // saturday-sunday WeekOff
                    else if (attendanceClass.Contains("WeekOff"))
                    {
                        string weekOffMessage = $"WeekOff: {DateTimeOffset.Now}\n";
                        File.AppendAllText(filePath, weekOffMessage);
                    }

                    driver.Close();
                    // Append the Attendance
                    string updateMessage = $"Update Attendance : {DateTimeOffset.Now}\n";
                    File.AppendAllText(filePath, updateMessage);

                    await Task.Delay(TimeSpan.FromSeconds(intervalSeconds), stoppingToken);
                }
                catch (Exception ex)
                {
                    driver.Close();
                    string errorMessage = $"Error to Fill the attendance at: {DateTimeOffset.Now}\n";
                    File.AppendAllText(filePath, errorMessage);
                }

            }
        }
    }
}