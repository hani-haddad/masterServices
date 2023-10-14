FROM mcr.microsoft.com/dotnet/core/sdk:3.1

RUN apt-get update && apt-get -y install bash rsync

ADD docker/ci /ci
WORKDIR /build

COPY *.csproj ./
RUN dotnet restore

COPY . ./
ENTRYPOINT ["/bin/bash", "/ci/start.sh"]
