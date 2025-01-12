FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

# copy http
# COPY --from=weihanli/dotnet-httpie /usr/bin/http /usr/bin/http

# use forward headers
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
# configure http port to use 80
ENV ASPNETCORE_HTTP_PORTS=80

LABEL Maintainer="WeihanLi"

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY nuget.config ./
COPY ./Directory.Build.props ./
COPY ./Directory.Packages.props ./
# https://andrewlock.net/optimising-asp-net-core-apps-in-docker-avoiding-manually-copying-csproj-files-part-2/
COPY */*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done

WORKDIR /src/OpenReservation
RUN dotnet restore

# copy everything and build
COPY . .
RUN dotnet publish -o out OpenReservation/OpenReservation.csproj -p:UseAppHost=false

# dotnet tools
RUN dotnet tool install dotnet-execute --tool-path ./out/tools/
RUN dotnet tool install dotnet-httpie --tool-path ./out/tools/

# build runtime image
FROM base AS final

WORKDIR /app
COPY --from=build-env /src/OpenReservation/out .

ENV PATH="/app/tools:${PATH}"

ENTRYPOINT ["dotnet", "OpenReservation.dll"]
