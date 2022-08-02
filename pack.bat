@echo on
echo %1

::create nuget_pub
if not exist nuget_pub (
    md nuget_pub
)

::clear nuget_pub
for /R "nuget_pub" %%s in (*) do (
    del %%s
)

::core
dotnet pack framework/core/YH.NetMicro.AutoMapper -c Release -o nuget_pub
dotnet pack framework/core/YH.NetMicro.Cache -c Release -o nuget_pub
dotnet pack framework/core/YH.NetMicro.Core -c Release -o nuget_pub
dotnet pack framework/core/YH.NetMicro.Elasticsearch -c Release -o nuget_pub
dotnet pack framework/core/YH.NetMicro.HttpClient -c Release -o nuget_pub
dotnet pack framework/core/YH.NetMicro.JwtBearer -c Release -o nuget_pub
dotnet pack framework/core/YH.NetMicro.MongoProvider -c Release -o nuget_pub
dotnet pack framework/core/YH.NetMicro.Prometheus -c Release -o nuget_pub
dotnet pack framework/core/YH.NetMicro.Swagger -c Release -o nuget_pub
dotnet pack framework/core/YH.NetMicro.Trace -c Release -o nuget_pub
dotnet pack framework/core/YH.NetMicro.Validations -c Release -o nuget_pub
dotnet pack framework/core/YH.NetMicro.Webs -c Release -o nuget_pub
dotnet pack framework/core/YH.NetMicro.Config -c Release -o nuget_pub


::ddd
dotnet pack framework/ddd/YH.NetMicro.Applications -c Release -o nuget_pub
dotnet pack framework/ddd/YH.NetMicro.Domains -c Release -o nuget_pub


::host
dotnet pack framework/host/YH.NetMicro.Host -c Release -o nuget_pub


::business

dotnet pack framework/business/YH.ServicePlatform.Business -c Release -o nuget_pub

::nacos
dotnet pack framework/nacos/YH.NetMicro.Nacos -c Release -o nuget_pub

::tools
dotnet pack framework/tools/YH.NetMicro.QrCode -c Release -o nuget_pub
dotnet pack framework/tools/YH.NetMicro.ExcelMapper -c Release -o nuget_pub

::event
dotnet pack framework/event/YH.NetMicro.EventBus -c Release -o nuget_pub
dotnet pack framework/event/YH.NetMicro.EventBus.MySql -c Release -o nuget_pub
dotnet pack framework/event/YH.NetMicro.EventBus.RabbitMQ -c Release -o nuget_pub
dotnet pack framework/event/YH.NetMicro.EventBus.Kafka -c Release -o nuget_pub

for /R "nuget_pub" %%s in (*symbols.nupkg) do (
    del %%s
)

echo.
echo.

set /p key=input key:
set source=http://nuget.escase.cn/repository/nuget-hosted/

for /R "nuget_pub" %%s in (*.nupkg) do (
    call nuget.exe  push %%s -ApiKey %key% -Source %source%
    echo.
)

pause