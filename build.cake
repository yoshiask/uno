//////////////////////////////////////////////////////////////////////
// TOOLS
//////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////
// ADDINS
//////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var target = Argument("target", "Default");

//////////////////////////////////////////////////////////////////////
// VARIABLES
//////////////////////////////////////////////////////////////////////
var local = BuildSystem.IsLocalBuild;
var isPullRequest = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTNUMBER"));

//////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
//////////////////////////////////////////////////////////////////////
Setup(context =>
{

});

Teardown(context =>
{
    // Executed AFTER the last task.
});

Task("BuildUno")
    .Does(() =>
{

    //   solution: Build/Uno.UI.Build.csproj
    //   msbuildLocationMethod: version
    //   msbuildVersion: latest
    //   msbuildArchitecture: x86
    //   msbuildArguments: /r /p:CheckExclusions=True "/p:CombinedConfiguration=$(CombinedConfiguration)" /nodeReuse:true /detailedsummary /m:16 /nr:false /bl:$(build.artifactstagingdirectory)\build.binlog
    //   clean: false
    //   maximumCpuCount: true
    //   restoreNugetPackages: false
    //   logProjectEvents: false
    //   createLogFile: false


    if (IsRunningOnWindows())
    {
        Information("We are running on Windows!");
    }

    if (IsRunningOnUnix())
    {
        Information("Not running on Windows!");
    }

});

Task("BuildWebsite")
    .Does(() =>
{

    //   msbuildLocationMethod: version
    //   msbuildVersion: latest
    //   msbuildArchitecture: x86
    //   msbuildArguments: /r /t:GenerateDoc /p:CheckExclusions=True "/p:CombinedConfiguration=$(CombinedConfiguration)" /detailedsummary
    //   clean: false
    //   maximumCpuCount: true
    //   restoreNugetPackages: false
    //   logProjectEvents: false
    //   createLogFile: false

    MSBuild("./build/Uno.UI.Build.csproj", settings =>
        settings.SetDetailedSummary(true)
                .SetNodeReuse(true)
                .SetMaxCpuCount(0) // If this value is zero, MSBuild will use as many processes as there are available CPUs to build the project.
                .WithProperty("CombinedConfiguration", "Release|Any CPU")
    );


});


Task("Default")
    .WithCriteria(() => local)
    .WithCriteria(() => !isPullRequest)
    .IsDependentOn("BuildUno")
    .IsDependentOn("BuildWebsite")
    .Does(() =>
{
});

RunTarget(target);
