# build the API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copies all files and folders from the current directory to the working directory in the container
COPY . .

# publishes API
RUN dotnet publish "Digital_Handbook_Portal/HandbookApi/HandbookApi.csproj" -c Release -o /app/publish

# runtime env
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# ports
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "HandbookApi.dll"]