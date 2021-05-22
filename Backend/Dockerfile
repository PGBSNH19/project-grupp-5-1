FROM mcr.microsoft.com/dotnet/core/sdk:3.1
ENV ASPNETCORE_URLS=http://+:80
WORKDIR /app

# Copy API project folder
COPY . .

# Publish our API to a new folder
RUN dotnet publish -c Release -o ./publish

# Run published API
ENTRYPOINT ["dotnet", "./publish/Backend.dll"]