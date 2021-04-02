Alterações necessárias.

Acessar o arquivo WEB.config -> 

Dentro do <appSettings></appSettings> procurar a linha que contém o seguinte código: <add key="CsvPath" value="D:\testeCSV\" />.
Alterar o Value para onde quer que o Arquivo CSV seja criado. O nome do Arquivo é gerado automatico apartir do nome do Cliente + a dataHora atual do servidor.
 
Dentro do <connectionStrings></connectionStrings> alterar as opções: 'Server' colocar o ip da maquina que vai rodar a aplicação, 'Port' colocar a porta do ip da maquina que vai rodar a aplicação, 'Database' colocar a Base que vai ser utilizada para salvar os dados, 'Uid' e 'Pwd' colocar o usuário e senha utilizado para acessar o bando de dados