# Shortener
## EN
A simple link shortener. Uses Dapper or Entity Framework Core through repositories for data access.
### Running
The project can be run right after cloning. It'll automatically create an SQLite database where the shortened links will be stored and Swagger will show a view for the endpoints. Using Dapper requires the database to already exist (either create it manually or execute the project once using EF Core, which will create the database).

## PT
Um simples encurtador de links. Usa Dapper ou Entity Framework Core através de repositórios para acessar dados.
### Rodando
O projeto pode ser executado logo após ser clonado. Um banco de dados SQLite será automaticamente criado e utilizado para salvar os links encurtados e o Swagger mostrará os endpoints. Para utilizar Dapper certifique-se de que o banco de dados já tenha sido criado, o que pode ser feito executando o projeto uma vez utilizando EF Core.