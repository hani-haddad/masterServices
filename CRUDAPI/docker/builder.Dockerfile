FROM mcr.microsoft.com/dotnet/sdk:7.0 

RUN apt-get update && apt-get -y install bash rsync
# RUN dotnet dev-certs https
# RUN dotnet dev-certs https --trust
# RUN mkdir -p /app
# RUN mkdir -p /app/AuthProject
# RUN mkdir -p /app/SharedModelNamespace
# COPY . /app

ADD CRUDAPI/docker/ci /CRUDAPI/ci
WORKDIR /build/
# Copy the solution and project files
COPY CRUDAPI/CRUDAPI.csproj ./CRUDAPI/
COPY SharedModelNamespace/SharedModelNamespace/SharedModelNamespace.csproj ./SharedModelNamespace/

RUN dotnet restore "./CRUDAPI/CRUDAPI.csproj" 


COPY . .
WORKDIR /build/CRUDAPI/
ENTRYPOINT ["/bin/bash", "/CRUDAPI/ci/start.sh"]

# ENTRYPOINT ["dotnet" ,"restore"]

