# api-client-shared-dotnet

## Hvordan utvikle på dette prosjektet?
Bruk Rider, som er C#-varianten av Intellij IDEA. Utvikling gjøres på master.

## Hvordan deploye?
En merge til beta vil lage en ny release med versjon gitt av `AssemblyVersion` i _AssemblyInfo.cs_, men med autoinkrementert patch. Merge til master vil føre til en produksjonsrelease, men kun om du har oppgradert versjonsnummeret manuelt. Vi bruker [Semantisk versjonering].(https://semver.org/)
