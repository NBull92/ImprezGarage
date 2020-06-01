@echo off
SET file=..\ImprezGarage.nuspec

FOR /F "USEBACKQ" %%F IN (`powershell -NoLogo -NoProfile -Command ^(Get-Item "..\Src\ImprezGarage\bin\Release\ImprezGarage.exe"^).VersionInfo.FileVersion`) DO (SET fileVersion=%%F)
echo File version: %fileVersion%

@echo on
rem Saved in %file%
@echo off

echo ^<package^> > %file%
echo 	^<metadata^> >> %file%
echo 		^<id^>ImprezGarage^</id^> >> %file%
echo 		^<version^>%fileVersion%^</version^> >> %file%
echo 		^<title^>ImprezGarage^</title^> >> %file%
echo 		^<authors^>Nicholas Bull^</authors^> >> %file%
echo 		^<owners^>Nicholas Bull^</owners^> >> %file%
echo 		^<requireLicenseAcceptance^>false^</requireLicenseAcceptance^> >> %file%
echo 		^<copyright^>NickBull-Computing 2020^</copyright^> >> %file%
echo 		^<tags^> ^</tags^> >> %file%
echo		^<projectUrl^>https://github.com/NBull92/ImprezGarage^</projectUrl^> >> %file%
echo		^<iconUrl^>https://github.com/NBull92/ImprezGarage/Src/ImprezGarage/iconv3.ico^</iconUrl^> >> %file%
echo		^<description^>An application that is used to help guide you in the maintenance of you vehicles.^</description^> >> %file%
echo 	^</metadata^> >> %file%
echo		^<files^> >> %file%
echo			^<file src="lib\net461\CommonServiceLocator.dll" target="lib\net461\CommonServiceLocator.dll" /^>  >> %file%
echo			^<file src="lib\net461\ControlzEx.dll" target="lib\net461\ControlzEx.dll" /^>  >> %file%
echo			^<file src="lib\net461\CountriesWrapper.dll" target="lib\net461\CountriesWrapper.dll" /^>  >> %file%
echo			^<file src="lib\net461\DeltaCompressionDotNet.dll" target="lib\net461\DeltaCompressionDotNet.dll" /^>  >> %file%
echo			^<file src="lib\net461\DeltaCompressionDotNet.MsDelta.dll" target="lib\net461\DeltaCompressionDotNet.MsDelta.dll" /^>  >> %file%
echo			^<file src="lib\net461\DeltaCompressionDotNet.PatchApi.dll" target="lib\net461\DeltaCompressionDotNet.PatchApi.dll" /^>  >> %file%
echo			^<file src="lib\net461\EntityFramework.dll" target="lib\net461\EntityFramework.dll" /^>  >> %file%
echo			^<file src="lib\net461\EntityFramework.SqlServer.dll" target="lib\net461\EntityFramework.SqlServer.dll" /^>  >> %file%
echo			^<file src="lib\net461\Firebase.Auth.dll" target="lib\net461\Firebase.Auth.dll" /^>  >> %file%
echo			^<file src="lib\net461\FireSharp.dll" target="lib\net461\FireSharp.dll" /^>  >> %file%
echo			^<file src="lib\net461\ImprezGarage.exe" target="lib\net461\ImprezGarage.exe" /^>  >> %file%
echo			^<file src="lib\net461\ImprezGarage.exe.config" target="lib\net461\ImprezGarage.exe.config" /^>  >> %file%
echo			^<file src="lib\net461\ImprezGarage.exe.manifest" target="lib\net461\ImprezGarage.exe.manifest" /^>  >> %file%
echo			^<file src="lib\net461\ImprezGarage.Infrastructure.dll" target="lib\net461\ImprezGarage.Infrastructure.dll" /^>  >> %file%
echo			^<file src="lib\net461\ImprezGarage.Infrastructure.Services.dll" target="lib\net461\ImprezGarage.Infrastructure.Services.dll" /^>  >> %file%
echo			^<file src="lib\net461\ImprezGarage.Modules.Account.dll" target="lib\net461\ImprezGarage.Modules.Account.dll" /^>  >> %file%
echo			^<file src="lib\net461\ImprezGarage.Modules.Firebase.dll" target="lib\net461\ImprezGarage.Modules.Firebase.dll" /^>  >> %file%
echo			^<file src="lib\net461\ImprezGarage.Modules.FirebaseAuth.dll" target="lib\net461\ImprezGarage.Modules.FirebaseAuth.dll" /^>  >> %file%
echo			^<file src="lib\net461\ImprezGarage.Modules.Logger.dll" target="lib\net461\ImprezGarage.Modules.Logger.dll" /^>  >> %file%
echo			^<file src="lib\net461\ImprezGarage.Modules.MyGarage.dll" target="lib\net461\ImprezGarage.Modules.MyGarage.dll" /^>  >> %file%
echo			^<file src="lib\net461\ImprezGarage.Modules.Notifications.dll" target="lib\net461\ImprezGarage.Modules.Notifications.dll" /^>  >> %file%
echo			^<file src="lib\net461\ImprezGarage.Modules.PerformChecks.dll" target="lib\net461\ImprezGarage.Modules.PerformChecks.dll" /^>  >> %file%
echo			^<file src="lib\net461\ImprezGarage.Modules.PetrolExpenditure.dll" target="lib\net461\ImprezGarage.Modules.PetrolExpenditure.dll" /^>  >> %file%
echo			^<file src="lib\net461\ImprezGarage.Modules.Settings.dll" target="lib\net461\ImprezGarage.Modules.Settings.dll" /^>  >> %file%
echo			^<file src="lib\net461\LiveCharts.dll" target="lib\net461\LiveCharts.dll" /^>  >> %file%
echo			^<file src="lib\net461\LiveCharts.Wpf.dll" target="lib\net461\LiveCharts.Wpf.dll" /^>  >> %file%
echo			^<file src="lib\net461\MahApps.Metro.dll" target="lib\net461\MahApps.Metro.dll" /^>  >> %file%
echo			^<file src="lib\net461\Microsoft.Bcl.AsyncInterfaces.dll" target="lib\net461\Microsoft.Bcl.AsyncInterfaces.dll" /^>  >> %file%
echo			^<file src="lib\net461\Microsoft.Expression.Controls.dll" target="lib\net461\Microsoft.Expression.Controls.dll" /^>  >> %file%
echo			^<file src="lib\net461\Microsoft.Expression.Drawing.dll" target="lib\net461\Microsoft.Expression.Drawing.dll" /^>  >> %file%
echo			^<file src="lib\net461\Microsoft.Expression.Effects.dll" target="lib\net461\Microsoft.Expression.Effects.dll" /^>  >> %file%
echo			^<file src="lib\net461\Microsoft.Expression.Interactions.dll" target="lib\net461\Microsoft.Expression.Interactions.dll" /^>  >> %file%
echo			^<file src="lib\net461\Microsoft.Expression.Prototyping.Interactivity.dll" target="lib\net461\Microsoft.Expression.Prototyping.Interactivity.dll" /^>  >> %file%
echo			^<file src="lib\net461\Microsoft.Expression.Prototyping.SketchControls.dll" target="lib\net461\Microsoft.Expression.Prototyping.SketchControls.dll" /^>  >> %file%
echo			^<file src="lib\net461\Microsoft.Practices.ServiceLocation.dll" target="lib\net461\Microsoft.Practices.ServiceLocation.dll" /^>  >> %file%
echo			^<file src="lib\net461\Microsoft.Practices.Unity.dll" target="lib\net461\Microsoft.Practices.Unity.dll" /^>  >> %file%
echo			^<file src="lib\net461\Microsoft.SDK.Expression.Blend.dll" target="lib\net461\Microsoft.SDK.Expression.Blend.dll" /^>  >> %file%
echo			^<file src="lib\net461\Microsoft.Win32.Primitives.dll" target="lib\net461\Microsoft.Win32.Primitives.dll" /^>  >> %file%
echo			^<file src="lib\net461\Mono.Cecil.dll" target="lib\net461\Mono.Cecil.dll" /^>  >> %file%
echo			^<file src="lib\net461\Mono.Cecil.Mdb.dll" target="lib\net461\Mono.Cecil.Mdb.dll" /^>  >> %file%
echo			^<file src="lib\net461\Mono.Cecil.Pdb.dll" target="lib\net461\Mono.Cecil.Pdb.dll" /^>  >> %file%
echo			^<file src="lib\net461\Mono.Cecil.Rocks.dll" target="lib\net461\Mono.Cecil.Rocks.dll" /^>  >> %file%
echo			^<file src="lib\net461\netstandard.dll" target="lib\net461\netstandard.dll" /^>  >> %file%
echo			^<file src="lib\net461\Newtonsoft.Json.dll" target="lib\net461\Newtonsoft.Json.dll" /^>  >> %file%
echo			^<file src="lib\net461\NuGet.Squirrel.dll" target="lib\net461\NuGet.Squirrel.dll" /^>  >> %file%
echo			^<file src="lib\net461\Prism.dll" target="lib\net461\Prism.dll" /^>  >> %file%
echo			^<file src="lib\net461\Prism.Unity.Wpf.dll" target="lib\net461\Prism.Unity.Wpf.dll" /^>  >> %file%
echo			^<file src="lib\net461\Prism.Wpf.dll" target="lib\net461\Prism.Wpf.dll" /^>  >> %file%
echo			^<file src="lib\net461\RestSharp.dll" target="lib\net461\RestSharp.dll" /^>  >> %file%
echo			^<file src="lib\net461\SharpCompress.dll" target="lib\net461\SharpCompress.dll" /^>  >> %file%
echo			^<file src="lib\net461\Splat.dll" target="lib\net461\Splat.dll" /^>  >> %file%
echo			^<file src="lib\net461\Squirrel.dll" target="lib\net461\Squirrel.dll" /^>  >> %file%

