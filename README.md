# Fiap Tech Challenge - Catalog API
Esse microservice é reponsável por gerenciar, recuperar e possibilitar operações CRUD (Create, Read, Update, Delete) em relação a todos os produtos disponíveis no sistema de totem.

## Technologies / Components implemented

- .NET 6
    - ASP.NET MVC Core
    - ASP.NET WebApi 
    - Background Services

- Components / Services
    - RabbitMQ
    - MongoDb
 
## Rodando a API em localhost
1. Instale o Docker: [www.docker.com/](https://www.docker.com/get-started/)
- Após instalado confira se o docker está em execução:

 ```sh
docker ps
 ```

2. Clone este repositório para o seu ambiente local.

3. Navegue até o diretório raiz do projeto.

4. No terminal, execute o seguinte comando para fazer o build e subir todos serviços:

 ```sh
docker build -t totem-order-api .
docker compose up -d
 ```

Agora basta abrir /swagger
Ficando assim: http://localhost:5010/swagger

5. Antes de executar o fluxo completo é necessario fazer o build de todas apis:
Totem Order API: https://github.com/ecsj/Totem.Order.API
 ```sh
docker build -t totem-order-api .
 ```
Totem Catalog API: https://github.com/ecsj/Totem.Api.Catalog
 ```sh
docker build -t totem-catalog-api .
 ```
Totem Payment API: https://github.com/ecsj/Totem.Api.Payment
 ```sh
docker build -t totem-payment-api .
 ```
Totem Production API: https://github.com/ecsj/Totem.Api.Production
 ```sh
docker build -t totem-production-api .
 ```

Por fim, executar todos serviços:

 ```sh
docker compose up -d
 ```

## Processo de criação de Produtos


```
curl -X 'POST' \
  'http://localhost:5002/Product' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
    "name": "Coca cola",
    "price": 9.99,
    "description": "",
    "category": 2,
    "imageURL": "https://picsum.photos/200/300"
}'
```


Categoria de produtos:

0 - Lanche,
1 - Acompanhamento,
2 - Bebida,
3 - Sobremesa

## Processo de criação de Pedidos (checkout)


```
curl -X 'POST' \
  'http://localhost:5010/Order' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "products": [
    {
      "productId": "38935d22-41e1-44c4-9da3-b356f4b9e0d0",
      "quantity": 1,
      "total": 10,
      "comments": "string"
    }
  ],
  "total": 10
}'
```


Status de pagamentos:
0 - Pending
1 - Approved
2 - Declined



## Observações

Essa api não tem implementacao de sistema de pagamento.
Foi utilizado apenas um fake Service retornando true, sempre que tiver um novo pedido.
