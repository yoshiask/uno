# goal: build uno via cla
# goal: build uno docs via cli
set CombinedConfiguration='Release|Any CPU'
msbuild Uno.UI.Build.csproj "/p:BUILD_BUILDNUMBER=test_test_8888" /m:16 /clp:PerformanceSummary;Summary /bl /r
pause