echo			^<file src="lib\net461\Squirrel.dll" target="lib\net461\Squirrel.dll" /^>  >> %file%


echo			^<file src="lib\net461\Unity.Abstractions.dll" target="lib\net461\Unity.Abstractions.dll" /^>  >> %file%
echo			^<file src="lib\net461\Unity.Configuration.dll" target="lib\net461\Unity.Configuration.dll" /^>  >> %file%
echo			^<file src="lib\net461\Unity.Container.dll" target="lib\net461\Unity.Container.dll" /^>  >> %file%
echo			^<file src="lib\net461\Unity.Interception.Configuration.dll" target="lib\net461\Unity.Interception.Configuration.dll" /^>  >> %file%
echo			^<file src="lib\net461\Unity.Interception.dll" target="lib\net461\Unity.Interception.dll" /^>  >> %file%
echo			^<file src="lib\net461\Unity.RegistrationByConvention.dll" target="lib\net461\Unity.RegistrationByConvention.dll" /^>  >> %file%
echo			^<file src="lib\net461\Unity.ServiceLocation.dll" target="lib\net461\Unity.ServiceLocation.dll" /^>  >> %file%
echo			^<file src="lib\net461\WPFToolkit.dll" target="lib\net461\WPFToolkit.dll" /^>  >> %file%
echo			^<file src="lib\net461\Xceed.Wpf.AvalonDock.dll" target="lib\net461\Xceed.Wpf.AvalonDock.dll" /^>  >> %file%
echo			^<file src="lib\net461\Xceed.Wpf.AvalonDock.Themes.Aero.dll" target="lib\net461\Xceed.Wpf.AvalonDock.Themes.Aero.dll" /^>  >> %file%
echo			^<file src="lib\net461\Xceed.Wpf.AvalonDock.Themes.Metro.dll" target="lib\net461\Xceed.Wpf.AvalonDock.Themes.Metro.dll" /^>  >> %file%
echo			^<file src="lib\net461\Xceed.Wpf.AvalonDock.Themes.VS2010.dll" target="lib\net461\Xceed.Wpf.AvalonDock.Themes.VS2010.dll" /^>  >> %file%
echo			^<file src="lib\net461\Xceed.Wpf.Toolkit.dll" target="lib\net461\Xceed.Wpf.Toolkit.dll" /^>  >> %file%
echo		^</files^> >> %file%
echo ^</package^> >> %file%


nuget.exe pack ..\ImprezGarage.nuspec
pause