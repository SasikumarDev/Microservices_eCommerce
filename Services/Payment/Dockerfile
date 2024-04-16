FROM mcr.microsoft.com/dotnet/sdk:8.0 as build-env

WORKDIR /source
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /PaymentAPI

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /PaymentAPI
COPY --from=build-env /PaymentAPI .
# ENV ASPNETCORE_URLS=http://+:5000
# EXPOSE 5000
ENTRYPOINT ["dotnet","Payment.dll"]