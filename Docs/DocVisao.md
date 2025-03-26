# Documento de Visão: MindHaven

## 1. Objetivo
O objetivo deste projeto é a investigação da área da Saúde Mental, permintindo assim várias experiências personalizadas para os utilizadores, desde uma aplicação móvel para o dia-a-dia até a implementação da robótica.

## 2. Visão
Os nossos serviços poderão ser utilizados em diversos contextos e ambientes, como:
- **Escolas e Universidades**: Fornecendo suporte emocional e ferramentas de bem-estar para estudantes lidarem com o stress acadêmico.
- **Empresas e Ambientes Corporativos**: Promovendo pausas saudáveis com exercícios de relaxamento e acompanhamento do bem-estar dos funcionários.
- **Clínicas e Consultórios Psicológicos**: Ajuda aos profissionais de saúde mental com insights de IA sobre padrões emocionais dos pacientes.

## 3. Partes Interessadas (Stakeholders)
- **Utilizadores Finais**: Adolescentes e jovens adultos que buscam um suporte acessível para o bem-estar mental, aliado a uma experiência interativa e envolvente.
- **Proprietário do Sistema**: Administrador da plataforma, responsável pela manutenção e gerenciamento dos serviços.
- **Equipa de Desenvolvimento**: Responsáveis pela implementação e manutenção do sistema.
- **Anunciantes**: Empresas interessadas em promover os produtos e serviços na plataforma.

## 4. Equipa do Projeto
### 4.1 Equipa de Desenvolvimento
- **Desenvolvedor Frontend**: Responsável pela interface do usuário utilizando XAML no MAUI .NET para aplicativos Android.
- **Desenvolvedor Backend**: Responsável pelo servidor e lógica do sistema (C# para MAUI .NET, PHP + SQL para banco de dados).
- **Desenvolvedor de Robótica**: Programação do robô utilizando Arduino (C) e integração com outros sistemas.
- **Desenvolvedor de IA**: Implementação de modelos de Inteligência Artificial utilizando Python.
- **Designer Gráfico**: Criação de gráficos, layout e identidade visual do aplicativo e outros sistemas.
- **Gerente de Projeto**: Coordenação da equipe e planejamento do cronograma de desenvolvimento.
- **Testadores**: Garantir a qualidade e funcionalidade dos sistemas antes do lançamento.

## 5. Características do Sistema
### 5.1 Funcionalidades Principais
1. **Biblioteca de Jogos**
   - Catálogo de minijogos organizados por categorias (arcade, quebra-cabeças, ação, etc.).
   - Pesquisa e filtro de jogos por tipo.

2. **Tabela de Pontuações**
   - Sistema de tabela de pontos para jogos que necessitem, que exibe as melhores pontuações dos jogadores.
   - Rankings específicos para cada jogo, incentivando a competitividade.

3. **Perfis do Utilizador**
   - Registo com nome e password para personalização e armazenamento de dados do utilizador.

4. **Design Responsivo**
   - Interface adaptável a qualquer computador, garantindo uma experiência de jogo consistente.
     
5. **Personalização de Avatares**
   - Opção para que os utilizadores personalizem os seus perfis com avatares, skins ou outras formas de personalização visual, utilizando pontos adquiridas nos jogos.

### 5.2 Funcionalidades Adicionais (Opcional)
1. **Interatividade Social**
   - Partilha de Ranking global, Comentários entre utilizadores online.
2. **Histórico de Jogo**
   - Armazenamento do histórico de partidas para cada utilizador, permitindo que estes consultem o seu desempenho anterior, melhorem estratégias e comparem as suas estatísticas ao longo do tempo.

1. **Diário Emocional**
    - Escolha entre emoções disponíveis
    - Escreva notas/descrições
    - Adicione registos

2. **Reports Page**
    - Gráficos e análise de notas guardadas pelo utilizador

3. **Login & Registo**
    - Login com email e palavra-passe
    - Registo de conta e envio de email de boas-vindas

4. **Main Page**
    - Página inicial e menu interativo
    - Acesso ao Diário Emocional e Reports
    - Perfil do utilizador com imagem e dados

5. **Modo de Emergência Emocional**
    - Ativação de suporte rápido em momentos críticos

6. **Reconhecimento de Padrões & Análise Wear OS**
    - Análise comportamental integrada

## 6. Arquitetura de Referência
A arquitetura do sistema será composta pelos seguintes componentes principais:
- **Frontend**: Implementado com XAML no .NET MAUI, responsável pela interface do utilizador em dispositivos Android.
- **Backend**: Desenvolvido em C# (.NET MAUI) para a lógica do aplicativo e PHP para a comunicação com a base de dados.
- **Base de Dados**: Sistema de armazenamento em SQL (usando XAMPP para desenvolvimento), que guarda informações dos utilizadores, registos emocionais e estatísticas.
- **Robótica**: Controle do robô utilizando Arduino (C).
- **Inteligência Artificial**: Implementação de modelos de IA utilizando Python para análise de padrões emocionais.

## 7. Restrições do Produto
### 7.1 Restrições Identificadas
- **Desempenho**: A aplicação pode apresentar limitações em dispositivos Android mais antigos ou com hardware limitado.
- **Conectividade**: Dependência de internet para sincronização de dados com o servidor, podendo afetar a experiência offline.
- **Compatibilidade**: Algumas funcionalidades podem não ser totalmente compatíveis com versões antigas do Android ou hardware de baixo desempenho.
- **Processamento de IA**: Modelos de Inteligência Artificial exigem maior capacidade computacional, podendo impactar o tempo de resposta em dispositivos menos potentes.

## 8. Integração LLM (Large Language Model)
### 8.1 Utilização do LLM
- **Assistência ao Utilizador**: Utilizar um LLM (como o GPT-4) para fornecer suporte em tempo real, ajudando utilizadores com dúvidas sobre o uso do aplicativo, funcionalidades do Diário Emocional e interpretação de relatórios.
- **Análise Emocional**: Processar notas e descrições do utilizador para identificar padrões emocionais e fornecer insights personalizados.
- **Moderação e Monitoramento**: Monitoramento de entradas do utilizador para garantir um ambiente seguro e respeitoso.
- **Personalização de Experiência**: Adaptar sugestões com base no histórico emocional do utilizador, ajudando na gestão do bem-estar.
