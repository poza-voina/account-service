---
title: AccountService.Api v1.0
language_tabs:
  - "": ""
language_clients:
  - "": ""
toc_footers: []
includes: []
search: true
highlight_theme: darkula
headingLevel: 2

---

<!-- Generator: Widdershins v4.0.1 -->

<h1 id="accountservice-api">AccountService.Api v1.0</h1>

> Scroll down for code samples, example requests and responses. Select a language for code samples from the tabs above or the mobile navigation menu.

# Authentication

- oAuth2 authentication. 

    - Flow: authorizationCode
    - Authorization URL = [http://localhost:8080/realms/application-realm/protocol/openid-connect/auth](http://localhost:8080/realms/application-realm/protocol/openid-connect/auth)
    - Token URL = [http://localhost:8080/realms/application-realm/protocol/openid-connect/token](http://localhost:8080/realms/application-realm/protocol/openid-connect/token)

|Scope|Scope Description|
|---|---|
|openid|OpenID scope|
|profile|User profile|

<h1 id="accountservice-api-account">Account</h1>

## post__accounts

> Code samples

`POST /accounts`

*Создает счет*

> Body parameter

```json
{
  "ownerId": "4d206909-730f-409a-88f6-dcfaa8fc28cc",
  "type": "Checking",
  "currency": "string",
  "interestRate": 0.1,
  "closingDate": "2019-08-24T14:15:22Z"
}
```

<h3 id="post__accounts-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[CreateAccountCommand](#schemacreateaccountcommand)|false|none|

> Example responses

> 200 Response

```
{"result":{"id":"497f6eca-6276-4993-bfeb-53cbbbba6f08","ownerId":"4d206909-730f-409a-88f6-dcfaa8fc28cc","type":"Checking","currency":"string","balance":0.1,"interestRate":0.1,"openingDate":"2019-08-24T14:15:22Z","closingDate":"2019-08-24T14:15:22Z"},"operationError":{"message":"string","stackTrace":"string","exceptionType":"string"},"validationErrors":[{"field":"string","message":"string"}],"statusCode":0}
```

```json
{
  "result": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "ownerId": "4d206909-730f-409a-88f6-dcfaa8fc28cc",
    "type": "Checking",
    "currency": "string",
    "balance": 0.1,
    "interestRate": 0.1,
    "openingDate": "2019-08-24T14:15:22Z",
    "closingDate": "2019-08-24T14:15:22Z"
  },
  "operationError": {
    "message": "string",
    "stackTrace": "string",
    "exceptionType": "string"
  },
  "validationErrors": [
    {
      "field": "string",
      "message": "string"
    }
  ],
  "statusCode": 0
}
```

<h3 id="post__accounts-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|[AccountViewModelMbResult](#schemaaccountviewmodelmbresult)|
|400|[Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)|Bad Request|[ObjectMbResult](#schemaobjectmbresult)|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|Unauthorized|[ObjectMbResult](#schemaobjectmbresult)|
|422|[Unprocessable Entity](https://tools.ietf.org/html/rfc2518#section-10.3)|Unprocessable Content|[ObjectMbResult](#schemaobjectmbresult)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
oauth2 ( Scopes: openid profile )
</aside>

## put__accounts

> Code samples

`PUT /accounts`

*Обновляет счет*

> Body parameter

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "closingDate": "2019-08-24T14:15:22Z",
  "interestRate": 0.1
}
```

<h3 id="put__accounts-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[PatchAccountCommand](#schemapatchaccountcommand)|false|none|

> Example responses

> 400 Response

```
{"result":null,"operationError":{"message":"string","stackTrace":"string","exceptionType":"string"},"validationErrors":[{"field":"string","message":"string"}],"statusCode":0}
```

```json
{
  "result": null,
  "operationError": {
    "message": "string",
    "stackTrace": "string",
    "exceptionType": "string"
  },
  "validationErrors": [
    {
      "field": "string",
      "message": "string"
    }
  ],
  "statusCode": 0
}
```

<h3 id="put__accounts-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|400|[Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)|Bad Request|[ObjectMbResult](#schemaobjectmbresult)|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|Unauthorized|[ObjectMbResult](#schemaobjectmbresult)|
|404|[Not Found](https://tools.ietf.org/html/rfc7231#section-6.5.4)|Not Found|[ObjectMbResult](#schemaobjectmbresult)|
|422|[Unprocessable Entity](https://tools.ietf.org/html/rfc2518#section-10.3)|Unprocessable Content|[ObjectMbResult](#schemaobjectmbresult)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
oauth2 ( Scopes: openid profile )
</aside>

## get__accounts

> Code samples

`GET /accounts`

*Получает список счетов*

<h3 id="get__accounts-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|query|query|[GetAccountsQuery](#schemagetaccountsquery)|false|none|

> Example responses

> 401 Response

```
{"result":null,"operationError":{"message":"string","stackTrace":"string","exceptionType":"string"},"validationErrors":[{"field":"string","message":"string"}],"statusCode":0}
```

```json
{
  "result": null,
  "operationError": {
    "message": "string",
    "stackTrace": "string",
    "exceptionType": "string"
  },
  "validationErrors": [
    {
      "field": "string",
      "message": "string"
    }
  ],
  "statusCode": 0
}
```

<h3 id="get__accounts-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|Unauthorized|[ObjectMbResult](#schemaobjectmbresult)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
oauth2 ( Scopes: openid profile )
</aside>

## delete__accounts_{id}

> Code samples

`DELETE /accounts/{id}`

*Удаляет счет*

<h3 id="delete__accounts_{id}-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|id|path|string(uuid)|true|Идентификатор счета|

> Example responses

> 401 Response

```
{"result":null,"operationError":{"message":"string","stackTrace":"string","exceptionType":"string"},"validationErrors":[{"field":"string","message":"string"}],"statusCode":0}
```

```json
{
  "result": null,
  "operationError": {
    "message": "string",
    "stackTrace": "string",
    "exceptionType": "string"
  },
  "validationErrors": [
    {
      "field": "string",
      "message": "string"
    }
  ],
  "statusCode": 0
}
```

<h3 id="delete__accounts_{id}-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|Unauthorized|[ObjectMbResult](#schemaobjectmbresult)|
|404|[Not Found](https://tools.ietf.org/html/rfc7231#section-6.5.4)|Not Found|[ObjectMbResult](#schemaobjectmbresult)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
oauth2 ( Scopes: openid profile )
</aside>

## get__accounts_{id}_exists

> Code samples

`GET /accounts/{id}/exists`

*Проверяет наличие счета*

<h3 id="get__accounts_{id}_exists-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|id|path|string(uuid)|true|Идентификатор счета|

> Example responses

> 200 Response

```
{"result":null,"operationError":{"message":"string","stackTrace":"string","exceptionType":"string"},"validationErrors":[{"field":"string","message":"string"}],"statusCode":0}
```

```json
{
  "result": null,
  "operationError": {
    "message": "string",
    "stackTrace": "string",
    "exceptionType": "string"
  },
  "validationErrors": [
    {
      "field": "string",
      "message": "string"
    }
  ],
  "statusCode": 0
}
```

<h3 id="get__accounts_{id}_exists-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|[ObjectMbResult](#schemaobjectmbresult)|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|Unauthorized|[ObjectMbResult](#schemaobjectmbresult)|
|404|[Not Found](https://tools.ietf.org/html/rfc7231#section-6.5.4)|Not Found|[ObjectMbResult](#schemaobjectmbresult)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
oauth2 ( Scopes: openid profile )
</aside>

<h1 id="accountservice-api-statement">Statement</h1>

## get__statements_{accountId}

> Code samples

`GET /statements/{accountId}`

*Получает выписку по счету*

<h3 id="get__statements_{accountid}-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|accountId|path|string(uuid)|true|Идентификатор счета|
|ownerId|query|string(uuid)|true|Идентификатор клиента|
|startDateTime|query|string(date-time)|false|Начала диапозона выписки|
|endDateTime|query|string(date-time)|false|Конец диапозона выписки|

> Example responses

> 200 Response

```
{"result":[{"id":"497f6eca-6276-4993-bfeb-53cbbbba6f08","ownerId":"4d206909-730f-409a-88f6-dcfaa8fc28cc","type":"Checking","currency":"string","balance":0.1,"interestRate":0.1,"openingDate":"2019-08-24T14:15:22Z","closingDate":"2019-08-24T14:15:22Z","transactions":[{"id":"497f6eca-6276-4993-bfeb-53cbbbba6f08","bankAccountId":"5b26b598-a880-4e32-8c41-126aa0206857","counterpartyBankAccountId":"cc9b712c-c0c3-4405-a86e-c2690000b458","amount":0.1,"currency":"string","type":"Credit","description":"string","createdAt":"2019-08-24T14:15:22Z","isApply":true}]}],"operationError":{"message":"string","stackTrace":"string","exceptionType":"string"},"validationErrors":[{"field":"string","message":"string"}],"statusCode":0}
```

```json
{
  "result": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "ownerId": "4d206909-730f-409a-88f6-dcfaa8fc28cc",
      "type": "Checking",
      "currency": "string",
      "balance": 0.1,
      "interestRate": 0.1,
      "openingDate": "2019-08-24T14:15:22Z",
      "closingDate": "2019-08-24T14:15:22Z",
      "transactions": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
          "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
          "amount": 0.1,
          "currency": "string",
          "type": "Credit",
          "description": "string",
          "createdAt": "2019-08-24T14:15:22Z",
          "isApply": true
        }
      ]
    }
  ],
  "operationError": {
    "message": "string",
    "stackTrace": "string",
    "exceptionType": "string"
  },
  "validationErrors": [
    {
      "field": "string",
      "message": "string"
    }
  ],
  "statusCode": 0
}
```

<h3 id="get__statements_{accountid}-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|[AccountWithTransactionsViewModelIEnumerableMbResult](#schemaaccountwithtransactionsviewmodelienumerablembresult)|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|Unauthorized|[ObjectMbResult](#schemaobjectmbresult)|
|404|[Not Found](https://tools.ietf.org/html/rfc7231#section-6.5.4)|Not Found|[ObjectMbResult](#schemaobjectmbresult)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
oauth2 ( Scopes: openid profile )
</aside>

<h1 id="accountservice-api-transaction">Transaction</h1>

## post__transactions_register

> Code samples

`POST /transactions/register`

*Зарегистрировать транзакцию*

> Body parameter

```json
{
  "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
  "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
  "amount": 0.1,
  "currency": "string",
  "type": "Credit",
  "description": "string"
}
```

<h3 id="post__transactions_register-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[RegisterTransactionCommand](#schemaregistertransactioncommand)|false|none|

> Example responses

> 200 Response

```
{"result":{"id":"497f6eca-6276-4993-bfeb-53cbbbba6f08","bankAccountId":"5b26b598-a880-4e32-8c41-126aa0206857","counterpartyBankAccountId":"cc9b712c-c0c3-4405-a86e-c2690000b458","amount":0.1,"currency":"string","type":"Credit","description":"string","createdAt":"2019-08-24T14:15:22Z","isApply":true},"operationError":{"message":"string","stackTrace":"string","exceptionType":"string"},"validationErrors":[{"field":"string","message":"string"}],"statusCode":0}
```

```json
{
  "result": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
    "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
    "amount": 0.1,
    "currency": "string",
    "type": "Credit",
    "description": "string",
    "createdAt": "2019-08-24T14:15:22Z",
    "isApply": true
  },
  "operationError": {
    "message": "string",
    "stackTrace": "string",
    "exceptionType": "string"
  },
  "validationErrors": [
    {
      "field": "string",
      "message": "string"
    }
  ],
  "statusCode": 0
}
```

<h3 id="post__transactions_register-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|200|[OK](https://tools.ietf.org/html/rfc7231#section-6.3.1)|OK|[TransactionViewModelMbResult](#schematransactionviewmodelmbresult)|
|400|[Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)|Bad Request|[ObjectMbResult](#schemaobjectmbresult)|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|Unauthorized|[ObjectMbResult](#schemaobjectmbresult)|
|422|[Unprocessable Entity](https://tools.ietf.org/html/rfc2518#section-10.3)|Unprocessable Content|[ObjectMbResult](#schemaobjectmbresult)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
oauth2 ( Scopes: openid profile )
</aside>

## post__transactions_transfer

> Code samples

`POST /transactions/transfer`

*Перенос между счетами*

> Body parameter

```json
{
  "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
  "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
  "amount": 0.1,
  "currency": "string",
  "description": "string"
}
```

<h3 id="post__transactions_transfer-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[TransferTransactionCommand](#schematransfertransactioncommand)|false|none|

> Example responses

> 400 Response

```
{"result":null,"operationError":{"message":"string","stackTrace":"string","exceptionType":"string"},"validationErrors":[{"field":"string","message":"string"}],"statusCode":0}
```

```json
{
  "result": null,
  "operationError": {
    "message": "string",
    "stackTrace": "string",
    "exceptionType": "string"
  },
  "validationErrors": [
    {
      "field": "string",
      "message": "string"
    }
  ],
  "statusCode": 0
}
```

<h3 id="post__transactions_transfer-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|400|[Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)|Bad Request|[ObjectMbResult](#schemaobjectmbresult)|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|Unauthorized|[ObjectMbResult](#schemaobjectmbresult)|
|404|[Not Found](https://tools.ietf.org/html/rfc7231#section-6.5.4)|Not Found|[ObjectMbResult](#schemaobjectmbresult)|
|422|[Unprocessable Entity](https://tools.ietf.org/html/rfc2518#section-10.3)|Unprocessable Content|[ObjectMbResult](#schemaobjectmbresult)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
oauth2 ( Scopes: openid profile )
</aside>

## post__transactions_execute

> Code samples

`POST /transactions/execute`

*Зарегистрировать и выполнить транзакцию*

> Body parameter

```json
{
  "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
  "amount": 0.1,
  "currency": "string",
  "type": "Credit",
  "description": "string"
}
```

<h3 id="post__transactions_execute-parameters">Parameters</h3>

|Name|In|Type|Required|Description|
|---|---|---|---|---|
|body|body|[ExecuteTransactionCommand](#schemaexecutetransactioncommand)|false|none|

> Example responses

> 400 Response

```
{"result":null,"operationError":{"message":"string","stackTrace":"string","exceptionType":"string"},"validationErrors":[{"field":"string","message":"string"}],"statusCode":0}
```

```json
{
  "result": null,
  "operationError": {
    "message": "string",
    "stackTrace": "string",
    "exceptionType": "string"
  },
  "validationErrors": [
    {
      "field": "string",
      "message": "string"
    }
  ],
  "statusCode": 0
}
```

<h3 id="post__transactions_execute-responses">Responses</h3>

|Status|Meaning|Description|Schema|
|---|---|---|---|
|400|[Bad Request](https://tools.ietf.org/html/rfc7231#section-6.5.1)|Bad Request|[ObjectMbResult](#schemaobjectmbresult)|
|401|[Unauthorized](https://tools.ietf.org/html/rfc7235#section-3.1)|Unauthorized|[ObjectMbResult](#schemaobjectmbresult)|
|404|[Not Found](https://tools.ietf.org/html/rfc7231#section-6.5.4)|Not Found|[ObjectMbResult](#schemaobjectmbresult)|
|422|[Unprocessable Entity](https://tools.ietf.org/html/rfc2518#section-10.3)|Unprocessable Content|[ObjectMbResult](#schemaobjectmbresult)|

<aside class="warning">
To perform this operation, you must be authenticated by means of one of the following methods:
oauth2 ( Scopes: openid profile )
</aside>

# Schemas

<h2 id="tocS_AccountType">AccountType</h2>
<!-- backwards compatibility -->
<a id="schemaaccounttype"></a>
<a id="schema_AccountType"></a>
<a id="tocSaccounttype"></a>
<a id="tocsaccounttype"></a>

```json
"Checking"

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|*anonymous*|string|false|none|none|

