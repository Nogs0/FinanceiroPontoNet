# Decisão da Arquitetura utilizada - Backend
Optei por utilizar uma arquitetura semelhante à Clean Architecture. Dividindo o projeto em 4 principais camadas, Domain, Application, Infrastructure e Web. 

## Com isso em mente pensei nas seguintes camadas:
 - ### Domain
    - Onde organizei as entidades da aplicação, e as interfaces para que os repositórios às implementassem
    - Criei uma interface IRepository, localizada em **Shared/**  que contém as operações que considerei básicas para o funcionamento de um CRUD, o que evita a duplicação de código para cada model.
    - Criei a intefaces ISoftDelete que será utilizada como um em _Global Query Filter_ do Entity Framework Core, localizada em **Shared/**
        - ISoftDelete foi criada visando um mecanismo de backup para caso algum registro seja excluído por engano, algo que já me ajudou em minha carreira como desenvolvedor e a **principal**.
    - Criei a classe FullAuditedEntiy, criada com o objetivo de possibilitar um mecanismo de auditoria no sistema, tendo rastreado quando foi a última modificação e podendo ser melhorada para abrir um UserId para cada ação, deixando o rastreio claro. 
 - ### Infrastructure
    - **/Persistence**: Aqui temos a configuração do bancos de dados, AppDbContext, junto às implementações dos repositories, a separação é realizada por pastas com os nomes das entidades. 
    Ex: 
    
        ![alt text](/imgsReadme/estruturaRepositories.png)
    - Seguindo a mesma ideia da criação da IRepository que contém os protótipos das funções básicas de CRUD, criei a Repository, uma classe abstrata que contém a implementação da interface. Esta que por sua vez é herdada pelos outros repositories, também evitando a duplicação de código.
    - No AppDbContext é onde os _Global Query Filters_ são aplicados, utilizando o ISoftDelete.
    - Da mesma forma é onde interceptamos as operações do EF para que quando um novo registro seja adicionado ou atualizado injetarmos as datas em que a criação ou modificação ocorreu e para quando um registro é excluído, injetarmos o DeletedAt ao invés de realizar o hard delete.
    - Também é o lugar onde foi adicionado o controle para que o código do banco seja único.

    
 - ### Application
    - **/Services**: Onde as regras para o funcionamento da aplicação se concentram, operações de CRUD para cada Entidade da plataforma e outras que forem necessárias.
    - Também temos nessa camada os DTOs (Data Transfer Object) que são responsáveis por trafegar os dados dentro da aplicação.

 - ### Web (API)
    - **/Controllers**: A camada que fornece os endpoints, que chamam seus respectivos Services para realizar as operações desejadas.
    - **/Middlewares**: Uma importante camada que contém uma classe customizada que atua no pipeline de requisições.
        - ExceptionHandlerMiddleware, como o próprio nome diz, é um handler de exceções, temos um tipo de exceção customizada, NotFoundException.


# Lista de bibliotecas de terceiros utilizadas
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.EntityFrameworkCore.Design
- Npgsql.EntityFrameworkCore.PostgreSQL
- Swashbuckle.AspNetCore
