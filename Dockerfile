FROM microsoft/dotnet:2.1-runtime
COPY bin/Debug/netcoreapp2.1 .
ENTRYPOINT ["dotnet", "Etisalat_Mobile_Http_Data_Gen.dll"]