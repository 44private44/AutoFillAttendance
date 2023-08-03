using WorkerServiceAutoFillAttendanceWorkSpace;
using YourWorkerServiceNamespace.Repositories;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<AttendanceRepository>();
        services.AddHostedService<Worker>();
    })
    .UseWindowsService(options =>
    {
        options.ServiceName = "WorkerServiceAutoFillAttendanceWorkSpace";
    })
    .Build();

await host.RunAsync();

//sc create WorkerServiceAutoFillAttendanceWorkSpace binpath="D:\important\122\Projects\AutoFillAttendance\WorkerServiceAutoFillAttendanceWorkSpace\bin\Release\net6.0\publish\WorkerServiceAutoFillAttendanceWorkSpace.exe"
