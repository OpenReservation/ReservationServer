FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

# use forward headers
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

LABEL Maintainer="WeihanLi"

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ./Directory.Build.props ./
# https://andrewlock.net/optimising-asp-net-core-apps-in-docker-avoiding-manually-copying-csproj-files-part-2/
COPY */*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done

WORKDIR /src/OpenReservation
RUN dotnet restore

# copy everything and build
COPY . .
RUN dotnet publish -c Release -o out OpenReservation/OpenReservation.csproj

# build runtime image
FROM base AS final

WORKDIR /app
COPY --from=build-env /src/OpenReservation/out .

ENTRYPOINT ["dotnet", "OpenReservation.dll"]
