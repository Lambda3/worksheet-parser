#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.1-alpine AS base

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Samples/Worksheet.Parser.Api.Sample/Worksheet.Parser.Api.Sample.csproj", "Samples/Worksheet.Parser.Api.Sample/"]
COPY ["WorkSheet.Parser/Worksheet.Parser.csproj", "Worksheet.Parser/"]
COPY ["Worksheet.Parser.AspNet/Worksheet.Parser.AspNet.csproj", "Worksheet.Parser.AspNet/"]
COPY ["WorkSheet.Parser.ClosedXml/Worksheet.Parser.ClosedXml.csproj", "Worksheet.Parser.ClosedXml/"]
RUN dotnet restore -r linux-musl-x64 "Samples/Worksheet.Parser.Api.Sample/Worksheet.Parser.Api.Sample.csproj"
COPY . .
RUN dotnet build "/src/Samples/Worksheet.Parser.Api.Sample/Worksheet.Parser.Api.Sample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "/src/Samples/Worksheet.Parser.Api.Sample/Worksheet.Parser.Api.Sample.csproj" -c Release -r linux-musl-x64 -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["/app/Worksheet.Parser.Api.Sample"]
EXPOSE 80
