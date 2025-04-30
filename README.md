# 🗣️ Fórum API

API RESTful desenvolvida com **.NET 9** para gerenciamento de um fórum de discussões, permitindo que usuários criem tópicos, comentem, curtam e filtrem discussões com base em parâmetros dinâmicos.  
A aplicação possui autenticação baseada em **JWT** e validações robustas de entrada, além de **testes unitários e de integração**.

---

##  🚀 Tecnologias Utilizadas

- **.NET 9**
- **Entity Framework Core**
- **FluentValidation**
- **JWT Bearer Authentication**
- **FluentMigrator**
- **xUnit + Moq** (Testes unitários e de integraão)
- **FluentAssertions** (Asserções mais legíveis e expressivas nos testes)
- **Bogus (Faker)** (Geração de dados falsos realistas para testes)
- **Swagger (Swashbuckle)** para documentação interativa da API

---

## ☁️ Upload de Fotos com Cloudinary

O projeto possui suporte a upload e gerenciamento de fotos de perfil por meio do serviço externo Cloudinary.

Para que o recurso funcione corretamente, é necessário configurar as seguintes chaves no arquivo de configuração (appsettings.json):

``` bash
"Cloudinary": {
  "CloudName": "your_cloud_name",
  "ApiKey": "your_api_key",
  "ApiSecret": "your_api_secret"
}
```

As fotos são automaticamente redimensionadas e otimizadas antes de serem disponibilizadas por URL pública, facilitando a exibição no frontend.

---

## 📩 Orquestração de Exclusão de Conta com RabbitMQ (CloudAMQP)

Para tornar o processo de exclusão de conta mais resiliente e desacoplado, o projeto utiliza o RabbitMQ como broker de mensagens, por meio do serviço gerenciado CloudAMQP.

Através dessa abordagem baseada em filas, a exclusão de conta é orquestrada da seguinte forma:

1. Ao solicitar a exclusão, o usuário é inativado imediatamente.

2. Em seguida, uma mensagem é publicada na fila com o identificador do usuário.

3. Um serviço em segundo plano (implementado com BackgroundService) consome essa mensagem e realiza, de forma assíncrona, a exclusão definitiva dos dados do usuário e suas respectivas imagens no Cloudinary.

Para que o recurso funcione corretamente, é necessário configurar as seguintes chaves no arquivo de configuração (appsettings.json):

``` bash
"RabbitMQ": {
  "Connection": "your_connection_url",
  "QueueName": "your_queue_name"
}
```

---

## 🛡️ Trilhas de Auditoria

O projeto implementa um sistema de trilha de auditoria automática através da sobreposição do método SaveChangesAsync no DbContext.
Sempre que uma entidade for criada, atualizada ou removida, um registro de auditoria (Audit) é automaticamente gerado contendo:

- Tipo de operação realizada (Insert, Update, Delete);

- Nome da tabela afetada;

- Data e hora em que a operação foi realizada;

- Identificador do registro alterado;

- Lista de alterações nos campos (AuditEntry);

Essa abordagem proporciona rastreabilidade completa das modificações no sistema, mantendo a separação de responsabilidades entre as camadas e eliminando a necessidade de código repetitivo de auditoria nos casos de uso.

---

## 🚦 Controle de Requisições com Rate Limiting
O projeto implementa um mecanismo de rate limiting para limitar a quantidade de requisições por IP em um determinado intervalo de tempo, utilizando a API nativa de Rate Limiting do .NET.

A política de rate limit é flexível: em ambientes de teste, ela é automaticamente desativada para não interferir na execução de testes automatizados.

A configuração da política é realizada via implementação de IRateLimiterPolicy, que pode ser aplicada diretamente em controladores ou endpoints por meio do atributo [EnableRateLimiting("PolicyName")].

---

## 📚 Endpoints Disponíveis

### 🔐 Autenticação necessária

- `GET /user` — Obter perfil do usuário
- `PUT /user` — Atualizar usuário
- `PUT /user/change-password` — Atualizar senha do usuário
- `PUT /user/update-photo` — Atualizar foto do usuário
- `DELETE /user/delete-photo` — Deletar foto do usuário
- `DELETE /user/account` — Solicitar exclusão de conta do usuário
- `POST /topic` — Criar novo tópico
- `PUT /topic/{id}` — Atualizar tópico (somente autor)
- `DELETE /topic/{id}` — Excluir tópico (somente autor)
- `POST /comment/{topicId}` — Criar comentário
- `PUT /comment/{commentId}` — Atualizar comentário (somente autor)
- `DELETE /comment/{commentId}` — Deletar comentário (somente autor)
- `POST /like/{topicId}` — Curtir/descurtir tópico

### ✅ Acesso público

- `POST /login` — Autenticação do usuário
- `POST /user` — Criar novo usuário
- `GET /forum` — Listar todos os tópicos com detalhes completos
- `GET /topic/{id}` — Obter um tópico específico com seus comentários
- `POST /topic/filter` — Filtro dinâmico de tópicos
- `POST /token/refresh-token` — Obter um novo token

---

## 🔍 Filtro de Tópicos

O endpoint `POST /topic/filter` permite filtrar os tópicos por:

- **Título**
- **Conteúdo**
- **Ordenação dinâmica**, com opções como:
  - `"createdOn"` (data de criação - default)
  - `"likes"` (quantidade de curtidas)
  - `"comments"` (quantidade de comentários)
