FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["ConsultaCentroDistribuicaoMock/ConsultaCentroDistribuicaoMock.csproj", "ConsultaCentroDistribuicaoMock/"]
RUN dotnet restore "./ConsultaCentroDistribuicaoMock/ConsultaCentroDistribuicaoMock.csproj"

COPY . .
WORKDIR "/src/ConsultaCentroDistribuicaoMock"

RUN dotnet build "./ConsultaCentroDistribuicaoMock.csproj" -o /app/build
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .
RUN ls -l /app

ENV DELAY=40
ENV ERROR=0
ENV TODOS_CDS=false

EXPOSE 8080

ENTRYPOINT ["dotnet", "ConsultaCentroDistribuicaoMock.dll"]