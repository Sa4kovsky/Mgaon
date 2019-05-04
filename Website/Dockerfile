FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Website.csproj", "Website/"]
RUN ls -al && pwd
RUN dotnet restore "Website/Website.csproj"
COPY . Website/
RUN ls -al && pwd
WORKDIR "/src/Website"
RUN ls -al && pwd
RUN dotnet build "Website.csproj" -c Release -o /app
RUN ls -al && pwd

FROM build AS publish
RUN dotnet publish "Website.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Website.dll"]