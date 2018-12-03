FROM microsoft/dotnet:2.2-sdk-alpine AS build-env
WORKDIR /src

# copy everything and build
COPY . .
RUN dotnet restore 
RUN dotnet publish ActivityReservation/ActivityReservation.csproj -c Release -o out

# build runtime image
FROM microsoft/dotnet:2.2-runtime-alpine
LABEL Maintainer="WeihanLi"
WORKDIR /app
COPY --from=build-env /src/ActivityReservation/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "ActivityReservation.dll"]
