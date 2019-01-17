FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["AmoretuERPWeb/AmoretuERPWeb.csproj", "AmoretuERPWeb/"]
RUN dotnet restore "AmoretuERPWeb/AmoretuERPWeb.csproj"
COPY . .
WORKDIR "/src/AmoretuERPWeb"
RUN dotnet build "AmoretuERPWeb.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AmoretuERPWeb.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AmoretuERPWeb.dll"]