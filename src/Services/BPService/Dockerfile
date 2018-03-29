FROM microsoft/dotnet:2-runtime

WORKDIR /app
COPY ./bin/Release/netcoreapp2.0/publish .
COPY bpservice.db .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Givestack.HealthCheck.BPService.dll", "--ip", "0.0.0.0"]