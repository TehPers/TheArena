# Build solution
FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
EXPOSE 12276

# Build solution
WORKDIR /src
COPY TheArena/. .
RUN dotnet restore
RUN dotnet build -c Release -o /app

# TODO: Run tests

# Publish
FROM build AS publish
RUN dotnet publish -c Release -o /app

# Run application
FROM microsoft/dotnet:2.2-runtime AS runtime
WORKDIR /app
COPY --from=publish /app .
RUN ls -al
ENTRYPOINT [ "dotnet", "ArenaV2.dll" ]