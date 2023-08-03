using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using YourWorkerServiceNamespace.Repositories;

namespace WorkerServiceAutoFillAttendanceWorkSpace
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly AttendanceRepository _attendanceRepository;

        public Worker(ILogger<Worker> logger, AttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                string filePath = "D:\\important\\122\\Projects\\Attendance.txt";
                DateTime targetTime = DateTime.Today.AddHours(9).AddMinutes(30);
                DateTime nextDayTargetTime = targetTime.AddDays(1);

                DateTime currentTime = DateTime.Now;

                if (currentTime > targetTime)
                {
                    await _attendanceRepository.FillAttendance(filePath);

                    // It's already past the target time, so we'll wait until the next day's target time (9:30 AM)
                    TimeSpan timeUntilNextTarget = nextDayTargetTime - currentTime;
                    await Task.Delay(timeUntilNextTarget);
                }
                else
                {
                    // It's before the target time, so we'll wait until the target time (9:30 AM) of the current day
                    TimeSpan timeUntilTarget = targetTime - currentTime;
                    await Task.Delay(timeUntilTarget);
                }

                await _attendanceRepository.FillAttendance(filePath);

                DateTime nextDay = DateTime.Today.AddDays(1);
                DateTime nextDayTarget = nextDay.AddHours(9).AddMinutes(30);
                TimeSpan timeUntilNextDayTarget = nextDayTarget - DateTime.Now;
                await Task.Delay(timeUntilNextDayTarget);
            }

        }

    }
}