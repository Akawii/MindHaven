## Modelo de Casos de Uso - MindHaven

### Para o Utilizador

#### Caso de Uso: Criação de Conta
- *Ator Principal*: Utilizador
- *Objetivo*: Criar uma conta para registar emoções e acompanhar o bem-estar mental.
- *Fluxo Principal*:
  1. O utilizador seleciona a opção "Criar Conta".
  2. O sistema solicita um email e uma palavra-passe.
  3. O utilizador insere as informações solicitadas.
  4. O sistema valida os dados e regista a nova conta.
  5. O utilizador recebe um email de boas-vindas.
- *Fluxo Alternativo*:
  - Caso o email já esteja registado, o sistema notifica o utilizador e sugere login.
  - Caso a palavra-passe seja fraca, o sistema sugere ajustes.

#### Caso de Uso: Login na Aplicação
- *Ator Principal*: Utilizador
- *Objetivo*: Aceder à aplicação para registar emoções e visualizar relatórios.
- *Fluxo Principal*:
  1. O utilizador insere o email e a palavra-passe.
  2. O sistema verifica as credenciais.
  3. Caso estejam corretas, o utilizador é redirecionado para a MainPage.
- *Fluxo Alternativo*:
  - Caso as credenciais sejam inválidas, o sistema exibe uma mensagem de erro.
  - Se o utilizador esquecer a palavra-passe, pode solicitar redefinição via email.

#### Caso de Uso: Registo de Emoções no Diário Emocional
- *Ator Principal*: Utilizador
- *Objetivo*: Registar emoções para acompanhamento do bem-estar.
- *Fluxo Principal*:
  1. O utilizador acede ao "Diário Emocional" na MainPage.
  2. O sistema apresenta uma seleção de emoções.
  3. O utilizador escolhe uma emoção.
  4. O utilizador pode escrever uma nota ou descrição sobre a emoção.
  5. O sistema guarda o registo.

#### Caso de Uso: Consulta de Relatórios e Gráficos
- *Ator Principal*: Utilizador
- *Objetivo*: Visualizar estatísticas sobre as emoções registadas.
- *Fluxo Principal*:
  1. O utilizador acede à "Reports Page".
  2. O sistema exibe gráficos e dados estatísticos sobre emoções registadas.
  3. O utilizador pode filtrar os dados por período de tempo.

### Para o Administrador

#### Caso de Uso: Gestão de Utilizadores
- *Ator Principal*: Administrador
- *Objetivo*: Gerir utilizadores da plataforma.
- *Fluxo Principal*:
  1. O administrador faz login no painel de administração.
  2. O sistema exibe uma lista de utilizadores registados.
  3. O administrador pode visualizar, editar ou remover contas conforme necessário.

#### Caso de Uso: Análise de Estatísticas de Uso
- *Ator Principal*: Administrador
- *Objetivo*: Avaliar a utilização da aplicação.
- *Fluxo Principal*:
  1. O administrador acede ao painel de estatísticas.
  2. O sistema exibe métricas como número de utilizadores ativos e frequência de registos.
  3. O administrador usa esses dados para melhorias na aplicação.
