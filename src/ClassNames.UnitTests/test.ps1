dotnet test --collect:"XPlat Code Coverage" 
reportgenerator "-reports:./TestResults/**/coverage.cobertura.xml" "-targetdir:./TestResults/"  -reporttypes:Html
rm ./TestResults/**/coverage.cobertura.xml