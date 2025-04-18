# 🗣️ Fórum API

API RESTful desenvolvida com **.NET 8** para gerenciamento de um fórum de discussões, permitindo que usuários criem tópicos, comentem, curtam e filtrem discussões com base em parâmetros dinâmicos.  
A aplicação possui autenticação baseada em **JWT** e validações robustas de entrada, além de **testes unitários e de integração**.

---

##  🚀 Tecnologias Utilizadas

- **.NET 8**
- **Entity Framework Core**
- **FluentValidation**
- **JWT Bearer Authentication**
- **FluentMigrator**
- **xUnit + Moq** (Testes unitários e de integraão)
- **FluentAssertions** (Asserções mais legíveis e expressivas nos testes)
- **Bogus (Faker)** (Geração de dados falsos realistas para testes)
- **Swagger (Swashbuckle)** para documentação interativa da API

---

## 📚 Endpoints Disponíveis

### 🔐 Autenticação necessária

- `POST /login` — Autenticação do usuário
- `PUT /user` — Atualizar usuário
- `PUT /user/change-password` — Atualizar senha do usuário
- `POST /topic` — Criar novo tópico
- `PUT /topic/{id}` — Atualizar tópico (somente autor)
- `DELETE /topic/{id}` — Excluir tópico (somente autor)
- `POST /comment/{topicId}` — Criar comentário
- `PUT /comment/{topicId}/{commentId}` — Atualizar comentário (somente autor)
- `DELETE /comment/{topicId}/{commentId}` — Deletar comentário (somente autor)
- `POST /like/{topicId}` — Curtir/descurtir tópico

### ✅ Acesso público

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
