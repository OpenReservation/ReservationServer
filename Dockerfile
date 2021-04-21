FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build-env
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ActivityReservation.Common/*.csproj ActivityReservation.Common/
COPY ActivityReservation.Models/*.csproj ActivityReservation.Models/
COPY ActivityReservation.DataAccess/*.csproj ActivityReservation.DataAccess/
COPY ActivityReservation.Business/*.csproj ActivityReservation.Business/
COPY ActivityReservation.Helper/*.csproj ActivityReservation.Helper/
COPY ActivityReservation.WechatAPI/*.csproj ActivityReservation.WechatAPI/
COPY ActivityReservation.AdminLogic/*.csproj ActivityReservation.AdminLogic/
COPY ActivityReservation.API/*.csproj ActivityReservation.API/
COPY ActivityReservation/ActivityReservation.csproj ActivityReservation/

WORKDIR /src/ActivityReservation
RUN dotnet restore

# copy everything and build
COPY . .
RUN dotnet publish -c Release -o out ActivityReservation/ActivityReservation.csproj

# build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
LABEL Maintainer="WeihanLi"
WORKDIR /app
COPY --from=build-env /src/ActivityReservation/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "ActivityReservation.dll"]
