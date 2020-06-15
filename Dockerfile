FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base

RUN apk add libgdiplus --update-cache --repository http://dl-3.alpinelinux.org/alpine/edge/testing/ --allow-untrusted && \
    apk add terminus-font && \
    apk add --no-cache icu-libs
# https://www.abhith.net/blog/docker-sql-error-on-aspnet-core-alpine/
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build-env
WORKDIR /src

# Copy csproj and restore as distinct layers
# https://andrewlock.net/optimising-asp-net-core-apps-in-docker-avoiding-manually-copying-csproj-files-part-2/
COPY */*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done

## diff between netcore2.2 and netcore3.0
WORKDIR /src/OpenReservation
RUN dotnet restore

# copy everything and build
COPY . .
RUN dotnet publish -c Release -o out OpenReservation/OpenReservation.csproj

# build runtime image
FROM base AS final

LABEL Maintainer="WeihanLi"
WORKDIR /app
COPY --from=build-env /src/OpenReservation/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "OpenReservation.dll"]
