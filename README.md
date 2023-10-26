*** disclaimer: com dotnet 7 não é necessário dockerfile***

1. Criar imagem:

```
dotnet publish --os Informe_OS_Aqui --arch x64 -p:PublisherProfile=DefaultContainer
```
    
2. Rodar imagem com docker

```
    docker run -p 5000:80 testeapibrasil:1.0.0
```
