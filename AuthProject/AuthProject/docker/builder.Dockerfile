FROM mcr.microsoft.com/dotnet/sdk:7.0 

RUN apt-get update && apt-get -y install bash rsync
# RUN dotnet dev-certs https
# RUN dotnet dev-certs https --trust
# RUN mkdir -p /app
# RUN mkdir -p /app/AuthProject
# RUN mkdir -p /app/SharedModelNamespace
# COPY . /app

ADD AuthProject/AuthProject/docker/ci /ci
WORKDIR /build/
# Copy the solution and project files
COPY AuthProject/AuthProject/AuthProject.csproj ./AuthProject/
COPY SharedModelNamespace/SharedModelNamespace/SharedModelNamespace.csproj ./SharedModelNamespace/

RUN dotnet restore "./AuthProject/AuthProject.csproj" 


COPY . .
WORKDIR /build/AuthProject/
ENTRYPOINT ["/bin/bash", "/ci/start.sh"]

# ENTRYPOINT ["dotnet" ,"run"]

