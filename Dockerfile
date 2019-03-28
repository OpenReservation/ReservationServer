FROM microsoft/dotnet:2.2-sdk-alpine AS build-env
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ActivityReservation.Common/*.csproj ActivityReservation.Common/
COPY ActivityReservation.Models/*.csproj ActivityReservation.Models/
COPY ActivityReservation.DataAccess/*.csproj ActivityReservation.DataAccess/
COPY ActivityReservation.Business/*.csproj ActivityReservation.Business/
COPY ActivityReservation.Helper/*.csproj ActivityReservation.Helper/
COPY ActivityReservation.WechatAPI/*.csproj ActivityReservation.WechatAPI/
COPY ActivityReservation.AdminLogic/*.csproj ActivityReservation.AdminLogic/
COPY ActivityReservation/ActivityReservation.csproj ActivityReservation/
RUN dotnet restore ActivityReservation/ActivityReservation.csproj

# copy everything and build
COPY . .
RUN dotnet publish -c Release -o out ActivityReservation/ActivityReservation.csproj

# build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
LABEL Maintainer="WeihanLi"
WORKDIR /app
COPY --from=build-env /src/ActivityReservation/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "ActivityReservation.dll"]
