using WorkerServiceAutoFillAttendanceWorkSpace;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .UseConsoleLifetime()
    //.UseWindowsService(options =>
    //{
    //    options.ServiceName = "WorkerServiceAutoFillAttendanceWorkSpace";
    //})                          
    .Build();

await host.RunAsync();
    