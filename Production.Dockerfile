# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 as build-env
WORKDIR /src
# Copy csproj to working directory
COPY ["AzureContainer.csproj", "./"]
# Restore the project to make sure that it is working fine.
RUN dotnet restore "./AzureContainer.csproj"

# Copy everything after restore
COPY . .
WORKDIR "/src/."
# Perform a build in current working directory
RUN dotnet build "AzureContainer.csproj" -c Release -o /app

FROM build-env as publish
# run dotnet publish to current working directory..
RUN dotnet publish "AzureContainer.csproj" -c Release -o /app/out

FROM base as final
ENV ASPNETCORE_URLS=http://*:80
# Expose port 5000 docker host
EXPOSE 80
WORKDIR /app
COPY --from=publish /app/out .

# Command for container to run
ENTRYPOINT ["dotnet", "AzureContainer.dll"]

# Notes:
# Expose is a matter of documeting your Dockerfile so that 

## Instructions
# docker build -t azurecontainer -f Production.Dockerfile .
# docker run --rm -p 3000:80 --name azurecontainer-container azurecontainer 

# -p is a way to specify port in the docker the first segment is what port should docker expose to the client.
# the 2nd segment port is the port of your host application expose so that docker could map the host port to client port.
