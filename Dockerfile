FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build-env
WORKDIR /src

# Copy csproj and restore as distinct layers
# https://andrewlock.net/optimising-asp-net-core-apps-in-docker-avoiding-manually-copying-csproj-files-part-2/
COPY */*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done

## diff between netcore2.2 and netcore3.0
WORKDIR /src/ActivityReservation
RUN dotnet restore

# copy everything and build
COPY . .
RUN dotnet publish -c Release -o out ActivityReservation/ActivityReservation.csproj

# build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine

RUN apk add libgdiplus --update-cache --repository http://dl-3.alpinelinux.org/alpine/edge/testing/ --allow-untrusted && \
    apk add terminus-font && \
    apk add icu-libs
# https://www.abhith.net/blog/docker-sql-error-on-aspnet-core-alpine/
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
LABEL Maintainer="WeihanLi"
WORKDIR /app
COPY --from=build-env /src/ActivityReservation/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "ActivityReservation.dll"]
