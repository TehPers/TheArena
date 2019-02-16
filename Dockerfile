# Build solution
FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /TheArena
RUN dotnet restore ArenaV2
RUN dotnet publish ArenaV2 -c Release -o bin

# TODO: Run tests

# Run application
FROM microsoft/dotnet:2.2-runtime AS runtime
WORKDIR /TheArena
COPY --from=build /TheArena/bin ./
ENTRYPOINT [ "dotnet", "ArenaV2.dll" ]