#### Enumerated Values

|Property|Value|
|---|---|
|*anonymous*|Checking|
|*anonymous*|Deposit|
|*anonymous*|Credit|

<h2 id="tocS_AccountViewModel">AccountViewModel</h2>
<!-- backwards compatibility -->
<a id="schemaaccountviewmodel"></a>
<a id="schema_AccountViewModel"></a>
<a id="tocSaccountviewmodel"></a>
<a id="tocsaccountviewmodel"></a>

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "ownerId": "4d206909-730f-409a-88f6-dcfaa8fc28cc",
  "type": "Checking",
  "currency": "string",
  "balance": 0.1,
  "interestRate": 0.1,
  "openingDate": "2019-08-24T14:15:22Z",
  "closingDate": "2019-08-24T14:15:22Z"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|id|string(uuid)|false|none|Индентификатор счета|
|ownerId|string(uuid)|false|none|Идентификатор клиента|
|type|[AccountType](#schemaaccounttype)|false|none|none|
|currency|string¦null|true|none|Валюта|
|balance|number(double)|false|none|Баланс счета|
|interestRate|number(double)¦null|false|none|Процентная ставка счета|
|openingDate|string(date-time)|false|none|Дата открытия счета|
|closingDate|string(date-time)¦null|false|none|Дата закрытия счета|

<h2 id="tocS_AccountViewModelMbResult">AccountViewModelMbResult</h2>
<!-- backwards compatibility -->
<a id="schemaaccountviewmodelmbresult"></a>
<a id="schema_AccountViewModelMbResult"></a>
<a id="tocSaccountviewmodelmbresult"></a>
<a id="tocsaccountviewmodelmbresult"></a>

```json
{
  "result": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "ownerId": "4d206909-730f-409a-88f6-dcfaa8fc28cc",
    "type": "Checking",
    "currency": "string",
    "balance": 0.1,
    "interestRate": 0.1,
    "openingDate": "2019-08-24T14:15:22Z",
    "closingDate": "2019-08-24T14:15:22Z"
  },
  "operationError": {
    "message": "string",
    "stackTrace": "string",
    "exceptionType": "string"
  },
  "validationErrors": [
    {
      "field": "string",
      "message": "string"
    }
  ],
  "statusCode": 0
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|result|[AccountViewModel](#schemaaccountviewmodel)|false|none|none|
|operationError|[OperationError](#schemaoperationerror)|false|none|none|
|validationErrors|[[Error](#schemaerror)]¦null|false|none|none|
|statusCode|integer(int32)|false|none|none|

<h2 id="tocS_AccountWithTransactionsViewModel">AccountWithTransactionsViewModel</h2>
<!-- backwards compatibility -->
<a id="schemaaccountwithtransactionsviewmodel"></a>
<a id="schema_AccountWithTransactionsViewModel"></a>
<a id="tocSaccountwithtransactionsviewmodel"></a>
<a id="tocsaccountwithtransactionsviewmodel"></a>

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "ownerId": "4d206909-730f-409a-88f6-dcfaa8fc28cc",
  "type": "Checking",
  "currency": "string",
  "balance": 0.1,
  "interestRate": 0.1,
  "openingDate": "2019-08-24T14:15:22Z",
  "closingDate": "2019-08-24T14:15:22Z",
  "transactions": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
      "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
      "amount": 0.1,
      "currency": "string",
      "type": "Credit",
      "description": "string",
      "createdAt": "2019-08-24T14:15:22Z",
      "isApply": true
    }
  ]
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|id|string(uuid)|false|none|Индентификатор счета|
|ownerId|string(uuid)|false|none|Идентификатор клиента|
|type|[AccountType](#schemaaccounttype)|false|none|none|
|currency|string¦null|true|none|Валюта|
|balance|number(double)|false|none|Баланс счета|
|interestRate|number(double)¦null|false|none|Процентная ставка счета|
|openingDate|string(date-time)|false|none|Дата открытия счета|
|closingDate|string(date-time)¦null|false|none|Дата закрытия счета|
|transactions|[[TransactionViewModel](#schematransactionviewmodel)]¦null|false|none|Коллекция транзакций|

<h2 id="tocS_AccountWithTransactionsViewModelIEnumerableMbResult">AccountWithTransactionsViewModelIEnumerableMbResult</h2>
<!-- backwards compatibility -->
<a id="schemaaccountwithtransactionsviewmodelienumerablembresult"></a>
<a id="schema_AccountWithTransactionsViewModelIEnumerableMbResult"></a>
<a id="tocSaccountwithtransactionsviewmodelienumerablembresult"></a>
<a id="tocsaccountwithtransactionsviewmodelienumerablembresult"></a>

```json
{
  "result": [
    {
      "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
      "ownerId": "4d206909-730f-409a-88f6-dcfaa8fc28cc",
      "type": "Checking",
      "currency": "string",
      "balance": 0.1,
      "interestRate": 0.1,
      "openingDate": "2019-08-24T14:15:22Z",
      "closingDate": "2019-08-24T14:15:22Z",
      "transactions": [
        {
          "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
          "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
          "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
          "amount": 0.1,
          "currency": "string",
          "type": "Credit",
          "description": "string",
          "createdAt": "2019-08-24T14:15:22Z",
          "isApply": true
        }
      ]
    }
  ],
  "operationError": {
    "message": "string",
    "stackTrace": "string",
    "exceptionType": "string"
  },
  "validationErrors": [
    {
      "field": "string",
      "message": "string"
    }
  ],
  "statusCode": 0
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|result|[[AccountWithTransactionsViewModel](#schemaaccountwithtransactionsviewmodel)]¦null|false|none|none|
|operationError|[OperationError](#schemaoperationerror)|false|none|none|
|validationErrors|[[Error](#schemaerror)]¦null|false|none|none|
|statusCode|integer(int32)|false|none|none|

<h2 id="tocS_CreateAccountCommand">CreateAccountCommand</h2>
<!-- backwards compatibility -->
<a id="schemacreateaccountcommand"></a>
<a id="schema_CreateAccountCommand"></a>
<a id="tocScreateaccountcommand"></a>
<a id="tocscreateaccountcommand"></a>

```json
{
  "ownerId": "4d206909-730f-409a-88f6-dcfaa8fc28cc",
  "type": "Checking",
  "currency": "string",
  "interestRate": 0.1,
  "closingDate": "2019-08-24T14:15:22Z"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|ownerId|string(uuid)|true|none|Идентификатор клиента|
|type|[AccountType](#schemaaccounttype)|true|none|none|
|currency|string¦null|true|none|Валюта|
|interestRate|number(double)¦null|false|none|Процентная ставка|
|closingDate|string(date-time)¦null|false|none|Дата закрытия счета|

<h2 id="tocS_Error">Error</h2>
<!-- backwards compatibility -->
<a id="schemaerror"></a>
<a id="schema_Error"></a>
<a id="tocSerror"></a>
<a id="tocserror"></a>

```json
{
  "field": "string",
  "message": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|field|string¦null|true|none|none|
|message|string¦null|true|none|none|

<h2 id="tocS_ExecuteTransactionCommand">ExecuteTransactionCommand</h2>
<!-- backwards compatibility -->
<a id="schemaexecutetransactioncommand"></a>
<a id="schema_ExecuteTransactionCommand"></a>
<a id="tocSexecutetransactioncommand"></a>
<a id="tocsexecutetransactioncommand"></a>

```json
{
  "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
  "amount": 0.1,
  "currency": "string",
  "type": "Credit",
  "description": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|bankAccountId|string(uuid)|true|none|Идентификатор счета|
|amount|number(double)|true|none|Количество денег|
|currency|string¦null|true|none|Валюта|
|type|[TransactionType](#schematransactiontype)|true|none|none|
|description|string¦null|true|none|Описание транзакции|

<h2 id="tocS_GetAccountsQuery">GetAccountsQuery</h2>
<!-- backwards compatibility -->
<a id="schemagetaccountsquery"></a>
<a id="schema_GetAccountsQuery"></a>
<a id="tocSgetaccountsquery"></a>
<a id="tocsgetaccountsquery"></a>

```json
{}

```

### Properties

*None*

<h2 id="tocS_ObjectMbResult">ObjectMbResult</h2>
<!-- backwards compatibility -->
<a id="schemaobjectmbresult"></a>
<a id="schema_ObjectMbResult"></a>
<a id="tocSobjectmbresult"></a>
<a id="tocsobjectmbresult"></a>

```json
{
  "result": null,
  "operationError": {
    "message": "string",
    "stackTrace": "string",
    "exceptionType": "string"
  },
  "validationErrors": [
    {
      "field": "string",
      "message": "string"
    }
  ],
  "statusCode": 0
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|result|any|false|none|none|
|operationError|[OperationError](#schemaoperationerror)|false|none|none|
|validationErrors|[[Error](#schemaerror)]¦null|false|none|none|
|statusCode|integer(int32)|false|none|none|

<h2 id="tocS_OperationError">OperationError</h2>
<!-- backwards compatibility -->
<a id="schemaoperationerror"></a>
<a id="schema_OperationError"></a>
<a id="tocSoperationerror"></a>
<a id="tocsoperationerror"></a>

```json
{
  "message": "string",
  "stackTrace": "string",
  "exceptionType": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|message|string¦null|true|none|none|
|stackTrace|string¦null|false|none|none|
|exceptionType|string¦null|false|none|none|

<h2 id="tocS_PatchAccountCommand">PatchAccountCommand</h2>
<!-- backwards compatibility -->
<a id="schemapatchaccountcommand"></a>
<a id="schema_PatchAccountCommand"></a>
<a id="tocSpatchaccountcommand"></a>
<a id="tocspatchaccountcommand"></a>

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "closingDate": "2019-08-24T14:15:22Z",
  "interestRate": 0.1
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|id|string(uuid)|true|none|Идентификатор счета|
|closingDate|string(date-time)¦null|false|none|Дата закрытия счета|
|interestRate|number(double)¦null|false|none|Процентная ставка|

<h2 id="tocS_RegisterTransactionCommand">RegisterTransactionCommand</h2>
<!-- backwards compatibility -->
<a id="schemaregistertransactioncommand"></a>
<a id="schema_RegisterTransactionCommand"></a>
<a id="tocSregistertransactioncommand"></a>
<a id="tocsregistertransactioncommand"></a>

```json
{
  "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
  "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
  "amount": 0.1,
  "currency": "string",
  "type": "Credit",
  "description": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|bankAccountId|string(uuid)|true|none|Идентификатор счета|
|counterpartyBankAccountId|string(uuid)¦null|false|none|Идентификатор счета контрагента|
|amount|number(double)|true|none|Количество денег|
|currency|string¦null|true|none|Валюта|
|type|[TransactionType](#schematransactiontype)|true|none|none|
|description|string¦null|true|none|Описание транзакции|

<h2 id="tocS_TransactionType">TransactionType</h2>
<!-- backwards compatibility -->
<a id="schematransactiontype"></a>
<a id="schema_TransactionType"></a>
<a id="tocStransactiontype"></a>
<a id="tocstransactiontype"></a>

```json
"Credit"

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|*anonymous*|string|false|none|none|

#### Enumerated Values

|Property|Value|
|---|---|
|*anonymous*|Credit|
|*anonymous*|Debit|

<h2 id="tocS_TransactionViewModel">TransactionViewModel</h2>
<!-- backwards compatibility -->
<a id="schematransactionviewmodel"></a>
<a id="schema_TransactionViewModel"></a>
<a id="tocStransactionviewmodel"></a>
<a id="tocstransactionviewmodel"></a>

```json
{
  "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
  "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
  "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
  "amount": 0.1,
  "currency": "string",
  "type": "Credit",
  "description": "string",
  "createdAt": "2019-08-24T14:15:22Z",
  "isApply": true
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|id|string(uuid)|true|none|Идентификатор транзакции|
|bankAccountId|string(uuid)|true|none|Идентификатор счета|
|counterpartyBankAccountId|string(uuid)¦null|false|none|Идентификатор счета контрагента|
|amount|number(double)|true|none|Количество денег|
|currency|string¦null|true|none|Валюта|
|type|[TransactionType](#schematransactiontype)|true|none|none|
|description|string¦null|true|none|Описание|
|createdAt|string(date-time)|true|none|Дата создания транзакции|
|isApply|boolean|true|none|Флаг принятия транзакции|

<h2 id="tocS_TransactionViewModelMbResult">TransactionViewModelMbResult</h2>
<!-- backwards compatibility -->
<a id="schematransactionviewmodelmbresult"></a>
<a id="schema_TransactionViewModelMbResult"></a>
<a id="tocStransactionviewmodelmbresult"></a>
<a id="tocstransactionviewmodelmbresult"></a>

```json
{
  "result": {
    "id": "497f6eca-6276-4993-bfeb-53cbbbba6f08",
    "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
    "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
    "amount": 0.1,
    "currency": "string",
    "type": "Credit",
    "description": "string",
    "createdAt": "2019-08-24T14:15:22Z",
    "isApply": true
  },
  "operationError": {
    "message": "string",
    "stackTrace": "string",
    "exceptionType": "string"
  },
  "validationErrors": [
    {
      "field": "string",
      "message": "string"
    }
  ],
  "statusCode": 0
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|result|[TransactionViewModel](#schematransactionviewmodel)|false|none|none|
|operationError|[OperationError](#schemaoperationerror)|false|none|none|
|validationErrors|[[Error](#schemaerror)]¦null|false|none|none|
|statusCode|integer(int32)|false|none|none|

<h2 id="tocS_TransferTransactionCommand">TransferTransactionCommand</h2>
<!-- backwards compatibility -->
<a id="schematransfertransactioncommand"></a>
<a id="schema_TransferTransactionCommand"></a>
<a id="tocStransfertransactioncommand"></a>
<a id="tocstransfertransactioncommand"></a>

```json
{
  "bankAccountId": "5b26b598-a880-4e32-8c41-126aa0206857",
  "counterpartyBankAccountId": "cc9b712c-c0c3-4405-a86e-c2690000b458",
  "amount": 0.1,
  "currency": "string",
  "description": "string"
}

```

### Properties

|Name|Type|Required|Restrictions|Description|
|---|---|---|---|---|
|bankAccountId|string(uuid)|true|none|Идентификатор счета|
|counterpartyBankAccountId|string(uuid)|true|none|Идентификато счета контрагента|
|amount|number(double)|true|none|Количество денег|
|currency|string¦null|true|none|Валюта|
|description|string¦null|true|none|Описание транзакции|

