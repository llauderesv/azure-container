FROM mcr.microsoft.com/dotnet/sdk:5.0 as build-env
LABEL author="Vincent Llauderes"

# ENV DOTNET_USE_POLLING_FILE_WATCHER=1
# ENV ASPNETCORE_ENVIRONMENT=development

WORKDIR /app
EXPOSE 5000/tcp

# Copy everything else and build
COPY . ./

# RUN dotnet tool install --global dotnet-ef
# CMD dotnet ef migrations add Initial
ENV ASPNETCORE_URLS=http://*:5000

# Command for container to run
ENTRYPOINT ["/bin/bash", "-c", "dotnet restore && dotnet watch run"]

## Instructions
# docker build -t azurecontainer -f Development.Dockerfile .
# docker run -p 5000:5000 azurecontainer --name azurecontainer-container